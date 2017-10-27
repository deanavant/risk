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
            ViewBag.game_started = game_started;
            return View("index");
        }

        [HttpPost]
        [Route("start")]
        public IActionResult StartGame(int num_player)
        {
            game_started = true;
            ViewBag.game_started = game_started;
            myGame.players = Player.createPlayers(num_player);
            myGame.current_turn_player = myGame.players[0];
            return Redirect("/");
        }


        [HttpPost]
        [Route("click/{t_name}")]
        public IActionResult clickTerritory(string t_name)
        {
            if(game_started == false)
            {
                TempData["Error"] = "Please select the number of Players and Start the Game !!!";
                return Redirect("/");
            }   
            
            
            if (myGame.turn_phase == "claim") {
                myGame.ClaimTerritory(t_name);
            }
            else if (myGame.turn_phase == "init_rein") {
                //DEAN
            }
            else if (myGame.turn_phase == "rein") {
                //DEAN
            }
            else if (myGame.turn_phase == "attack") {
                //JOHN
            }
            else if (myGame.turn_phase == "move") {
                //RITU
            }

            
            
            return Redirect("/");



        }
    }
}
