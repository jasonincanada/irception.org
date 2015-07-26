using System;
using irception.Domain;

namespace irception.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static ircCache Cache { get; set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            // TODO: Logging

            Cache = new ircCache();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}