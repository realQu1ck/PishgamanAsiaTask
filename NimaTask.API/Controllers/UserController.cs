namespace NimaTask.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<UserController> logger;
    private readonly IConfiguration _config;
    public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger, IConfiguration config)
    {
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        _config = config;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (user == null) return NotFound();

        if (!SecurePasswordHasher.Verify(model.Password, user.Password)) return Ok("Password is worng");

        var token = GenerateToken(user);

        var userToken = await unitOfWork.UserTokenRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (userToken != null)
        {
            userToken.Token = token;
            await unitOfWork.UserTokenRepository.UpdateAsync(userToken);
        }
        else
        {
            await unitOfWork.UserTokenRepository.AddAsync(new NTUserToken
            {
                Token = token,
                UserId = user.Id
            });
        }

        await unitOfWork.SaveChangesAsync();

        return Ok(new { Token = token, PhoneNumber = user.PhoneNumber, User = user.Name + " " + user.Family });
    }

    //[HttpPost]
    //[Route("Register")]
    //public async Task<IActionResult> Register()
    //{

    //}

    private string GenerateToken(NTUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.PhoneNumber) };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
