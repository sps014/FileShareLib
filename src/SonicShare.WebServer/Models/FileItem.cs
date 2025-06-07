using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.WebServer.Models;

public record FileItem(string Path,string ContentType)
{
    public FileStream OpenStream()
    {
        return File.OpenRead(Path);
    }

    public long Length=>new FileInfo(Path).Length;
    public string Name => System.IO.Path.GetFileName(Path);


   
}