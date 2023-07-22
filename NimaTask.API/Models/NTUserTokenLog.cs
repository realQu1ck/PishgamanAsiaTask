namespace NimaTask.API.Models;

public class NTUserTokenLog : BaseModel
{
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [NotMapped]
    public NTUser User { get; set; }
    public int TokenId { get; set; }
    [ForeignKey(nameof(TokenId))]
    public NTUserToken Token { get; set; }
}
