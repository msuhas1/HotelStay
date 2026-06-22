namespace HotelStay.Api.Models;

public class HotelBookingResponse
{
    public string BookingReference { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public RoomType RoomType { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public string ConfirmationMessage { get; set; } = string.Empty;
}
