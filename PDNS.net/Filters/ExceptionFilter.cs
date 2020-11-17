using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDNS.net
{
    public class ExceptionFilter : IExceptionFilter
    {
        public ExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            /*
            var result = new ViewResult { ViewName = "CustomError" };
            result.ViewData.Add("Exception", context.Exception);*/
            // TODO: Pass additional detailed data via ViewData
            context.Result = new ObjectResult(context)
            {
                Value = context.Exception.Message,
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
