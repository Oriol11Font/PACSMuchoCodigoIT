using PACS_Objects;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace PACS_Utils
{
    public class TcpipSystemService
    {
        bool code = false;
        RsaKeysService rsa = new RsaKeysService();
        // FUNCIONES RELACIONADAS CON EL SERVIDOR Y EL CLIENTE EN TCP/IP

        public TcpListener StartServer(int numPort, TcpListener listener)
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

        public TcpListener StopServer(TcpListener Listener)
        {
            try
            {
                Listener.Stop();

                return Listener;
            }
            catch
            {
                return null;
            }
        }

        public string WaitingForResponse(TcpListener Listener, Planet planet)
        {
            if (Listener.Pending()) // IF THE SERVER RECIVE A RESPONSE FROM CLIENT
            {
                var client = Listener.AcceptTcpClient();
                var ns = client.GetStream();
                string decryptedCode = string.Empty;
                if (ns.CanRead)
                {
                    var buffer = new byte[1024]; // NEW BUFFER
                    var missatge = new StringBuilder();
                    var numberOfBytesRead = 0;
                    var mssg = string.Empty;
                    // POSSIBLE MESSAGE BIGGER THAN BUFFER
                    if (code)
                    {
                        do
                        {
                            numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                        } while (ns.DataAvailable);

                        code = false;
                        return "VK" + rsa.DecryptCode(buffer, planet.GetCode());

                    } else
                    {
                        do
                        {
                            numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                            missatge.AppendFormat("{0}",
                                Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                        } while (ns.DataAvailable);
                    }
                    // READ ALL MESSAGE

                    

                    if (mssg == "code")
                    {
                        code = true;
                    }

                    return missatge.ToString();
                }
                else
                {
                    return "Ho sento. No puc llegir aquest stream.";
                }
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

        public string SendMessageToServer(byte [] mssg, string serverIp, int portIp)
        {
            try
            {
                var client = new TcpClient(serverIp, portIp);
                var ns = client.GetStream();
                ns.Write(mssg, 0, mssg.Length);

                return "[SYSTEM] - Message Succesfully sended!";
            }
            catch
            {
                return "[ERROR] - Failed to send message!";
            }
        }

        public bool CheckXarxa(string ipToPing, int repeat)
        {
            var check = true;
            try
            {
                for (var i = 0; i < repeat; i++)
                {
                    var myPing = new Ping();
                    var reply = myPing.Send(ipToPing, 1000);
                    if (reply != null)
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
                var endpoint = new IPEndPoint(long.Parse(ip), port);

                var client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                client.Connect(endpoint);

                Console.WriteLine("Sending {0} to the host.", filePath);
                client.SendFile(filePath);

                client.Shutdown(SocketShutdown.Both);
                client.Close();

                return @"[SYSTEM] S'ha enviat correctament el fitxer";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return @"[ERROR] No s'ha pogut enviar el fitxer";
            }
        }
    }
}