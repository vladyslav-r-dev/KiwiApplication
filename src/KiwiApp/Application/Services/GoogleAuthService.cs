using Google.Apis.Auth;

namespace KiwiApp.Application.Services;

public class GoogleAuthService // вынести строку в конфиг
{
    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { "269421523996-s4emj656j64b2k6vjhb417jqb2ilkivd.apps.googleusercontent.com" }
            };
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        
            return payload;
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}