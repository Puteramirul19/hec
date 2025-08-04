using Hec.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Controllers
{
    public class FilePublicController : Controller
    {
        IFileStore fileStore;

        public FilePublicController(IFileStore fileStore)
        {
            this.fileStore = fileStore;
        }

        public FileStreamResult Index(string fileId)
        {
            if (String.IsNullOrEmpty(fileId))
                throw new HttpException(403, "Forbidden: Directory listing denied.");

            try
            {
                var stream = fileStore.GetStream(fileId);
                var fileName = System.IO.Path.GetFileName(fileId);

                return File(stream, System.Web.MimeMapping.GetMimeMapping(fileName), fileName);
            }
            catch (Exception ex)
            {
                throw new HttpException(404, "File not found.", ex);
            }
        }
    }
}