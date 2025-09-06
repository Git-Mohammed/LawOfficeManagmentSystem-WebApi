using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace LOMs.Api.Infrastructure;

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var title = "Application error";
        var detail = exception.Message;

        if (exception is NotSupportedException notSupportedEx &&
            notSupportedEx.Message.Contains("type discriminator"))
        {
            statusCode = StatusCodes.Status400BadRequest;
            title = "Invalid client payload";
            detail = "يرجى تحديد نوع العميل في البيانات المرسلة باستخدام الخاصية 'kind'.";
        }

        // Optional: log the exception
        logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = title,
            Detail = detail,
            Status = statusCode
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }
}
