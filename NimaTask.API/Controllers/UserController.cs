namespace NimaTask.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly Microsoft.Extensions.Logging.ILogger logger;

    public UserController(IUnitOfWork unitOfWork, Microsoft.Extensions.Logging.ILogger logger)
    {
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }
}
