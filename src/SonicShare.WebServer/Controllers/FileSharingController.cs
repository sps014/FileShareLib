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
[Route("api/shared")]
public class FileSharingController:Controller
{
    [HttpGet("getAll")]
    public IEnumerable<FileItemDto> GetAll()
    {
        foreach(var item in  FileManager.Current.GetAll().OrderBy(x=>Path.GetExtension(x.Path)))
        {
            yield return new FileItemDto(item.Hash, item.Name, item.ContentType);
        }
    }

    [HttpGet("download")]
    public IActionResult DownloadFile([FromQuery] string hash)
    {
        // ... validate request, get file info ...

        var file = FileManager.Current.GetValueOrDefault(hash);
        if (file == null || !System.IO.File.Exists(file.Path))
            return NotFound();

        long totalLength = file.Length;
        string? rangeHeader = Request.Headers["Range"];
        long start = 0;
        long end = totalLength - 1;
        bool isPartial = false;

        if (!string.IsNullOrEmpty(rangeHeader) && rangeHeader.StartsWith("bytes="))
        {
            var range = rangeHeader["bytes=".Length..].Split('-');

            if (long.TryParse(range[0], out var parsedStart))
                start = parsedStart;

            if (range.Length > 1 && long.TryParse(range[1], out var parsedEnd))
                end = parsedEnd;

            // Clamp and validate range
            if (start >= totalLength || end >= totalLength || start > end)
                return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable);

            isPartial = true;
        }

        long length = end - start + 1;



        return new StreamingFileResult(
            file,
            _start: start,
            _length: length,
            isPartial,
            _progressCallback: (sentBytes) =>
            {
                // 🔥 Do something like log, update DB, signal client, etc.
                Console.WriteLine($"Progress: {sentBytes}/{length} bytes sent");
            }
        );
    }
}
