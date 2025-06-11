using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.WebServer.Models;

public record Session(Fingerprint Fingerprint,SessionState SessionState=SessionState.Active)
{

}


public enum SessionState
{
    None,
    Active,
    Inactive,
}