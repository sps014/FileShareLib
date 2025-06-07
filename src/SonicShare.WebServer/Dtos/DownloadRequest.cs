using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.WebServer.Dtos;

public class DownloadRequest
{
    public string FilePath { get; set; } = string.Empty;
    public long? RangeStart { get; set; }
    public long? RangeEnd { get; set; }
}