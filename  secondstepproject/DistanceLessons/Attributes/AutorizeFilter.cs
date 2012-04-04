using System.Web.Mvc;


namespace DistanceLessons.Attributes
{
    public class AutorizeFilterAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            context.Result = new RedirectResult("/Account/FewPermissions");
        }
    }
}