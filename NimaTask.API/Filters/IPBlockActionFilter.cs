using Microsoft.AspNetCore.Mvc.Filters;
using NimaTask.API.Middlewares.IPBlockService;

namespace NimaTask.API.Filters;

public class IPBlockActionFilter : ActionFilterAttribute
{
    private readonly IIPBlockingService _blockingService;

    public IPBlockActionFilter(IIPBlockingService blockingService)
    {
        _blockingService = blockingService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }

        var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
        var isBlocked = _blockingService.IsBlocked(remoteIp!);
        if (isBlocked)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        base.OnActionExecuting(context);
    }
}
