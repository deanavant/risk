using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using risk.Models;

namespace risk.Controllers
{
    public class HomeController : Controller
    {
        private RiskContext _context;

        public HomeController (RiskContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            Game myGame = new Game();
            Console.Write(myGame.territories["Peru"].name);
            
            Console.WriteLine(myGame.territories["Peru"].bottomRightX);
            return View("index");
        }
    }
}
