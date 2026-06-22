using HotelStay.Api.Models;

namespace HotelStay.Api.Providers;

public class BudgetNestsProvider : IHotelProvider
{
    public string Name => "BudgetNests";

    public Task<IEnumerable<HotelSearchResult>> SearchAsync(HotelSearchRequest request, CancellationToken cancellationToken)
    {
        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var results = new List<HotelSearchResult>
        {
            new()
            {
                ProviderName = Name,
                HotelName = "Budget Nests Economy",
                Destination = request.Destination,
                RoomType = RoomType.Standard,
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                TotalPrice = 80m * nights,
                Currency = "USD",
                CancellationPolicy = "Non-refundable"
            }
        };

        if (request.RoomType.HasValue && request.RoomType.Value == RoomType.Deluxe)
        {
            results = new List<HotelSearchResult>();
        }

        return Task.FromResult<IEnumerable<HotelSearchResult>>(results);
    }
}
