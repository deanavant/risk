using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

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
        public List<Territory> territories { get; set; }
        
        [NotMapped]
        public List<Player> players { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        public Game()
        {
            territories = new List<Territory>();
            rand = new Random();
        }

        public static int DieRoll(int sides = 6)
        {
            return rand.Next(sides);
        }

        public bool AttackTerritory(Territory attacker, Territory foe, int dice = 2)
        {
            // attacker and foe are territories
            if (attacker.isNeighbor(foe) && attacker.isFoe(foe) && attacker.armies > 1)
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