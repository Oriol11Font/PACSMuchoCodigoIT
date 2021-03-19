using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PACS_Objects
{
    public class SpaceShip
    {
        int IdSpaceShip;
        string CodeSpaceShip;
        string IpSpaceShip;
        int PortSpaceShip;

        public SpaceShip(int id, string Code, string Ip, int Port)
        {
            IdSpaceShip = id;
            CodeSpaceShip = Code;
            IpSpaceShip = Ip;
            PortSpaceShip = Port;
        }

        public int getId()
        {
            return IdSpaceShip;
        }

        public string getCode()
        {
            return CodeSpaceShip;
        }

        public string getIp()
        {
            return IpSpaceShip;
        }
        public int getPort()
        {
            return PortSpaceShip;
        }
    }
}
