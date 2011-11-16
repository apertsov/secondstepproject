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
        public static MvcHtmlString TextHtml(this HtmlHelper helper, string text)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(text);

            return MvcHtmlString.Create(sb.ToString());
        }

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
                sb.Append(helper.ActionLink(" << ", ViewName, new { numPage = 0 }, ao));
                sb.Append(helper.ActionLink(" < ", ViewName, new { numPage = numPage - 1 }, ao));
            }

            int PageCount = (int)Math.Ceiling((double)itemsCount / itemOnPage);

            if (numPage - 1 >= 0)
            {
                sb.Append(helper.ActionLink((numPage).ToString(), ViewName, new { numPage = numPage - 1 }, ao));
            }

            if (itemsCount>itemOnPage)
            {
                sb.Append("<span class=\"current_page\">" + (numPage + 1).ToString() + "</span>");   
            }

            if (numPage + 1 <= PageCount - 1)
            {
                sb.Append(helper.ActionLink((numPage + 2).ToString(), ViewName, new { numPage = numPage + 1 }, ao));
            }


            if (numPage < PageCount - 1)
            {
                sb.Append(helper.ActionLink(" > ", ViewName, new { numPage = numPage + 1 }, ao));
                sb.Append(helper.ActionLink(" >> ", ViewName, new { numPage = PageCount - 1 }, ao));
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}