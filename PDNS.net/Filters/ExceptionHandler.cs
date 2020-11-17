using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net.Filters
{
    public class ExceptionHandler : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new ObjectResult(filterContext)
            {
                Value = filterContext.Exception.Message + Environment.NewLine + "On: " + filterContext.Exception.Source,
                StatusCode = 500
            };
        }
    }
}
