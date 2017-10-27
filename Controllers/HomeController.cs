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
                myGame.InitRein(t_name);
            }
            else if (myGame.turn_phase == "rein") {
                myGame.Reinforce(t_name);
            }
            else if (myGame.turn_phase == "attack") {
                //if you own the territory you clicked on
                if (myGame.territories[t_name].owner == myGame.current_turn_player) {
                    if (myGame.current_turn_player.selectedTerritory == myGame.territories[t_name]) {
                        myGame.current_turn_player.selectedTerritory = null;
                    } else {
                        if (myGame.territories[t_name].armies >= 2){
                            myGame.current_turn_player.selectedTerritory = myGame.territories[t_name];
                        }
                    }
                } 
                //if you don't own the territory you clicked on
                else {
                    if (myGame.current_turn_player.selectedTerritory != null && myGame.current_turn_player.selectedTerritory.IsNeighbor(myGame.territories[t_name])){
                        myGame.AttackTerritory(myGame.current_turn_player.selectedTerritory, myGame.territories[t_name]);
                    }
                }
            }
            else if (myGame.turn_phase == "move") {
                //RITU
            }

            return Redirect("/");



        }
    }
}
