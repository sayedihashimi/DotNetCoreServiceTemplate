using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lykke.ApiAuth.Mvc
{
    public class ApiAuthConfig
    {
        public string ApiId { get; set; }
    }

    public class ApiAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiAuthConfig _apiAuthConfig;
        private readonly ILykkeApiKeysRepository _apiKeysRepository;

        public ApiAuthMiddleware(RequestDelegate next, ApiAuthConfig apiAuthConfig,
            ILykkeApiKeysRepository apiKeysRepository)
        {
            _next = next;
            _apiAuthConfig = apiAuthConfig;
            _apiKeysRepository = apiKeysRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers
                .ContainsKey(HttpRequestHeader.Authorization.ToString()))
            {
                var authHeader = context.Request.Headers[HttpRequestHeader.Authorization.ToString()];
                context.User = await GetClaimsAsync(authHeader);
            }

            await _next.Invoke(context);
        }

        private async Task<ClaimsPrincipal> GetClaimsAsync(string authHeader)
        {
            var auth = authHeader.Split(' ');
            if (auth.Length != 2 || auth[0] != "ApiKey")
                return null;

            var apiKey = await _apiKeysRepository.GetAsync(auth[1]);
            if (apiKey?.LykkeApiId != _apiAuthConfig.ApiId)
                return null;

            return LykkeApiAuthIdentity
                .Create(apiKey)
                .ToClaimsPrincipal();
        }
    }

    public class LykkeApiAuthIdentity : IIdentity
    {
        public string AuthenticationType => "ApiKey";

        public bool IsAuthenticated =>
            !string.IsNullOrEmpty(LykkeApiId) &&
            !string.IsNullOrEmpty(LykkeAppId);

        public string Name => LykkeAppId;

        public string LykkeAppId { get; private set; }
        public string LykkeApiId { get; private set; }

        public static LykkeApiAuthIdentity Create(ILykkeApiKey apiKey)
        {
            return new LykkeApiAuthIdentity
            {
                LykkeApiId = apiKey.LykkeApiId,
                LykkeAppId = apiKey.LykkeAppId,
            };
        }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var principal = new ClaimsPrincipal(this);

            (principal.Identity as ClaimsIdentity)?
                .AddClaim(new Claim(nameof(LykkeApiId), LykkeApiId));

            (principal.Identity as ClaimsIdentity)?
                .AddClaim(new Claim(nameof(LykkeAppId), LykkeAppId));

            return principal;
        }
    }
}
