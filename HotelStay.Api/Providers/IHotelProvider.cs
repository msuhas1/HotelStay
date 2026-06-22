using HotelStay.Api.Models;

namespace HotelStay.Api.Providers;

public interface IHotelProvider
{
    string Name { get; }
    Task<IEnumerable<HotelSearchResult>> SearchAsync(HotelSearchRequest request, CancellationToken cancellationToken);
}
