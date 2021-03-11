
namespace PACS_Objects
{
    public class Planet
    {
        int IdPlanet;
        string CodePlanet;
        string NamePlanet;
        string IpPlanet;
        int PortPlanet;

        public Planet (int id, string Code, string Name, string Ip, int Port)
        {
            IdPlanet = id;
            CodePlanet = Code;
            NamePlanet = Name;
            IpPlanet = Ip;
            PortPlanet = Port;
        }

        public int getId()
        {
            return IdPlanet;
        }

        public string getCode()
        {
            return CodePlanet;
        }

        public string getName()
        {
            return NamePlanet;
        }

        public string getIp()
        {
            return IpPlanet;
        }
        public int getPort()
        {
            return PortPlanet;
        }

    }
}
