using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PACS_Objects
{
    public class Planet
    {
        int idPlanet;
        int CodePlanet;
        string NamePlanet;
        
        public void insert (int id, int Code, string Name)
        {
            idPlanet = id;
            CodePlanet = Code;
            NamePlanet = Name;
        }
    }
}
