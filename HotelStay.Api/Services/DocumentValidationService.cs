using HotelStay.Api.Models;

namespace HotelStay.Api.Services;

public static class DocumentValidationService
{
    public static bool IsValidForDestination(string destination, DocumentType documentType, out string message)
    {
        var isDomestic = DestinationMetadata.Domestic.Contains(destination, StringComparer.OrdinalIgnoreCase);

        if (!isDomestic && documentType == DocumentType.NationalId)
        {
            message = "International travel requires Passport.";
            return false;
        }

        message = string.Empty;
        return true;
    }
}
