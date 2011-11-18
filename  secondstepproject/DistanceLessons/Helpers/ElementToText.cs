using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;

namespace DistanceLessons.Helpers
{
    public static class ElementToText
    {
        public static MvcHtmlString CheckBoxToText(this HtmlHelper helper, bool value)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(value==true?"Правильно":"Не правильно");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}