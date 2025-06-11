using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonicShare.WebServer.Models;

namespace SonicShare.WebServer.Services;

public class SessionManager
{
    private static SessionManager current = new();

    ConcurrentDictionary<Fingerprint, Session> Sessions { get; } = new();

    public static SessionManager Current
    {
        get { return current; }
    }

    public Session? CreateSession(DeviceInfo deviceInfo)
    {
        if(Sessions.ContainsKey(deviceInfo.Fingerprint))
        {
                return null;
        }

        var session = new Session(deviceInfo.Fingerprint);
        return session;
    }

    public bool CloseSession(string fingerprintId)
    {
        var fingerPrint = new Fingerprint(fingerprintId);

        return Sessions.TryRemove(fingerPrint, out _);
    }
}
