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

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Read")]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await unitOfWork.UserRepository.GetDbSet()
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
        var totalRecords = await unitOfWork.UserRepository.GetDbSet().CountAsync();
        return Ok(new PaginationResponse<List<UserViewModel>>(pagedData.Select(x => new UserViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Family = x.Family,
            Meli = x.Meli,
            Parent = x.Parent,
            PhoneNumber = x.PhoneNumber,
            Picture = x.Picture != null ? Convert.ToBase64String(x.Picture) : null
        }).ToList(), validFilter.PageNumber, validFilter.PageSize));
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await unitOfWork.UserRepository.GetDbSet()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (user == null) return NotFound();

        if (!SecurePasswordHasher.Verify(model.Password, user.Password)) return BadRequest("Password is worng");

        List<string> roles = new List<string>();
        foreach (var item in user.UserRoles)
        {
            roles.Add(item.Role.Role);
        }

        var token = GenerateToken(user, roles);

        var userToken = await unitOfWork.UserTokenRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (!userToken.Valid)
            return Unauthorized();

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
                UserId = user.Id,
                Valid = true
            });
        }

        await unitOfWork.SaveChangesAsync();

        var response = new LoginResponse()
        {
            Token = token,
            PhoneNumber = user.PhoneNumber,
            User = user.Name + " " + user.Family
        };
        return Ok(response);
    }

    [HttpPut]
    [Route("EditProfile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> EditProfile([FromBody] EditViewModel model)
    {
        var check = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (check == null) return NotFound("Not Found");

        var user = new NTUser
        {
            Id = check.Id,
            Family = model.Family,
            PhoneNumber = model.PhoneNumber,
        };

        await unitOfWork.UserRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Ok("User Update Successfully.");
    }

    [HttpPost]
    [Route("Register")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        var check = await unitOfWork.UserRepository.AnyAsync(x => x.PhoneNumber == model.PhoneNumber || x.Family == model.Family);
        if (check) return Ok("Family or PhoneNumber is Duplicate");

        var user = new NTUser
        {
            Name = model.Name,
            Family = model.Name,
            Meli = model.Name,
            Parent = model.Name,
            PhoneNumber = model.Name,
            Password = model.Password,
        };
        if (model.Picture != null)
        {
            if (model.Picture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.Picture.CopyTo(ms);
                    user.Picture = ms.ToArray();
                }
            }
        }
        var add = await unitOfWork.UserRepository.AddAsync(user);

        var saveracc = await unitOfWork.UserRepository.GetDbSet()
            .Include(x => x.Tokens)
            .Where(x => x.PhoneNumber == User.Identity.Name).FirstOrDefaultAsync();

        await unitOfWork.UserTokenLogRepository.AddAsync(new NTUserTokenLog
        {
            Token = saveracc.Tokens.Last(),
            UserId = add.Id
        });

        await unitOfWork.SaveChangesAsync();

        return Ok("User Created Successfully.");
    }

    [HttpDelete]
    [Route("Invoke/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> Invoke(int id)
    {
        var check = await unitOfWork.UserTokenRepository.FirstOrDefaultAsync(x => x.UserId == id);
        if (check == null) return NotFound("Not Found");

        check.Valid = false;

        await unitOfWork.UserTokenRepository.UpdateAsync(check);
        await unitOfWork.SaveChangesAsync();

        return Ok("User Invoked Successfully.");
    }


    [HttpDelete]
    [Route("Delete/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> Delete(int id)
    {
        var check = await unitOfWork.UserRepository.GetAsync(id);
        if (check == null) return NotFound("Not Found");

        unitOfWork.UserRepository.DeleteAsync(check);
        await unitOfWork.SaveChangesAsync();

        return Ok("User Deleted Successfully.");
    }


    private string GenerateToken(NTUser user, List<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.PhoneNumber) };
        foreach (var item in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, item));
        }

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
