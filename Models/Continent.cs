using System;
using System.Collections.Generic;

namespace risk.Models
{
    public class Continent
    {
        public List<Territory> territories;
        public string name { get; set; }
        public int value { get; set; }

        public Continent()
        {
            territories = new List<Territory>();
        }

        public bool AddTerritory(Territory t)
        {
            if (territories.Contains(t) )
            {
                return false;
            }
            territories.Add(t);
            return true;
        }

        public bool OwnedBy(int pid)
        {
            foreach(Territory t in territories)
            {
                if(t.owner.id != pid)
                {
                    return false;
                }
            }
            return true;   
        }
    }
}