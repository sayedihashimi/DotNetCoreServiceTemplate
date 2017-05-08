using Lykke.ApiAuth.Mvc;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Mvc
{
    public static class LykkeApiAuthControllerExtensions
    {
        public static string GetLykkeAppId(this ControllerBase controller)
        {
            var claim = controller.HttpContext.User
                .FindFirst(nameof(LykkeApiAuthIdentity.LykkeAppId));
            return claim?.Value;
        }
    }
}