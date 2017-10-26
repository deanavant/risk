using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;


namespace risk.Models
{
    public class Player
    {
        [Key]
        public int id {get; set;}

        [Required]
        [MinLength(1)]
        public string name { get; set; }

        [Required]
        public string color { get; set; }

        [Required]
        public int order_number { get; set; }

        [Required]
        public int placement_units { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created_at { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime updated_at { get; set; }

        public Player(string name, string color, int order, int units)
        {
            this.name = name;
            this.color = color;
            order_number = order;
            placement_units = units;
        }
    }
}