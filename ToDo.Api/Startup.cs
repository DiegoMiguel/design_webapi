
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using ToDo.Api.Formatter;
using ToDo.Api.Helper;

namespace ToDo.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            
            // Enable Cors
            app.UseCors(CorsOptions.AllowAll);

            // Web API configuration and services
            httpConfiguration.Formatters.Clear();
            httpConfiguration.Formatters.Insert(0, new JilFormatter());

            ConfigureOAuth(app);
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
            
            
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/bearerToken"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}