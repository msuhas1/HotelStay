namespace HotelStay.Api.Models;

public class HotelBookingRequest
{
    public string ProviderName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public RoomType RoomType { get; set; }
    public string PassengerName { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
}
