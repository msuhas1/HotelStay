using HotelStay.Api.Models;
using HotelStay.Api.Providers;

namespace HotelStay.Api.Services;

public class HotelBookingService
{
    private readonly BookingRepository _repository;
    private readonly IEnumerable<IHotelProvider> _providers;

    public HotelBookingService(BookingRepository repository, IEnumerable<IHotelProvider> providers)
    {
        _repository = repository;
        _providers = providers;
    }

    public Task<HotelBookingResponse> BookAsync(HotelBookingRequest request, CancellationToken cancellationToken)
    {
        var provider = _providers.SingleOrDefault(p => p.Name.Equals(request.ProviderName, StringComparison.OrdinalIgnoreCase));
        if (provider is null)
        {
            throw new InvalidOperationException("Provider not found");
        }

        var booking = new HotelBookingResponse
        {
            BookingReference = Guid.NewGuid().ToString("N"),
            ProviderName = provider.Name,
            Destination = request.Destination,
            PassengerName = request.PassengerName,
            RoomType = request.RoomType,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            TotalPrice = 999m,
            ConfirmationMessage = "Booking confirmed"
        };

        _repository.Add(new BookingRecord { Reference = booking.BookingReference, Response = booking });
        return Task.FromResult(booking);
    }

    public HotelBookingResponse? GetBooking(string reference)
    {
        return _repository.Get(reference)?.Response;
    }
}
