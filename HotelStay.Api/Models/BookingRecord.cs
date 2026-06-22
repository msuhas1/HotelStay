namespace HotelStay.Api.Models;

public class BookingRecord
{
    public string Reference { get; set; } = string.Empty;
    public HotelBookingResponse Response { get; set; } = new HotelBookingResponse();
}
