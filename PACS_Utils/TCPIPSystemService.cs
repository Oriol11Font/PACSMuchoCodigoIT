using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using PACS_Objects;

namespace PACS_Utils
{
    public class TcpipSystemService
    {
        private readonly RsaKeysService _rsa = new RsaKeysService();

        private bool _code;
        // FUNCIONES RELACIONADAS CON EL SERVIDOR Y EL CLIENTE EN TCP/IP

        public static TcpListener StartServer(int numPort, TcpListener listener)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, numPort);
                listener.Start();

                return listener;
            }
            catch
            {
                return null;
            }
        }

        public static TcpListener StopServer(TcpListener listener)
        {
            try
            {
                listener.Stop();

                return listener;
            }
            catch
            {
                return null;
            }
        }

        public string WaitingForResponse(TcpListener listener, Planet planet)
        {
            if (listener.Pending())
            {
                var client = listener.AcceptTcpClient();
                var ns = client.GetStream();
                if (!ns.CanRead) return "Ho sento. No puc llegir aquest stream.";
                var missatge = new StringBuilder();
                var buffer = new byte[1024]; // NEW BUFFER
                var bufferCode = new byte[128];
                // POSSIBLE MESSAGE BIGGER THAN BUFFER
                if (_code)
                {
                    //do
                    //{
                    ns.Read(bufferCode, 0, bufferCode.Length);
                    //} while (ns.DataAvailable);

                    _code = false;

                    return RsaKeysService.DecryptCode(bufferCode, planet.GetCode());
                }

                do
                {
                    int numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                    missatge.AppendFormat("{0}",
                        Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                } while (ns.DataAvailable);
                // READ ALL MESSAGE

                var mssg = missatge.ToString();

                if (mssg == "code") _code = true;

                ns.Close();
                client.Close();

                return mssg;
            }
            else
            {
                return null;
            }
        }

        public static string SendMessageToServer(string mssg, string serverIp, int portIp)
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

        public static string SendMessageToServer(byte [] mssg, string serverIp, int portIp)
        {
            try
            {
                var client1 = new TcpClient(serverIp, portIp);
                var ns1 = client1.GetStream();
                ns1.Write(mssg, 0, mssg.Length);
                return "[SYSTEM] - Message Succesfully sended!";
            }
            catch
            {
                return "[ERROR] - Failed to send message!";
            }
        }

        public static bool CheckXarxa(string ipToPing, int repeat)
        {
            var check = true;
            try
            {
                for (var i = 0; i < repeat; i++)
                {
                    var myPing = new Ping();
                    var reply = myPing.Send(ipToPing, 1000);
                    if (reply == null) continue;
                    if (reply.Status != IPStatus.Success)
                        check = false;
                }

                return check;
            }
            catch
            {
                return false;
            }
        }

        public string SendFile(string filePath, string ip, int port)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);

                var client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                client.Connect(endpoint);

                //Console.WriteLine("Sending {0} to the host.", filePath);
                client.SendFile(filePath);

                client.Shutdown(SocketShutdown.Both);
                client.Close();

                return "[SYSTEM] - S'ha enviat correctament el fitxer";
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                return "[ERROR] - No s'ha pogut enviar el fitxer: "+ e.ToString();
            }
        }

        public static string RecivedFile(TcpListener listener, string filePath)
        {
            try
            {
                while (listener.Pending())
                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                        if (stream.CanRead)
                        {
                            using (var output = File.Create(filePath + "PACS.zip"))
                            {
                                var buffer = new byte[1024];
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                }
                            }

                            return "[SYSTEM] - Client connected. Starting to receive the file!";
                        }
                return null;
            }
            catch 
            {
                return null;
            }
        }
    }
}