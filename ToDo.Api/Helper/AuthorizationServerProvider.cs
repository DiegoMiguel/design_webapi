
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToDo.Api.Helper
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var user = new Models.Entities.User().Authenticate(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Usuário ou senha esta incorreto");
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("usr", context.UserName));
            identity.AddClaim(new Claim("role", "admin"));

            context.Validated(identity);


        }
    }
}