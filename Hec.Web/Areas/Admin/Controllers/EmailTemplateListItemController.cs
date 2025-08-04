using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Hec.Settings;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hec.Web.Controllers;

namespace Hec.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class EmailTemplateListItemController : BaseController
    {
        public EmailTemplateListItemController(HecContext db) : base(db)
        {
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var list = db.EmailTemplates.OrderBy(x => x.Id);
            
            return Json(list.ToDataSourceResult(request, x => new
            {
                Id = x.Id,
                Description = x.Description
            }));
        }
    }
}
