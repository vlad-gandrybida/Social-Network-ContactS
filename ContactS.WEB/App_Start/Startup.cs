using ContactS.BLL.Interfaces;
using ContactS.BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(ContactS.WEB.App_Start.Startup))]

namespace ContactS.WEB.App_Start
{
    public class Startup
    {
        private IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(CreateUserService);
            app.CreatePerOwinContext(CreateMessageService);
            app.CreatePerOwinContext(CreateDialogService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        private IUserService CreateUserService()
        {
            return serviceCreator.CreateUserService("DefaultConnection");
        }

        private IMessageService CreateMessageService()
        {
            return serviceCreator.CreateMessageService("DefaultConnection");
        }

        private IDialogService CreateDialogService()
        {
            return serviceCreator.CreateDialogService("DefaultConnection");
        }
    }
}