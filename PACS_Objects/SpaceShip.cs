﻿namespace PACS_Objects
{
    public class SpaceShip
    {
        private readonly string _codeSpaceShip;
        private readonly int _idSpaceShip;
        private readonly string _ipSpaceShip;
        private readonly int _portSpaceShip1;
        private readonly int _portSpaceShip2;

        public SpaceShip(int id, string code, string ip, int port, int port2)
        {
            _idSpaceShip = id;
            _codeSpaceShip = code;
            _ipSpaceShip = ip;
            _portSpaceShip1 = port;
            _portSpaceShip2 = port2;
        }

        public int GetId() => _idSpaceShip;

        public string GetCode() => _codeSpaceShip;

        public string GetIp() => _ipSpaceShip;

        public int GetPort1() => _portSpaceShip1;

        public int GetPort2 => _portSpaceShip2;
    }
}