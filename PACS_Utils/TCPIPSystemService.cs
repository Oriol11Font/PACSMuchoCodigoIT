using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;

namespace PACS_Utils
{
    class TCPIPSystemService
    {
        // FUNCIONES RELACIONADAS CON EL SERVIDOR Y EL CLIENTE EN TCP/IP

        public string StartServer (int numPort, TcpListener Listener)
        {
            try
            {
                Listener = new TcpListener(IPAddress.Any, numPort);
                Listener.Start();

                return "[INFO] - Server ON";
            }
            catch
            {
                return "[ERROR] - Failed to start the server";
            }
        }

        public string StopServer (TcpListener Listener)
        {
            try
            {
                Listener.Stop();

                return "[INFO] - Server OFF";
            } 
            catch 
            {
                return "[ERROR] - Failed to stop the Server Process";
            }
            
        }

        private string WaitingForResponse (TcpListener Listener)
        {
            if (Listener.Pending()) // IF THE SERVER RECIVE A RESPONSE FROM CLIENT
            {
                TcpClient client = Listener.AcceptTcpClient();
                NetworkStream ns = client.GetStream();
                if (ns.CanRead)
                {
                    byte[] buffer = new byte[1024]; // NEW BUFFER
                    StringBuilder missatge = new StringBuilder();
                    int numberOfBytesRead = 0;
                    // POSSIBLE MESSAGE BIGGER THAN BUFFER
                    do
                    {
                        numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                        missatge.AppendFormat("{0}",
                        Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                    }
                    while (ns.DataAvailable); // READ ALL MESSAGE
                    string mssg = "" + missatge;
                    return mssg;
                }
                else
                {
                    return "Ho sento.No puc llegir aquest stream.";
                }
            }

            return null;
        }

        public string SendMessageToServer (string mssg, string ServerIP, int PortIP)
        {
            try
            {
                TcpClient client = new TcpClient(ServerIP, PortIP);
                Byte[] dades = Encoding.ASCII.GetBytes(mssg);
                NetworkStream ns = client.GetStream();
                ns.Write(dades, 0, dades.Length);

                return "[INFO] - Message Succesfully sended!";
            }
            catch
            {
                return "[ERROR] - Failed to send message!";
            }
        }

        public string checkXarxa(string IPToPing, int Repeat)
        {
            try
            {
                String ping = "";
                bool connErr = false;

                for (int i = 0; i < Repeat; i++)
                {
                    Ping myPing = new Ping();
                    PingReply reply = myPing.Send(IPToPing, 1000);
                    if (reply != null)
                    {
                        if (reply.Status != IPStatus.Success)
                        {
                            connErr = true;
                            ping = reply.Status.ToString();
                        }
                    }
                }

                if (connErr)
                {
                    return "[ERROR] - Failed to ping to " + IPToPing + ". [INFO] - " + ping;
                }
                else
                {
                    return "[INFO] - Succesfully connected!";
                }

            }
            catch
            {
                return "[ERROR] - We can't ping to " + IPToPing;
            }
        }
    }
}
