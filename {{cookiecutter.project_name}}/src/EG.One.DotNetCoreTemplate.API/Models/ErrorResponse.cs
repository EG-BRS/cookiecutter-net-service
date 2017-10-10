using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EG.One.DotNetCoreTemplate.Models
{
    /// <summary>
    /// Error Response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error meassge
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error stack trace
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Current model state
        /// </summary>
        public ModelStateDictionary ModelState { get; set; }

        /// <summary>
        /// The innerException
        /// </summary>
        public string InnerException { get; set; }
    }
}
