using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace SainsTekWepApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("ID-id");
        }

        //void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (Request.AppRelativeCurrentExecutionFilePath == "~/")
        //        HttpContext.Current.RewritePath("~/Gate/Index.aspx");//This is the default page to navigate after a successful login.
        //}

        //protected  override void Application_Error(object sender, EventArgs e)
        //{
        //    var ex = Server.GetLastError();
        //    var httpException = ex as HttpException ?? ex.InnerException as HttpException;
        //    if (httpException == null) return;

        //    if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
        //    {
        //        //handle the error
        //        Response.Write("Sorry, file is too big"); //show this message for instance
        //    }
        //}

        //public override void Init()
        //{
        //    base.Init();
        //    base.EndRequest += MvcApplication_EndRequest;
        //    base.BeginRequest += MvcApplication_BeginRequest;
        //}

        //void MvcApplication_BeginRequest(object sender, EventArgs e)
        //{
        //    //Debug.WriteLine(" sender {0}, event", sender, e);
        //}

        //void MvcApplication_EndRequest(object sender, EventArgs e)
        //{
        //    var ex = Server.GetLastError();

        //    if (ex == null)
        //    {
        //        return;
        //    }

        //    var httpException = ex as HttpException ?? ex.InnerException as HttpException;

        //    if (httpException == null)
        //    {
        //        return;
        //    }

        //    if (httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
        //    {
        //        //handle the error
        //        Response.Write("Ukuran file terlalu besar."); //show this message for instance                
        //    }
        //}
    }
}
