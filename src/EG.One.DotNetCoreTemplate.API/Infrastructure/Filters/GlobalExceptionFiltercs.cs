using EG.One.DotNetCoreTemplate.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace EG.One.DotNetCoreTemplate.API.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _enviroment;

        public GlobalExceptionFilter(IHostingEnvironment env)
        {
            _enviroment = env;
        }

        public void OnException(ExceptionContext context)
        {
            var response = new ErrorResponse()
            {
                Message = context.Exception.Message,
                StackTrace = _enviroment.IsDevelopment() ? context.Exception.StackTrace : "", //only show stacktrace in dev
                InnerException = (context.Exception.InnerException != null) ? context.Exception.InnerException.Message : String.Empty
            };

            var exceptionType = context.Exception.GetType();

            if (!context.HttpContext.Request.Path.ToString().Contains("swagger"))
            {
                if (exceptionType == typeof(NotImplementedException))
                {
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.NotImplemented,
                        DeclaredType = typeof(ErrorResponse)
                    };
                }
                else if (exceptionType == typeof(KeyNotFoundException))
                {
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        DeclaredType = typeof(ErrorResponse)
                    };
                }
                else if (exceptionType == typeof(HttpRequestException)) //From the request to Xena if "ensureSuccesStatusCode" is hit and not handled before
                {
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        DeclaredType = typeof(ErrorResponse)
                    };
                }
                else if (exceptionType == typeof(UnauthorizedAccessException)) //if access to xena is unauthorized e.g if xena_access_token is expired
                {
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        DeclaredType = typeof(ErrorResponse)
                    };
                }
                else
                {
                    //default statusCode
                    context.Result = new ObjectResult(response)
                    {
                        StatusCode = 500,
                        DeclaredType = typeof(ErrorResponse)
                    };
                }

            }
        }
    }
}
