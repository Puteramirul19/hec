using System.Web.Http;
using System.Web;
using Telerik.Reporting.Services.WebApi;

namespace Hec.Web.Controllers
{
    [Authorize]
    public class ReportsController : ReportsControllerBase
    {
        static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
            new Telerik.Reporting.Services.ReportServiceConfiguration
            {
                HostAppId = "Hec",
                ReportResolver = new ReportFileResolver(HttpContext.Current.Server.MapPath("~/Reports"))
                    .AddFallbackResolver(new ReportTypeResolver()),
                Storage = new Telerik.Reporting.Cache.File.FileStorage(),
            };

        public ReportsController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }
}
