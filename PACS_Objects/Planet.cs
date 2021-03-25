namespace PACS_Objects
{
    public class Planet
    {
        private readonly string _codePlanet;
        private readonly int _idPlanet;
        private readonly string _ipPlanet;
        private readonly string _namePlanet;
        private readonly int _portPlanet;
        private readonly int _portPlanet1;

        public Planet(int id, string code, string name, string ip, int port, int port1)
        {
            _idPlanet = id;
            _codePlanet = code;
            _namePlanet = name;
            _ipPlanet = ip;
            _portPlanet = port;
            _portPlanet1 = port1;
        }

        public int GetId()
        {
            return _idPlanet;
        }

        public string GetCode()
        {
            return _codePlanet;
        }

        public string GetName()
        {
            return _namePlanet;
        }

        public string GetIp()
        {
            return _ipPlanet;
        }

        public int GetPort()
        {
            return _portPlanet;
        }

        public int GetPort1()
        {
            return _portPlanet1;
        }
    }
}