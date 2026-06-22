using HotelStay.Api.Models;

namespace HotelStay.Api.Services;

public class BookingRepository
{
    private readonly List<BookingRecord> _bookings = new();

    public void Add(BookingRecord booking)
    {
        _bookings.Add(booking);
    }

    public BookingRecord? Get(string reference)
    {
        return _bookings.SingleOrDefault(b => b.Reference == reference);
    }
}
