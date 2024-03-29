﻿using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace DistanceLessons.Attributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string culture= "uk-ua";

            if (filterContext.RequestContext.HttpContext.Request.Cookies["language"] != null)
            {
                culture = (filterContext.RequestContext.HttpContext.Request.Cookies["language"].Value != null)
                              ? string.Format(
                                  filterContext.RequestContext.HttpContext.Request.Cookies["language"].Value)
                              : "uk-ua";
            }

            CultureInfo cultureInfo = new CultureInfo(culture);

            if (!Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(culture.ToLower()))
            {
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
        }
    }
}