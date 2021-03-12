namespace PACS_Objects
{
    public class Planet
    {
        private string _codePlanet;
        private int _idPlanet;
        private string _ipPlanet;
        private string _namePlanet;
        private string _portPlanet;

        public void Insert(int id, string code, string name, string ip, string port)
        {
            _idPlanet = id;
            _codePlanet = code;
            _namePlanet = name;
            _ipPlanet = ip;
            _portPlanet = port;
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

        public string GetPort()
        {
            return _portPlanet;
        }
    }
}