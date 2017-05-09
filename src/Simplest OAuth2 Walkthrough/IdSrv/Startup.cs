using Owin;
using IdentityServer3.Core.Configuration;

namespace IdSrv
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                SiteName = "Test Identity Server",
                Factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                            .UseInMemoryUsers(Users.Get()),

                SigningCertificate = Certificate.Get(),

                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}