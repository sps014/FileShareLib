using SonicShare.WebServer.Models;

namespace SonicShare.WebServer;

public class FileManager
{
    private List<FileItem> filesToShare = new();
    private static object lockObject= new object();

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
        lock (lockObject)
        {
            if (filesToShare.Contains(fileItem))
                return false;

            filesToShare.Add(fileItem);
            return true;
        }
    }

    public FileItem? FirstOrDefault(string path)
    {
        lock (lockObject)
        {
            return filesToShare.FirstOrDefault(x => x.Path == path);
        }
    }

    public bool Remove(FileItem fileItem)
    {
        lock (lockObject)
        {
            return filesToShare.Remove(fileItem);
        }
    }

    public void Clear()
    {
        lock (lockObject)
        {
            filesToShare.Clear();
        }
    }

    public IEnumerable<FileItem> GetAll()
    {
        lock (lockObject)
        {
            return filesToShare;
        }
    }
}
