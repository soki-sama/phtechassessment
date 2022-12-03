using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Propeller.API.Providers
{
    public class TokenBasedRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // Retrieve the Bearer Token
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (token != null)
            {
                token = token.Replace("Bearer ", "").ToString();

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

                if (jwt != null && jwt.Claims != null)
                {
                    var localeClaim = jwt.Claims.First(c => c.Type == Constants.LocaleClaim).Value;

                    if (!string.IsNullOrEmpty(localeClaim))
                    {
                        return Task.FromResult(new ProviderCultureResult(localeClaim));
                    }
                }
            }

            //var identity = httpContext.User.Identity as ClaimsIdentity;
            //if (identity != null)
            //{
            //    IEnumerable<Claim> claims = identity.Claims;
            //    // or
            //    // identity.FindFirst("ClaimName").Value;

            //}

            //var localeClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.LocaleClaim);

            //if (localeClaim != null)
            //{
            //    if (!string.IsNullOrEmpty(localeClaim.Value))
            //    {
            //        return Task.FromResult(new ProviderCultureResult(localeClaim.Value));
            //    }
            //}

            //             new ProviderCultureResult("es-MX")
            //          var c=httpContext.User.cul
            // TODO: Investigate about this warning

            // Default
            return Task.FromResult(new ProviderCultureResult("en-NZ"));

            // User
            //      throw new NotImplementedException();
        }
    }
}
