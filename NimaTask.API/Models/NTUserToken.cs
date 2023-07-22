namespace NimaTask.API.Models;

public class NTUserToken : BaseModel
{
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public NTUser User { get; set; }
    public string Token { get; set; }
    public bool Valid { get; set; }
    public virtual ICollection<NTUserTokenLog> Logs { get; set; }
}
