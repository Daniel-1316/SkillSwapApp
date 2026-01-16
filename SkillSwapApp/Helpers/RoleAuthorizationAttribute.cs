using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SkillSwapApp.Helpers
{
    public class RoleAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public RoleAuthorizationAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userRole = session.GetString("Role");

            // Guest level (Ниво 0) - само Read операции
            if (_requiredRole == "Guest")
            {
                base.OnActionExecuting(context);
                return;
            }

            // User level (Ниво 1) - търсене, сортиране, странициране
            if (_requiredRole == "User")
            {
                if (userRole == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Auth", null);
                    return;
                }
            }

            // Admin level (Ниво 2) - пълен достъп
            if (_requiredRole == "Admin")
            {
                if (userRole != "Admin")
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}