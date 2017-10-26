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
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (myGame == null) {
                myGame = new Game();
            }

            ViewBag.game = myGame.territories;

            return View("index");
        }

        [HttpPost]
        [Route("start/{num_players}")]
        public IActionResult StartGame(int num_players)
        {

            // myGame.players = Player.createPlayers(num_players);
            return Redirect("index");
        }
    }
}
