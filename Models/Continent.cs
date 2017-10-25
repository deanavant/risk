using System;
using System.Collections.Generic;

namespace risk.Models
{
    public class Continent
    {
        public List<Territory> territories;

        public Continent()
        {
            // need to gather xml data here and add territories, etc
        }

        public bool hasContinent(int pid)
        {
            // see if given player owns all territories on this continent
            return false;
        }
    }
}