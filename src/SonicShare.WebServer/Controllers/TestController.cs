using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SonicShare.WebServer.Controllers;


public class TestController : Controller
{
    [HttpGet("/mvc-test")]
    public IActionResult Index()
    {
        return Content("Hello MVC!");
    }
}