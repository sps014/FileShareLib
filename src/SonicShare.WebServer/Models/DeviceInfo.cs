using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.WebServer.Models;

public record DeviceInfo(string Name, string Platform, string Model,string Manufracturer,string Idiom)
{
    public Fingerprint Fingerprint
    {
        get
        {
            return UniqueDeviceFingerprint();
        }
    }
    private Fingerprint? fingerprint;

    public Fingerprint UniqueDeviceFingerprint()
    {
        if (fingerprint != null)
            return fingerprint.Value;

        // Concatenate the values to create a unique string
        var rawData = $"{Name}-{Platform}-{Model}-{Manufracturer}-{Idiom}";

        // Create a hash of the concatenated string for uniqueness
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(rawData);
        var hashBytes = sha256.ComputeHash(bytes);

        // Convert hash to hex string
        var id = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        fingerprint = new Fingerprint(id);

        return fingerprint.Value;

    }
}
