namespace NimaTask.API.Middlewares.IPBlockService;

public interface IIPBlockingService
{
    bool IsBlocked(IPAddress ipAddress);
}
