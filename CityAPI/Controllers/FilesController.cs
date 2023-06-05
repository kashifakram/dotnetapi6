using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityAPI.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
    }

    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        const string filePath = "Week02- Tute.pdf";

        if (!System.IO.File.Exists(filePath)) return NotFound();

        var bytes = System.IO.File.ReadAllBytes(filePath);

        if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType)) contentType = "application/octet-stream";

        return File(bytes, contentType, Path.GetFileName(filePath));
    }
}