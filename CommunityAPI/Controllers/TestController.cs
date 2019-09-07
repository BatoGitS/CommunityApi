using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityAPI.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }
}
