using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using risk.Models;

namespace risk.Models
{
    public class Game
    {
        private static Random rand;

        [Key]
        public int id { get; set; }

        [Required]
        public bool setup_phase { get; set; }
        
        [Required]
        public string turn_phase { get; set; }

        [ForeignKey("player_turn_id")]
        public Player current_turn_player { get; set; }

        [NotMapped]
        public Dictionary<string,Territory> territories { get; set; }
        
        [NotMapped]
        public List<Player> players { get; set; }

        [NotMapped]
        public Player emptyPlayer { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        public Game()
        {
            emptyPlayer = new Player("Player7","white",7,0);
            territories = LoadTerritoriesToDictionary();
            setup_phase = true;
            turn_phase = "Place";
            rand = new Random();

        }

        // public void AddPlayer(string name, string color, int order)
        // {
        //     players.Add(new Player(name,color,order));
        // }

        // public void InitialPlacements()
        // {
        //     int p = players.Count;
        //     int armies = 50;
        //     if (p == 3)
        //     {
        //         armies = 35;
        //     } else if (p == 4)
        //     {
        //         armies = 30;
        //     } else if (p == 5)
        //     {
        //         armies = 25;
        //     } else if (p == 6)
        //     {
        //         armies = 20;
        //     }
        //     foreach(Player a in players)
        //     {
        //         a.placement_units = armies;
        //     }
        // }

        private Dictionary<string,Territory> LoadTerritoriesToDictionary() {
            Dictionary<string,Territory> territories = new Dictionary<string,Territory>();

                using (StreamReader file = File.OpenText(@"territories.json"))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject jsonTerritories = (JObject)JToken.ReadFrom(reader);
                    
                    foreach (var territory in jsonTerritories) {
                        // Console.WriteLine(territory.Key);
                        // Console.WriteLine(territory.Value["Neighbors"]);
                        // Console.WriteLine(territory.Value["TopLeftX"]);
                        // break;
                        
                        Territory temp = new Territory();
                        temp.name = territory.Key;
                        temp.owner = emptyPlayer;
                        temp.topLeftX = (int)territory.Value["TopLeftX"];
                        temp.topLeftY = (int)territory.Value["TopLeftY"];
                        temp.bottomRightX = (int)territory.Value["BottomRightX"];
                        temp.bottomRightY = (int)territory.Value["BottomRightY"];
                        temp.neighbors = null;
                        temp.armies = 0;
                        territories[temp.name] = temp;
                    }

                    foreach (var territory in jsonTerritories) {
                        List<Territory> tempList = new List<Territory>();
                        foreach(var name in territory.Value["Neighbors"]) {
                            Console.WriteLine((string)name);
                            tempList.Add(territories[(string)name]);
                        }
                        territories[territory.Key].neighbors = tempList;
                    }      
                }
            return territories; 
        }

        public static int DieRoll(int sides = 6)
        {
            return rand.Next(sides);
        }

        public bool AttackTerritory(Territory attacker, Territory foe, int dice = 2)
        {
            // attacker and foe are territories
            if (attacker.IsNeighbor(foe) && attacker.IsFoe(foe) && attacker.armies > 1)
            {
                // do the attack here
                return true;
            }
            // foe must be a neighbor of attacker
            // attacker must belong to current player
            // attacker army count must be >= 2
            // foe must not belong to current player
            // defender uses 2 dice unless they have 1 army

            return false;
        }
    }
}