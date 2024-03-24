namespace api.files.Models;

public class FileUploadModel
{
    public required string FileName { get; set; }
    public required Stream FileStream { get; set; }
}
