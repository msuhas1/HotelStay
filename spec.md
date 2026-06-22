# HotelStay Specification

## Backend Requirements
- `GET /hotels/search?destination={city}&checkIn={date}&checkOut={date}&roomType={type}`
- `POST /hotels/book`
- `GET /hotels/booking/{reference}`

### Validation
- `destination`, `checkIn`, `checkOut` required.
- `checkOut` must be after `checkIn`.
- `roomType` optional.
- International travel requires `Passport`.
- Domestic travel accepts `Passport` or `NationalId`.

## Providers
- `PremierStays`: PascalCase response, full details, all room types available.
- `BudgetNests`: snake_case response, minimal details, may return unavailable rooms.
- `BoutiqueCollection`: new provider with boutique fees, Deluxe and Suite only.

## Booking
- Must validate document rules server-side.
- Should return booking reference, provider, total price, cancellation policy.
