using Entities.LogModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Fluent;
using Services.Contracts;
using RouteData = Microsoft.AspNetCore.Routing.RouteData;

namespace Presentation.ActionFilters;

public class LogFilterAttribute : ActionFilterAttribute
{
    private readonly ILoggerService _loggerService;

    public LogFilterAttribute(ILoggerService service)
    {
        _loggerService = service;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _loggerService.LogInfo(Log("OnActionExecuting", context.RouteData));
    }

    private string Log(string modelName, RouteData routeData)
    {
        var logDetails = new LogDetails()
        {
            ModelName = modelName,
            Controller = routeData.Values["controller"],
            Action = routeData.Values["action"],
        };
        if (routeData.Values.Count >= 3)
        {
            logDetails.Id = routeData.Values["Id"];
        }

        return logDetails.ToString();
    }
}