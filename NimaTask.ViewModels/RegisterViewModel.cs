using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NimaTask.ViewModels;

public class RegisterViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please Enter Your Name !")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please Family Your Name !")]
    public string Family { get; set; }

    [Required(ErrorMessage = "Please Enter Your Father Name !")]
    public string Parent { get; set; }

    [Required(ErrorMessage = "Please Enter Your Meli Code !")]
    public string Meli { get; set; }

    [Required(ErrorMessage = "Please Enter Your Name !")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please Enter Your Password !")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please Enter Your Password !")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords Not Match")]
    public string ConfirmPassword { get; set; }

    [FileSize(2048, 3072)]
    public IFormFile Picture { get; set; }
}
