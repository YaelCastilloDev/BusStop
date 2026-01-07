using Google.Apis.Auth;

namespace Application.Services.Interfaces.Authentication
{
    public interface IGoogleAuthService
    {
        // Returns the payload (Email, Name, Subject/Id) if valid
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
    }
}