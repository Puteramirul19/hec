using Hec.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using System.IO;
using Hec.Integrations;

namespace Hec.Web.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        private IBcrmService bcrmService;

        public SetupController(IBcrmService bcrmService)
        {
            this.bcrmService = bcrmService;
        }

        // GET: Setup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAccountInfoFromBcrm(string accountNo)
        {
            var acc = bcrmService.GetAccountInfo(accountNo);
            if (acc == null)
                throw new Exception("Account not found.");

            return Content("<pre>" + acc.Dump() + "</pre>");
        }

        //public ActionResult MigrateFile()
        //{
        //    var diskStore = MvcApplication.Container.Resolve<IFileStore>() as DiskFileStore;
        //    if (diskStore == null)
        //        throw new Exception("IFileStore not set up or is not a DiskFileStore");
            
        //    var dbStore = new DbFileStore("HecContext");
        //    dbStore.Initialize();

        //    var di = new DirectoryInfo(diskStore.StorageDir);
        //    var count = 0;
        //    foreach (var fi in di.GetFiles())
        //    {
        //        var filename = fi.Name;
        //        Response.Write("<br>Migrating: " + filename);
        //        Response.Flush();
        //        using (var stream = diskStore.GetStream(filename))
        //        {
        //            dbStore.Save(filename, stream);
        //            stream.Close();

        //            Response.Write("  >> DONE");
        //            Response.Flush();
        //        }
        //        count++;
        //    }

        //    return Content("<br>ALL DONE! Migrated " + count + " files.");
        //}
    }
}