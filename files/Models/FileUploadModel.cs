namespace api.files.Models;

public class FileUploadModel
{
    public string FileName { get; set; }
    public Stream FileStream { get; set; }
}
