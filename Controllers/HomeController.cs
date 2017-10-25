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
            return View("index");
        }
    }
}
