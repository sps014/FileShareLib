using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SonicShare.WebServer.CustomActions;

public class StreamingFileResult : IActionResult
{
    private readonly string _filePath;
    private readonly string _contentType;
    private readonly long _start;
    private readonly long _length;
    private readonly string _fileName;
    private readonly Action<long> _progressCallback;

    public StreamingFileResult(string filePath, string contentType, long start, long length, string fileName, Action<long> progressCallback)
    {
        _filePath = filePath;
        _contentType = contentType;
        _start = start;
        _length = length;
        _fileName = fileName;
        _progressCallback = progressCallback;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.StatusCode = StatusCodes.Status206PartialContent;
        response.ContentType = _contentType;
        response.Headers["Accept-Ranges"] = "bytes";
        response.Headers["Content-Disposition"] = $"attachment; filename=\"{_fileName}\"";
        response.Headers["Content-Range"] = $"bytes {_start}-{_start + _length - 1}/{new FileInfo(_filePath).Length}";
        response.ContentLength = _length;

        const int bufferSize = 64 * 1024; // 64 KB
        byte[] buffer = new byte[bufferSize];

        using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        fs.Seek(_start, SeekOrigin.Begin);

        long bytesRemaining = _length;
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
            _progressCallback?.Invoke(totalSent);
        }
    }
}
