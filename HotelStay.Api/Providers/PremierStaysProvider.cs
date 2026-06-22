using HotelStay.Api.Models;

namespace HotelStay.Api.Providers;

public class PremierStaysProvider : IHotelProvider
{
    public string Name => "PremierStays";

    public Task<IEnumerable<HotelSearchResult>> SearchAsync(HotelSearchRequest request, CancellationToken cancellationToken)
    {
        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var results = new List<HotelSearchResult>
        {
            new()
            {
                ProviderName = Name,
                HotelName = "Premier Grand Hotel",
                Destination = request.Destination,
                RoomType = RoomType.Standard,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = 120m * nights,
                Currency = "USD",
                CancellationPolicy = "Free cancellation up to 48 hours before check-in"
            },
            new()
            {
                ProviderName = Name,
                HotelName = "Premier Deluxe Suites",
                Destination = request.Destination,
                RoomType = RoomType.Deluxe,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = 220m * nights,
                Currency = "USD",
                CancellationPolicy = "Free cancellation up to 72 hours before check-in"
            }
        };

        return Task.FromResult<IEnumerable<HotelSearchResult>>(results);
    }
}
