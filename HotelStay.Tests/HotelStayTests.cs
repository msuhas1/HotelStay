using System;
using System.Threading;
using System.Threading.Tasks;
using HotelStay.Api.Models;
using HotelStay.Api.Providers;
using HotelStay.Api.Services;
using Xunit;

namespace HotelStay.Tests;

public class HotelStayTests
{
    [Fact]
    public async Task SearchService_filters_unavailable_budget_nests_rooms()
    {
        var providers = new IHotelProvider[] { new PremierStaysProvider(), new BudgetNestsProvider() };
        var service = new HotelSearchService(providers);

        var request = new HotelSearchRequest
        {
            Destination = "London",
            CheckIn = DateOnly.Parse("2026-07-01"),
            CheckOut = DateOnly.Parse("2026-07-04")
        };

        var results = await service.SearchAsync(request, CancellationToken.None);

        Assert.Contains(results, r => r.ProviderName == "BudgetNests" && r.RoomType == RoomType.Standard);
        Assert.DoesNotContain(results, r => r.ProviderName == "BudgetNests" && r.RoomType == RoomType.Deluxe);
    }

    [Fact]
    public void DocumentValidation_service_rejects_national_id_for_international_destinations()
    {
        var isValid = DocumentValidationService.IsValidForDestination("Paris", DocumentType.NationalId, out var message);

        Assert.False(isValid);
        Assert.Equal("International travel requires Passport.", message);
    }

    [Fact]
    public void DocumentValidation_service_accepts_passport_for_international_destinations()
    {
        var isValid = DocumentValidationService.IsValidForDestination("Tokyo", DocumentType.Passport, out var message);

        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }

    [Fact]
    public void DocumentValidation_service_accepts_national_id_for_domestic_destinations()
    {
        var isValid = DocumentValidationService.IsValidForDestination("London", DocumentType.NationalId, out var message);

        Assert.True(isValid);
        Assert.Equal(string.Empty, message);
    }
}
