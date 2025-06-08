using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SonicShare.WebServer.Models;

namespace SonicShare.WebServer.CustomActions;

public class StreamingFileResult : IActionResult
{
    private readonly string filePath;
    private readonly string contentType;
    private readonly long start;
    private readonly long length;
    private readonly bool isPartial;
    private readonly string fileName;
    private readonly Action<long> progressCallback;

    public StreamingFileResult(FileItem fileItem, long _start, long _length,bool _isPartial, Action<long> _progressCallback)
    {
        filePath = fileItem.Path;
        contentType = fileItem.ContentType;
        start = _start;
        length = _length;
        isPartial = _isPartial;
        fileName = fileItem.Name;
        progressCallback = _progressCallback;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {

        var response = context.HttpContext.Response;

        response.StatusCode = isPartial ? StatusCodes.Status206PartialContent : StatusCodes.Status200OK;

        if (isPartial)
            response.Headers["Content-Range"] = $"bytes {start}-{start + length - 1}/{new FileInfo(filePath).Length}";

        response.ContentType = contentType;
        response.Headers["Accept-Ranges"] = "bytes";
        response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";
        response.ContentLength = length;

        const int bufferSize = 64 * 1024; // 64 KB
        byte[] buffer = new byte[bufferSize];

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        fs.Seek(start, SeekOrigin.Begin);

        long bytesRemaining = length;
        long totalSent = 0;

        while (bytesRemaining > 0)
        {
            int read = await fs.ReadAsync(buffer, 0, (int)Math.Min(bufferSize, bytesRemaining));
            if (read == 0) break;

            await response.Body.WriteAsync(buffer, 0, read);

            totalSent += read;
            bytesRemaining -= read;

            // Optional: flush to send data sooner
            await response.Body.FlushAsync();

            // Callback to report progress
            progressCallback?.Invoke(totalSent);
        }
    }
}
