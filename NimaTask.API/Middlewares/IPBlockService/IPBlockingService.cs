namespace NimaTask.API.Middlewares.IPBlockService;

public class IPBlockingService : IIPBlockingService
{
    private readonly List<string> _blockedIps;
    public IPBlockingService(IConfiguration configuration)
    {
        var blockedIps = configuration.GetValue<string>("BlockedIPs");
        _blockedIps = blockedIps.Split(',').ToList();
    }
    public bool IsBlocked(IPAddress ipAddress) => _blockedIps.Contains(ipAddress.ToString());
}
