namespace NimaTask.API.Models;

public class NTRole : BaseModel
{
    public string Role { get; set; }

    public virtual ICollection<NTUserRole> UserRoles { get; set; }
}
