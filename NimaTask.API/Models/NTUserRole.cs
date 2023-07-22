namespace NimaTask.API.Models;

public class NTUserRole : BaseModel
{
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public NTUser User { get; set; }
    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public NTRole Role { get; set; }
}
