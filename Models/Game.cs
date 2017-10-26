using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        public Game()
        {
            territories = LoadTerritoriesToDictionary();
            rand = new Random();
        }

        private static Dictionary<string,Territory> LoadTerritoriesToDictionary() {
            Dictionary<string,Territory> territories = new Dictionary<string,Territory>();

                // JObject o1 = JObject.Parse(File.ReadAllText(@"c:\videogames.json"));

                // // read JSON directly from a file

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
                        temp.owner = null;
                        temp.topLeftX = (int)territory.Value["TopLeftX"];
                        temp.topLeftY = (int)territory.Value["TopLeftY"];
                        temp.bottomRightX = (int)territory.Value["BottomRightX"];
                        temp.bottomRightY = (int)territory.Value["BottomRightY"];
                        temp.neighbors = null;
                        temp.armies = 0;

                        territories[temp.name] = temp;

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