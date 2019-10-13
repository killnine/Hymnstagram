using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hymnstagram.Web.Helpers
{
    /// <summary>
    /// Class to help generate response when client input fails validation.
    /// </summary>
    public static class InvalidModelStateResponseFactory
    {
        /// <summary>
        /// Generates an UnprocessableEntityObjectResult with standard field information for troubleshooting
        /// invalid client input. 
        /// </summary>
        /// <param name="modelState">The ModelState (presumably invalid) of the current controller action</param>
        /// <param name="context">HttpContext with information about the current request</param>
        /// <returns>UnprocessableEntityObjectResult (422 Status Code)</returns>
        public static UnprocessableEntityObjectResult GenerateResponseForInvalidModelState(ModelStateDictionary modelState, HttpContext context)
        {
            var problemDetails = new ValidationProblemDetails(modelState)
            {
                Type = "https://hymnstagram.com/api/modelvalidationproblem",
                Title = "One or more model validation errors occurred.",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "See the errors property for details",
                Instance = context.Request.Path
            };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}
