using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Text;

namespace DistanceLessons.Helpers
{
    public static class Paging
    {
        public static MvcHtmlString Pages(this HtmlHelper helper, int numPage, int itemsCount, int itemOnPage, string ViewName)
        {
            StringBuilder sb = new StringBuilder();

            if (numPage > 0)
            {
                sb.Append(helper.ActionLink(" < ", ViewName, new { numPage = numPage - 1 }));
            }

            int PageCount = (int)Math.Ceiling((double)itemsCount / itemOnPage);

            if (numPage < PageCount - 1)
            {
                sb.Append(helper.ActionLink(" > ", ViewName, new { numPage = numPage + 1 }));
            }


            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString Pages(this AjaxHelper helper, int numPage, int itemsCount, int itemOnPage, string target, string ViewName)
        {
            StringBuilder sb = new StringBuilder();
            AjaxOptions ao = new AjaxOptions();
            ao.UpdateTargetId = target;

            if (numPage > 0)
            {
                sb.Append(helper.ActionLink(" < ", ViewName, new { numPage = numPage - 1 }, ao));
            }

            int PageCount = (int)Math.Ceiling((double)itemsCount / itemOnPage);

            if (numPage < PageCount - 1)
            {
                sb.Append(helper.ActionLink(" > ", ViewName, new { numPage = numPage + 1 }, ao));
            }


            return MvcHtmlString.Create(sb.ToString());
        }
    }
}