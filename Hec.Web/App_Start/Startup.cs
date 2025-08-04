using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.Dashboard;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Linq;
using Hangfire.Annotations;
using Hec.Jobs;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(Hec.Web.Startup))]
namespace Hec.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //app.UseHangfireDashboard("/jobs", new DashboardOptions
            //{
            //    Authorization = new[] { new CustomHangfireAuthorizationFilter() }
            //});

            //app.UseHangfireServer();

            //
            // Register recurring tasks
            //
            //RecurringJob.AddOrUpdate<EmailDispatchJob>(x => x.Run(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<SlaCheckJob>(x => x.Run(), Cron.Daily(int.Parse(ConfigurationManager.AppSettings["SlaCheckHour"])), TimeZoneInfo.Local);
        }
    }

    public class CustomHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private static string[] allowed = new string[] {
            "YWRtaW4=",         // admin is a lucky and priviledged individual
            "cHJpbXVzY29yZQ==", // primuscore too!
            "aW1tZXRyYQ=="      // and this guy too!!
        };

        public bool Authorize([NotNull] DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            var iden = owinContext.Authentication.User.Identity;
            
            if (iden.IsAuthenticated)
            {
                var cypher = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(iden.Name));
                return allowed.Contains(cypher);

            }
            return false;
        }
    }
}