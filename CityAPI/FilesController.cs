using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityAPI
{
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
			var filePath = "Week02- Tute.pdf";

			if (!System.IO.File.Exists(filePath)) return NotFound();

			var bytes = System.IO.File.ReadAllBytes(filePath);

			if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contenttype)) contenttype = "application/octet-stream";

			return File(bytes, contenttype, Path.GetFileName(filePath));
        }
	}
}

