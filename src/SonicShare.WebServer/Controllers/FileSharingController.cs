using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonicShare.WebServer.Dtos;
using SonicShare.WebServer.Models;

namespace SonicShare.WebServer.Controllers;


[ApiController]
[Route("shared")]
public class FileSharingController:Controller
{
    [HttpGet("getAll")]
    public IEnumerable<FileItem> GetAll()
    {
        return FileManager.Current.GetAll();
    }

    [HttpPost("download")]
    public IActionResult DownloadFile([FromBody] DownloadRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FilePath))
            return BadRequest("File path is required.");

        var file = FileManager.Current.FirstOrDefault(request.FilePath);
        if (file == null || !System.IO.File.Exists(file.Path))
            return NotFound("File not found.");

        long totalLength = file.Length;
        long start = request.RangeStart ?? 0;
        long end = request.RangeEnd ?? (totalLength - 1);

        if (start < 0 || end >= totalLength || start > end)
            return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable);

        long length = end - start + 1;

        var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1 << 20, useAsync: true);
        fileStream.Seek(start, SeekOrigin.Begin);

        Response.StatusCode = (start == 0 && end == totalLength - 1)
            ? StatusCodes.Status200OK
            : StatusCodes.Status206PartialContent;

        if (Response.StatusCode == StatusCodes.Status206PartialContent)
            Response.Headers["Content-Range"] = $"bytes {start}-{end}/{totalLength}";

        Response.Headers["Accept-Ranges"] = "bytes";
        Response.Headers["Content-Length"] = length.ToString();
        Response.Headers["Content-Disposition"] = $"attachment; filename=\"{file.Name}\"";

        return new FileStreamResult(fileStream, file.ContentType);
    }
}
