using NuGet.Common;

namespace NimaTask.MVC.Controllers;

public class UserController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string route = "https://localhost:7033/api/User";

    public UserController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Token")?.Value;
        _httpClient.DefaultRequestHeaders.Authorization =
           new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var res = await _httpClient.GetFromJsonAsync<PaginationResponse<List<UserViewModel>>>(new Uri(route));
        return View(res);
    }
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    public async Task<IActionResult> LoginConfirm(LoginViewModel model)
    {
        var post = await _httpClient.PostAsJsonAsync(new Uri(route + "/Login"), model);
        if (!post.IsSuccessStatusCode)
            return RedirectToAction("Login", "User");

        var res = await post.Content.ReadFromJsonAsync<LoginResponse>();

        var claims = new List<Claim>
        {
            new Claim("Token", res.Token),
            new Claim("PhoneNumber", res.PhoneNumber),
            new Claim("User", res.User)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            null);

        return RedirectToAction("Index", "Home");
    }
}
