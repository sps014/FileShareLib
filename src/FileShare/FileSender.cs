using System.Net;
using System.Net.Sockets;
using System.Net.Http;

namespace FileShare;

public class FileSender
{
    public IPAddress IpAddress { get; }
    public int Port { get; }
    private HttpCli _client;
    public FileSender(IPAddress address, int port)
    {
        IpAddress = address;
        Port = port;
    }
}
