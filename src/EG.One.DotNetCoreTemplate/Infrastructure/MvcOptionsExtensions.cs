using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace EG.One.DotNetCoreTemplate.Infrastructure
{
    /// <summary>
    /// Extends MvcOptions to make global route prefix possible
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// This method makes it possible to add a prefix route to the application
        /// </summary>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }    
}
