﻿using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Text;

namespace DistanceLessons.Helpers
{
    public static class Paging
    {
        public static MvcHtmlString PagesType(this AjaxHelper helper, int numPage, int itemsCount, int itemOnPage, string target, string ViewName, int type)
        {
            int PageCount = (int)Math.Ceiling((double)itemsCount / itemOnPage);
            StringBuilder sb = new StringBuilder();
            AjaxOptions ao = new AjaxOptions();
            ao.UpdateTargetId = target;
            
            if (numPage > 1)
            {
                sb.Append(helper.ActionLink(" << ", ViewName, new { withStatus=type,numPage = 0 }, ao));
            }

            if (numPage > 0)
            {
                sb.Append(helper.ActionLink(" < ", ViewName, new { withStatus = type, numPage = numPage - 1 }, ao));
            }

            if (numPage - 1 >= 0)
            {
                sb.Append(helper.ActionLink((numPage).ToString(), ViewName, new { withStatus = type,numPage = numPage - 1 }, ao));
            }

            if (itemsCount > itemOnPage)
            {
                sb.Append("<span class=\"current_page\">" + (numPage + 1).ToString() + "</span>");
            }

            if (numPage + 1 <= PageCount - 1)
            {
                sb.Append(helper.ActionLink((numPage + 2).ToString(), ViewName, new { withStatus = type,numPage = numPage + 1 }, ao));
            }

            if (numPage < PageCount - 1)
            {
                sb.Append(helper.ActionLink(" > ", ViewName, new { withStatus = type, numPage = numPage + 1 }, ao));
            }

            if (numPage < PageCount - 2)
            {
                sb.Append(helper.ActionLink(" >> ", ViewName, new { withStatus = type,numPage = PageCount - 1 }, ao));
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString Pages(this AjaxHelper helper, int numPage, int itemsCount, int itemOnPage, string target, string ViewName)
        {
            int PageCount = (int)Math.Ceiling((double)itemsCount / itemOnPage);
            StringBuilder sb = new StringBuilder();
            AjaxOptions ao = new AjaxOptions();
            ao.UpdateTargetId = target;

            if (numPage > 1)
            {
                sb.Append(helper.ActionLink(" << ", ViewName, new { numPage = 0 }, ao));
            }

            if (numPage > 0)
            {
                sb.Append(helper.ActionLink(" < ", ViewName, new { numPage = numPage - 1 }, ao));
            }

            if (numPage - 1 >= 0)
            {
                sb.Append(helper.ActionLink((numPage).ToString(), ViewName, new { numPage = numPage - 1 }, ao));
            }

            if (itemsCount > itemOnPage)
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
            }

            if (numPage < PageCount - 2)
            {
                sb.Append(helper.ActionLink(" >> ", ViewName, new { numPage = PageCount - 1 }, ao));
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}