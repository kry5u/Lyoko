using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Lyoko.Filters
{
    public class IdentityPageAccessFilter : IPageFilter
    {
        private static readonly List<string> AllowedPages = new()
        {
            "/Account/Login",
            "/Account/Register",
            "/Account/Logout",
            "/Account/AccessDenied",
            "/Account/Manage/Index",
            "/Account/Manage/ChangePassword",
            "/Account/Manage/PersonalData",
            "/Account/Manage/DeletePersonalData"
        };

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var pagePath = context.ActionDescriptor.RouteValues["page"];

            if (pagePath != null && !AllowedPages.Contains(pagePath, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToPageResult("/Account/AccessDenied");
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }
    }
}
