# Reflection

## Implementation choices
- Used .NET Minimal API for a light backend implementation.
- Built provider stubs as deterministic classes to satisfy the challenge requirements.
- Normalized provider results into a shared DTO.
- Document validation is centralized in `DocumentValidationService`.

## Live tweak
- Added `BoutiqueCollectionProvider` without changing aggregation or provider interface.
- Registered only the new provider in DI.

## Copilot usage
- Copilot generated the initial model and service boilerplate.
- Manual review ensured validation and provider filtering met the challenge constraints.
