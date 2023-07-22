namespace NimaTask.API.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please Enter Your Phone Number !")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please Enter Your Password !")]
    [MinLength(8)]
    public string Password { get; set; }
}
