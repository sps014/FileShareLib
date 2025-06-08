using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
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

    private string? hash;
    public string Hash
    {
        get
        {
            if(hash == null)
            {
                hash = BitConverter.ToString(MD5.HashData(Encoding.UTF8.GetBytes(Path))).Replace("-",string.Empty).ToLowerInvariant();
            }

            return hash;
        }
    }


   
}