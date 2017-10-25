using System.Collections.Generic;
using System;

namespace risk.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Territory> territories { get; set; }

        public Game()
        {
            territories = new List<Territory>();
        }

    }
}