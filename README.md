# HotelStay - Hotel Search & Booking

## Overview
This solution implements a hotel search and booking API for the SkyRoute Travel Platform.

## Architecture
- Backend: .NET Minimal API (`HotelStay.Api`)
- Tests: xUnit (`HotelStay.Tests`)
- Frontend: Angular shell (`hotelstay-ui`)

### Provider abstraction
- `IHotelProvider` defines search/booking operations.
- `PremierStaysProvider`, `BudgetNestsProvider`, and `BoutiqueCollectionProvider` are deterministic stubs.
- New providers can be added by implementing `IHotelProvider` and registering in DI only.

### Data flow
- Search endpoint queries all providers.
- BudgetNests unavailable rooms are filtered.
- Search results are normalized to `HotelSearchResult`.
- Booking endpoint validates request and documents.
- Booking records are kept in-memory.

## Copilot usage
Copilot was used to scaffold project files, generate models, provider stubs, and service patterns. Naming and validation logic were refined manually.

## Running the solution
1. Build backend and tests: `dotnet build HotelStay.sln`
2. Run tests: `dotnet test HotelStay.Tests\HotelStay.Tests.csproj`

## Notes
- The Angular frontend is scaffolded manually due to npm not being available in this environment.
- Destination metadata includes at least 2 domestic and 3 international cities.
