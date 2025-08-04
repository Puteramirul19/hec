using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hec.Web
{
    public static class HtmlHelpers
    {
        public static ReadOnlyCustomDataSourceBuilder Ajax(this ReadOnlyDataSourceBuilder me, string actionName, string controllerName)
        {
            return me.Custom()
                .ServerFiltering(true)
                .Type("aspnetmvc-ajax") //Set this type if you want to use DataSourceRequest and ToDataSourceResult instances.
                .Transport(transport =>
                    transport.Read(actionName, controllerName)
                )
                .Schema(schema => schema
                    .Data("Data")   //define the [data](http://docs.telerik.com/kendo-ui/api/javascript/data/datasource#configuration-schema.data) option
                    .Total("Total") //define the [total](http://docs.telerik.com/kendo-ui/api/javascript/data/datasource#configuration-schema.total) option
                );
        }

        public static IHtmlString CustomEncode(this HtmlHelper html, string str)
        {            
            if (String.IsNullOrEmpty(str)) return new HtmlString(str);
            return html.Raw(html.Encode(str).Replace("#", @"\\#"));
        }
    }
}