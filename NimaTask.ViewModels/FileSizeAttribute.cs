using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NimaTask.ViewModels;

public class FileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    private readonly int _minFileSize;
    public FileSizeAttribute(int maxFileSize, int minFileSize)
    {
        _maxFileSize = maxFileSize;
        _minFileSize = minFileSize;
    }

    protected override ValidationResult IsValid(
    object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            if (file.Length > _maxFileSize && file.Length < _minFileSize)
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
        return $"Maximum allowed file size is {_maxFileSize} bytes.";
    }
}
