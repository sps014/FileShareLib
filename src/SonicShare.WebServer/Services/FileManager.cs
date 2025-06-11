using System.Collections.Concurrent;
using System.Collections.Generic;
using SonicShare.WebServer.Models;

namespace SonicShare.WebServer.Services;

public class FileManager
{
    private ConcurrentDictionary<string, FileItem> filesToShare = new();

    private static FileManager current = new FileManager();

    public static FileManager Current
    {
        get { return current; }
    }

    private FileManager()
    {

    }

    public bool Add(FileItem fileItem)
    {
        if (filesToShare.ContainsKey(fileItem.Hash))
            return false;

        return filesToShare.TryAdd(fileItem.Hash, fileItem);
    }

    public FileItem? GetValueOrDefault(string hash)
    {
        return filesToShare.GetValueOrDefault(hash);
    }

    public bool Remove(FileItem fileItem)
    {
        return filesToShare.Remove(fileItem.Hash, out _);
    }

    public void Clear()
    {
        filesToShare.Clear();
    }

    public IEnumerable<FileItem> GetAll()
    {
        return filesToShare.Values;
    }
}
