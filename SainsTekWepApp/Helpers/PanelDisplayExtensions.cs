using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp
{
    public static class PanelDisplayExtensions
    {
        public static IHtmlString PanelMessage(this HtmlHelper helper, string alertType, string iconType, int count, string message, string url)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"<div class=""col-lg-3 col-md-6"">");
            sb.Append(string.Format( @"<div class=""panel panel-{0}"">", alertType ));
            sb.Append(@"<div class=""panel-heading"">");

            sb.Append(@"<div class=""row"">");
            sb.Append(string.Format(@"<div class=""col-xs-3""><i class=""fa fa-{0} fa-5x""></i></div>", iconType));
            sb.Append(@"<div class=""col-xs-9 text-right"">");
            sb.Append(string.Format(@"<div class=""huge"">{0}</div>", count));
            sb.Append(string.Format(@"<div>{0}</div>", message));
            sb.Append(@"</div></div></div>");
            sb.Append(string.Format(@"<a href=""{0}"">", url));
            sb.Append(@"<div class=""panel-footer"">");
            sb.Append(@"<span class=""pull-left"">View Details</span>");
            sb.Append(@"<span class=""pull-right""><i class=""fa fa-arrow-circle-right""></i></span>");
            sb.Append(@"<div class=""clearfix""></div>");
            sb.Append(@"</div></a></div></div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}