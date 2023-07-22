namespace NimaTask.API.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ModifiedDateTime { get; set; }
}
