using System.ComponentModel.DataAnnotations;

namespace NimaTask.API.Data.NLogDatabase;

public class Logs
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    [MaxLength(10)]
    public string Level { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Exception { get; set; }
    [MaxLength(255)]
    public string Logger { get; set; }
    [MaxLength(255)]
    public string Url { get; set; }
}
