﻿namespace NimaTask.API.Controllers;

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
            Name = x.Name,
            Family = x.Family,
            Meli = x.Family,
            Parent = x.Parent,
            PhoneNumber = x.PhoneNumber,
            Picture = Convert.ToBase64String(x.Picture)
        }).ToList(), validFilter.PageNumber, validFilter.PageSize));
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

    [HttpPut]
    [Route("EditProfile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> EditProfile([FromForm] RegisterViewModel model)
    {
        var check = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (check == null) return NotFound("Not Found");

        var user = new NTUser
        {
            Id = model.Id,
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

        await unitOfWork.UserRepository.UpdateAsync(user);

        await unitOfWork.SaveChangesAsync();

        return Ok("User Created Successfully.");
    }

    [HttpPost]
    [Route("Register")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Write")]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
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
        await unitOfWork.UserRepository.AddAsync(user);

        await unitOfWork.SaveChangesAsync();

        return Ok("User Created Successfully.");
    }

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
