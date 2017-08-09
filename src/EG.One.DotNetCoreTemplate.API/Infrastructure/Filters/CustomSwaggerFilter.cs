using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace EG.One.DotNetCoreTemplate.API.Infrastructure.Filters
{
    public class CustomSwaggerHeadersFilter : IOperationFilter
    {
        /// <summary>
        /// Add session and company header to swagger test page
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            var auth = new NonBodyParameter()
            {
                Name = "Authorization",
                @In = "header",
                Description = "Authorization",
                Required = true,
                Type = "string",
                Default = ""
            };

            operation.Parameters.Add(auth);
        }
    }
}
