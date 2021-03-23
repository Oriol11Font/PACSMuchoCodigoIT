using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_PLANET
{
    class CodificationGenerator
    {
        //Method to return dict with the random codification
        Dictionary<string, string> codification = new Dictionary<string, string>
        {

        };
        public Dictionary<String, String> generateCodification()
        {
            //For each letter in alphabet, associate a pair of 3 random unique numbers
            codification.Add("A", "007");
            Console.WriteLine(codification["A"]);
            return codification;
        }
    }
}
