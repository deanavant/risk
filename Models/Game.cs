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
            emptyPlayer = new Player("Player7","white",7,9);
            territories = LoadTerritoriesToDictionary();
            setup_phase = true;
            turn_phase = "claim";
            rand = new Random();

        }

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
                        territories[temp.name] = temp;
                    }

                    foreach (var territory in jsonTerritories) {
                        List<Territory> tempList = new List<Territory>();
                        foreach(var name in territory.Value["Neighbors"]) {
                            // Console.WriteLine((string)name);
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

        public bool AttackTerritory(Territory attacker, Territory foe)
        {
            // attacker and foe are territories
            if (attacker.IsNeighbor(foe) 
                && attacker.IsFoe(foe) 
                && attacker.armies > 1
                && foe.armies > 0)
            {
                // do the attack here
                
                int dice = attacker.armies >= 4 ? 3 : attacker.armies - 1;
                int[] atk = new int[dice];
                for(int i = 0;i < dice;i++)
                {
                    atk[i] = DieRoll();
                }
                Array.Sort(atk);
                Array.Reverse(atk);
                int d = foe.armies >= 2 ? 2 : 1;
                int[] def = new int[d];
                for(int i = 0;i < d;i++)
                {
                    def[i] = DieRoll();
                }
                Array.Sort(def);
                Array.Reverse(def);
                int aloss = 0;
                int dloss = 0;
                for (int i = 0;i < Math.Min(def.Length, atk.Length) ;i++)
                {
                    if(atk[i] > def[i])
                    {
                        dloss++;
                    } else
                    {
                        aloss++;
                    }
                }
                attacker.armies -= aloss;
                foe.armies -= dloss;
                if (foe.armies == 0 && attacker.armies >= 2)
                {
                    foe.owner = attacker.owner;
                    foe.armies = attacker.armies - 1;
                    attacker.armies = 1;
                    this.current_turn_player.selectedTerritory = foe;
                }
                return true;
            }
            
            // foe must be a neighbor of attacker
            // attacker must belong to current player
            // attacker army count must be >= 2
            // foe must not belong to current player
            // defender uses 2 dice unless they have 1 army

            return false;
        }

        public void MoveUnits(Territory origin, Territory destination)
        {
            destination.armies += origin.armies -1;
            origin.armies = 1;
        }

        public bool AllClaimed()
        {
            int claim = 0;
            foreach(KeyValuePair<string,Territory> t in territories)
            {
                if(t.Value.owner.name == "Player7")
                {
                    claim++;
                }
            }
            return claim == 0;
        }

        public bool InitRein(string t_name)
        {   
            if(territories[t_name].AddArmies(current_turn_player) )
            {
                if(RemainingDepUnits() == 0)
                {
                    current_turn_player = players[0];
                    current_turn_player.placement_units = CalcRein();
                    Console.WriteLine("****** rein phase now");
                    turn_phase = "rein";
                    return true;
                }
                AdvancePlayer();
                while(current_turn_player.placement_units == 0)
                {
                    AdvancePlayer();
                }
                return true; 
            }
            return false;
        }

        public bool Reinforce(string t_name)
        {
            if(territories[t_name].AddArmies(current_turn_player) )
            {
                if (current_turn_player.placement_units == 0)
                {
                    turn_phase = "attack";
                    Console.WriteLine("****** attack phase now");
                }
                return true;
            }
            return false;
        }

        public bool ClaimTerritory(string t_name){
            if (territories[t_name].Claim(current_turn_player) )
            {
                if (AllClaimed() )
                {
                    turn_phase = "init_rein";
                    current_turn_player = players[0];
                    Console.WriteLine("***** all territories claimed");
                } else
                {
                    AdvancePlayer();
                    Console.WriteLine("***** advancing to next player");
                }
                return true;
            }
            Console.WriteLine("******* did not claim territory");
            return false;
        }

        public void AdvancePlayer()
        {
            int i = current_turn_player.order_number + 1;
            if (i >= players.Count)
            {
                i = 0;
            }
            current_turn_player = players[i];
        }

        public int CalcRein()
        {
            int result = 0;
            foreach(KeyValuePair<string,Territory> t in territories)
            {
                if(t.Value.owner.name == current_turn_player.name)
                {
                    result++;
                }
            }
            int subtotal = result/3;
            result = subtotal < 3 ? 3 : subtotal;
            return result;
        }

        public int RemainingDepUnits()
        {
            int count = 0;
            foreach(var p in players)
            {
                count += p.placement_units;
            }
            return count;
        }
    }
}