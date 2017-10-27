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
        public static Game myGame;
        public static bool game_started = false;

        public HomeController (RiskContext context)
        {
            Console.WriteLine("New Home Controller!!!!");
            
            _context = context;
            if (myGame == null) {
                myGame = new Game();
                Console.WriteLine("new game made by new home controller");
            }
            // game_started = false;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {

            ViewBag.game = myGame.territories;
            ViewBag.game_started = game_started;
            Console.WriteLine("In Index");
            return View("index");

        }

        [HttpPost]
        [Route("start")]
        public IActionResult StartGame(int num_player)
        {
            Console.WriteLine("In StartGame");
            game_started = true;
            ViewBag.game_started = game_started;
            myGame.players = Player.createPlayers(num_player);
            return Redirect("/");
        }
    }
}
