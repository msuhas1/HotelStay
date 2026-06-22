using HotelStay.Api.Models;

namespace HotelStay.Api.Providers;

public class BoutiqueCollectionProvider : IHotelProvider
{
    public string Name => "BoutiqueCollection";

    public Task<IEnumerable<HotelSearchResult>> SearchAsync(HotelSearchRequest request, CancellationToken cancellationToken)
    {
        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var results = new List<HotelSearchResult>
        {
            new()
            {
                ProviderName = Name,
                HotelName = "Boutique Collection Resort",
                Destination = request.Destination,
                RoomType = RoomType.Deluxe,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = 190m * nights,
                Currency = "USD",
                CancellationPolicy = "Free cancellation up to 24 hours before check-in"
            }
        };

        return Task.FromResult<IEnumerable<HotelSearchResult>>(results);
    }
}
