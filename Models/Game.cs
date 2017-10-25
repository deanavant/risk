using System.Collections.Generic;
using System;

namespace risk.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string State { get; set; }
        public List<Territory> territories { get; set; }
        public List<Player> players { get; set; }

        public Game()
        {
            territories = new List<Territory>();
        }

    }
}