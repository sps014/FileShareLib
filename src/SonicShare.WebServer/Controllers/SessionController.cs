using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SonicShare.WebServer.Models;
using SonicShare.WebServer.Services;

namespace SonicShare.WebServer.Controllers;


[ApiController]
[Route("api/session")]
public class SessionController:Controller
{

    [HttpPost("/create")]
    public IActionResult CreateSession([FromBody] DeviceInfo deviceInfo)
    {
        Session? session = SessionManager.Current.CreateSession(deviceInfo);
         if (session != null)
            return Ok(session);

        return BadRequest("Can't create session");
    }

    [HttpGet("/close/{fingerprintId}")]
    public IActionResult CreateSession(string fingerprintId)
    {
        if (SessionManager.Current.CloseSession(fingerprintId))
            return Ok();

        return BadRequest("Can't close session");
    }
}
