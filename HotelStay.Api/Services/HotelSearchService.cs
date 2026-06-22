using HotelStay.Api.Models;
using HotelStay.Api.Providers;

namespace HotelStay.Api.Services;

public class HotelSearchService
{
    private readonly IEnumerable<IHotelProvider> _providers;

    public HotelSearchService(IEnumerable<IHotelProvider> providers)
    {
        _providers = providers;
    }

    public async Task<IEnumerable<HotelSearchResult>> SearchAsync(HotelSearchRequest request, CancellationToken cancellationToken)
    {
        var results = new List<HotelSearchResult>();

        foreach (var provider in _providers)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var providerResults = await provider.SearchAsync(request, cancellationToken);
            results.AddRange(providerResults);
        }

        if (request.RoomType.HasValue)
        {
            results = results.Where(r => r.RoomType == request.RoomType.Value).ToList();
        }

        return results.OrderBy(r => r.TotalPrice).ToList();
    }
}
