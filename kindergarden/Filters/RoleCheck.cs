using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace kindergarden.Filters
{
    public class RoleCheck : ActionFilterAttribute
    {
        public string RoleName { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string roleName = filterContext.HttpContext.Session["role"].ToString();
            if (roleName != RoleName)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "Login" } });
            }
        }
    }
}