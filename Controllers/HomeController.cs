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
        private Game myGame;

        public HomeController (RiskContext context)
        {
            _context = context;
            if (myGame == null) {
                myGame = new Game();
            }
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
             
            ViewBag.game = myGame.territories;
            return View("index");

        }

        [HttpPost]
        [Route("start")]
        public IActionResult StartGame(int num_player)
        {
            myGame.players = Player.createPlayers(num_player);
            return Redirect("/");
        }
    }
}
