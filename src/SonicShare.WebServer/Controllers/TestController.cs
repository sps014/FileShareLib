using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SonicShare.WebServer.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet("mvc-test")]
    public IActionResult Index()
    {
        return Ok("Hello All!");
    }
}