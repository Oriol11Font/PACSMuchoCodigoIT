using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace PACS_Utils
{
    public class TcpipSystemService
    {
        // FUNCIONES RELACIONADAS CON EL SERVIDOR Y EL CLIENTE EN TCP/IP

        public string StartServer(int numPort, TcpListener listener)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, numPort);
                listener.Start();

                return "[SYSTEM] - Server ON";
            }
            catch
            {
                return "[ERROR] - Failed to start the server";
            }
        }

        public string StopServer(TcpListener listener)
        {
            try
            {
                listener.Stop();
                return "[SYSTEM] - Server OFF";
            }
            catch 
            {
                return "[ERROR] - Failed to stop the Server Process";
            }
        }

        public string WaitingForResponse (TcpListener Listener)
        {
            if (Listener.Pending()) // IF THE SERVER RECIVE A RESPONSE FROM CLIENT
            {
                var client = Listener.AcceptTcpClient();
                var ns = client.GetStream();
                if (ns.CanRead)
                {
                    var buffer = new byte[1024]; // NEW BUFFER
                    var missatge = new StringBuilder();
                    var numberOfBytesRead = 0;
                    // POSSIBLE MESSAGE BIGGER THAN BUFFER
                    do
                    {
                        numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                        missatge.AppendFormat("{0}",
                            Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                    } while (ns.DataAvailable); // READ ALL MESSAGE

                    var mssg = "" + missatge;
                    return mssg;
                }

                return "Ho sento.No puc llegir aquest stream.";
            }

            return null;
        }

        public string SendMessageToServer(string mssg, string serverIp, int portIp)
        {
            try
            {
                var client = new TcpClient(serverIp, portIp);
                var dades = Encoding.ASCII.GetBytes(mssg);
                var ns = client.GetStream();
                ns.Write(dades, 0, dades.Length);

                return "[SYSTEM] - Message Succesfully sended!";
            }
            catch
            {
                return "[ERROR] - Failed to send message!";
            }
        }

        public string CheckXarxa(string ipToPing, int repeat)
        {
            try
            {
                var ping = "";
                var connErr = false;

                for (var i = 0; i < repeat; i++)
                {
                    var myPing = new Ping();
                    var reply = myPing.Send(ipToPing, 1000);
                    if (reply != null)
                        if (reply.Status != IPStatus.Success)
                        {
                            connErr = true;
                            ping = reply.Status.ToString();
                        }
                }

                if (connErr)
                {
                    return "[ERROR] - Failed to ping to " + ipToPing + ". [INFO] - " + ping;
                }
                else
                {
                    return "[SYSTEM] - Succesfully connected!";
                }
            }
            catch
            {
                return "[ERROR] - We can't ping to " + ipToPing;
            }
        }
    }
}