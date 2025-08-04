using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Hec.Web
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings();
            Settings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
            Settings.Formatting = Formatting.Indented;
            //Settings.Converters.Add(new StringEnumConverter());
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(response.Output, this.Data);
        }
    }

    public class JsonNetActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(JsonResult))
            {
                // Get the standard result object with unserialized data
                JsonResult result = filterContext.Result as JsonResult;

                // Replace it with our new result object and transfer settings
                filterContext.Result = new JsonNetResult
                {
                    ContentEncoding = result.ContentEncoding,
                    ContentType = result.ContentType,
                    Data = result.Data,
                    JsonRequestBehavior = result.JsonRequestBehavior
                };

                // Later on when ExecuteResult will be called it will be the
                // function in JsonNetResult instead of in JsonResult
            }
            base.OnActionExecuted(filterContext);
        }
    }

    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                return null;

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var bodyText = reader.ReadToEnd();

            return String.IsNullOrEmpty(bodyText) ? null : new DictionaryValueProvider<object>(JsonConvert.DeserializeObject<ExpandoObject>(bodyText, new ExpandoObjectConverter()), CultureInfo.CurrentCulture);            
        }
    }
}