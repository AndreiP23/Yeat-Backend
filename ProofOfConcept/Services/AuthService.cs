using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ProofOfConcept.Services
{
    public class FirebaseAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {

    }
    public class AuthService : AuthenticationHandler<FirebaseAuthenticationSchemeOptions>
    {
        private readonly FirebaseAuth _firebaseAuth;

        public AuthService(IOptionsMonitor<FirebaseAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            FirebaseApp firebaseApp) : base(options, logger, encoder, clock)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header missing");

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(token);
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
                new Claim(ClaimTypes.Name, decodedToken.Claims["name"].ToString()),
                new Claim("FirebaseToken", token)
            };

                var identity = new ClaimsIdentity(claims, nameof(AuthService));
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Firebase Authentication failed: {ex.Message}");
            }
        }
    }
}
