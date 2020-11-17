using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string sidemenu = string.Empty;
            if (!(context.Controller is Controller controller)) return;
            var action = controller.ControllerContext.ActionDescriptor;
            if (action.ControllerName != "Home") sidemenu = @$"/{action.ControllerName}";
            if (action.ActionName != "Index") sidemenu += @$"/{action.ActionName}";
            controller.ViewData["side-menu"] = sidemenu;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
