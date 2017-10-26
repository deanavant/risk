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

        public Player(string name, string color, int order, int units = 0)
        {
            this.name = name;
            this.color = color;
            order_number = order;
            placement_units = units;
        }
         public static List<Player> createPlayers(int num_players)
        {
            Player player;
            List<Player> players = new List<Player>();
            String[] arr = new String[]{"Red","Blue","Yellow","Green","Black","Orange"};
            var unit = 50;
             if (num_players == 3)
            {
                unit = 35;
            } else if (num_players == 4)
            {
                unit = 30;
            } else if (num_players == 5)
            {
                unit = 25;
            } else if (num_players == 6)
            {
                unit = 20;
            }

            for(int i=1;i<=num_players;i++)
            {
                var name = "Player"+i;
                var color = arr[i];
                var order = i;
                player = new Player(name,color,order,unit);
                players.Add(player);
            }

            return players;
            }
        }
       
    }
