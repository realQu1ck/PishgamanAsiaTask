namespace NimaTask.API.Models;

public class NTUser : BaseModel
{
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

    public byte[] Picture { get; set; }

    public virtual ICollection<NTUserRole> UserRoles { get; set; }
}
