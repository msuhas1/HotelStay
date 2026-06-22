using HotelStay.Api.Models;
using HotelStay.Api.Providers;
using HotelStay.Api.Services;
using HotelStay.Api.JsonConverters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        // Allow any localhost origin (any port) for local development.
        policy.SetIsOriginAllowed(origin =>
        {
            try
            {
                var uri = new Uri(origin);
                return uri.IsLoopback || string.Equals(uri.Host, "localhost", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddSingleton<BookingRepository>();
builder.Services.AddSingleton<IHotelProvider, PremierStaysProvider>();
builder.Services.AddSingleton<IHotelProvider, BudgetNestsProvider>();
builder.Services.AddSingleton<IHotelProvider, BoutiqueCollectionProvider>();
builder.Services.AddSingleton<HotelSearchService>();
builder.Services.AddSingleton<HotelBookingService>();

// Ensure DateOnly is correctly (de)serialized from/to JSON (e.g. "2026-07-10")
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

var app = builder.Build();

app.UseCors("AllowAngularClient");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/hotels/search", async (HttpRequest httpRequest, HotelSearchService searchService, CancellationToken cancellationToken) =>
{
    if (!httpRequest.Query.TryGetValue("destination", out var destinationValues) || string.IsNullOrWhiteSpace(destinationValues.ToString()))
    {
        return Results.BadRequest(new { error = "destination is required" });
    }

    var destination = destinationValues.ToString();

    if (!httpRequest.Query.TryGetValue("checkIn", out var checkInValues) || !DateOnly.TryParse(checkInValues.ToString(), out var checkIn))
    {
        return Results.BadRequest(new { error = "checkIn is required and must be a valid date" });
    }

    if (!httpRequest.Query.TryGetValue("checkOut", out var checkOutValues) || !DateOnly.TryParse(checkOutValues.ToString(), out var checkOut))
    {
        return Results.BadRequest(new { error = "checkOut is required and must be a valid date" });
    }

    if (checkOut <= checkIn)
    {
        return Results.BadRequest(new { error = "checkOut must be after checkIn" });
    }

    RoomType? roomType = null;
    if (httpRequest.Query.TryGetValue("roomType", out var roomTypeValues) && !string.IsNullOrWhiteSpace(roomTypeValues.ToString()))
    {
        if (!Enum.TryParse<RoomType>(roomTypeValues.ToString(), true, out var parsedRoomType))
        {
            return Results.BadRequest(new { error = "roomType is invalid" });
        }

        roomType = parsedRoomType;
    }

    var request = new HotelSearchRequest
    {
        Destination = destination,
        CheckIn = checkIn,
        CheckOut = checkOut,
        RoomType = roomType
    };

    var results = await searchService.SearchAsync(request, cancellationToken);
    return Results.Ok(results);
});

app.MapPost("/hotels/book", async (HotelBookingRequest bookingRequest, HotelBookingService bookingService) =>
{
    if (string.IsNullOrWhiteSpace(bookingRequest.ProviderName) || string.IsNullOrWhiteSpace(bookingRequest.Destination) || string.IsNullOrWhiteSpace(bookingRequest.PassengerName) || string.IsNullOrWhiteSpace(bookingRequest.DocumentNumber))
    {
        return Results.BadRequest(new { error = "providerName, destination, passengerName, and documentNumber are required" });
    }

    if (bookingRequest.CheckOut <= bookingRequest.CheckIn)
    {
        return Results.BadRequest(new { error = "checkOut must be after checkIn" });
    }

    if (!DestinationMetadata.All.Contains(bookingRequest.Destination, StringComparer.OrdinalIgnoreCase))
    {
        return Results.BadRequest(new { error = "destination is not supported" });
    }

    if (!DocumentValidationService.IsValidForDestination(bookingRequest.Destination, bookingRequest.DocumentType, out var documentError))
    {
        return Results.UnprocessableEntity(new { error = documentError });
    }

    try
    {
        var created = await bookingService.BookAsync(bookingRequest, CancellationToken.None);
        return Results.Ok(created);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/hotels/booking/{reference}", (string reference, HotelBookingService bookingService) =>
{
    var booking = bookingService.GetBooking(reference);
    return booking is not null ? Results.Ok(booking) : Results.NotFound(new { error = "Booking reference not found" });
});

app.MapGet("/", () => Results.Ok(new { message = "HotelStay API is running. Use /hotels/search to search hotels." }));

app.Run();
