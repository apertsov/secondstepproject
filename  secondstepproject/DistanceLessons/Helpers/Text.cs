using System.Text;
using System.Web.Mvc;

namespace DistanceLessons.Helpers
{
    public static class Text
    {
        public static MvcHtmlString TextHtml(this HtmlHelper helper, string text)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(text);

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}