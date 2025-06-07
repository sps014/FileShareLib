using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonicShare.WebServer.CustomActions;
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
        // ... validate request, get file info ...

        var file = FileManager.Current.FirstOrDefault(request.FilePath);
        if (file == null || !System.IO.File.Exists(file.Path))
            return NotFound();

        long totalLength = file.Length;
        long start = request.RangeStart ?? 0;
        long end = request.RangeEnd ?? (totalLength - 1);

        if (start < 0 || end >= totalLength || start > end)
            return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable);

        long length = end - start + 1;

        return new StreamingFileResult(
            file,
            start: start,
            length: length,
            progressCallback: (sentBytes) =>
            {
                // 🔥 Do something like log, update DB, signal client, etc.
                Console.WriteLine($"Progress: {sentBytes}/{length} bytes sent");
            }
        );
    }
}
