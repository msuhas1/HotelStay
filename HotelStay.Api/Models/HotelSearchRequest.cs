namespace HotelStay.Api.Models;

public class HotelSearchRequest
{
    public string Destination { get; set; } = string.Empty;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public RoomType? RoomType { get; set; }
}
