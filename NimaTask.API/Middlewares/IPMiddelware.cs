using NimaTask.API.Middlewares.IPBlockService;

namespace NimaTask.API.Middlewares;

public class IPMiddelware
{
    private readonly RequestDelegate _next;
    private readonly IIPBlockingService _blockingService;
    public IPMiddelware(RequestDelegate next, IIPBlockingService blockingService)
    {
        _next = next;
        _blockingService = blockingService;
    }
    public async Task Invoke(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress;
        var isBlocked = _blockingService.IsBlocked(remoteIp!);
        if (isBlocked)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        await _next.Invoke(context);
    }
}
