using System.Globalization;
using System.Web.Compilation;
using System.Web.Mvc;

namespace DistanceLessons.Helpers
{
    public static class Localization
    {
        public static string Resource(this HtmlHelper helper, string expr, params object[] args)
        {
            string path = ((RazorView) helper.ViewContext.View).ViewPath;

            ResourceExpressionFields fields =
                (ResourceExpressionFields)
                (new ResourceExpressionBuilder()).ParseExpression(expr, typeof (string),
                                                                  new ExpressionBuilderContext(path));
            return (!string.IsNullOrEmpty(fields.ClassKey))
                       ? string.Format(
                           (string)
                           helper.ViewContext.HttpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey,
                                                                                  CultureInfo.CurrentUICulture), args)
                       : string.Format(
                           (string)
                           helper.ViewContext.HttpContext.GetLocalResourceObject(path, fields.ResourceKey,
                                                                                 CultureInfo.CurrentUICulture), args);
        }
    }
}