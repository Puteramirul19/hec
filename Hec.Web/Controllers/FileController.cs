using Hec.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        IFileStore fileStore;

        public FileController(IFileStore fileStore)
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

        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            var fileName = System.IO.Path.GetFileName(file.FileName);
            var fileId = DateTime.Now.Ticks.ToString() + "_" + fileName;

            object result = null;
            try
            {
                fileStore.Save(fileId, file.InputStream);
                result = new
                {
                    fileName = fileName,
                    fileId = fileId,
                    fileUrl = Url.Action("Index", "File", new { fileId = fileId }),
                    fileExtension = System.IO.Path.GetExtension(fileName),
                    fileSize = file.ContentLength
                };
            }
            catch (System.IO.IOException)
            {
                // If file already exist, just ignore this exception.
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                result = new
                {
                    error = ex.Message
                };
            }

            return Json(result, "text/html");
        }


        [HttpPost]
        public JsonResult Remove(string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                fileStore.Delete(fileName);
            }

            return Json(new { }, "text/html");
        }

        [HttpPost]
        public JsonResult RemoveOnly(string fileId)
        {
            if (!String.IsNullOrEmpty(fileId))
            {
                fileStore.Delete(fileId);
            }

            return Json(new { }, "text/html");
        }
    }
}