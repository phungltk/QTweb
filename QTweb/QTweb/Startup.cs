using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QTweb.Startup))]
namespace QTweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
