using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace risk.Models
{
    public class Territory
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MinLength(1)]
        public string name { get; set; }

        [ForeignKey("owner_id")]
        public Player owner { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        [NotMapped]
        public int topLeftX { get; set; }

        [NotMapped]
        public int topLeftY { get; set; }

        [NotMapped]
        public int bottomRightX { get; set; }

        [NotMapped]
        public int bottomRightY { get; set; }

        [NotMapped]
        public List<Territory> neighbors { get; set; }

        [NotMapped]
        public int armies { get; set; }

        public Territory()
        {
            neighbors = new List<Territory>();
            armies = 0;
        }

        public bool AddNeighbor(Territory n)
        {
            if(!neighbors.Contains(n) )
            {
                neighbors.Add(n);
                return true;
            }
            return false;
        }

        public bool IsNeighbor(Territory n)
        {
            return neighbors.Contains(n);
        }
        public bool IsFoe(Territory n)
        {
            return owner != n.owner;
        }

        public bool AddArmies(Player player, int armies = 1)
        {
            if(armies != 0)
            {
                if(this.armies == 0)
                {
                    owner = player;
                }
                this.armies = armies;
                return true;
            }
            return false;
        }
    }
}
