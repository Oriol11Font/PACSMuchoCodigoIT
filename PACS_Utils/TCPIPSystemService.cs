﻿using System;
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
            if (!listener.Pending()) return null;
            var client = listener.AcceptTcpClient();
            var ns = client.GetStream();
            var decryptedCode = string.Empty;
            if (!ns.CanRead) return "Ho sento. No puc llegir aquest stream.";
            var buffer = new byte[1024]; // NEW BUFFER
            var missatge = new StringBuilder();
            var numberOfBytesRead = 0;
            var mssg = string.Empty;
            // POSSIBLE MESSAGE BIGGER THAN BUFFER
            if (_code)
            {
                do
                {
                    numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                } while (ns.DataAvailable);

                _code = false;
                return "VK" + RsaKeysService.DecryptCode(buffer, planet.GetCode());
            }

            do
            {
                numberOfBytesRead = ns.Read(buffer, 0, buffer.Length);
                missatge.AppendFormat("{0}",
                    Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
            } while (ns.DataAvailable);
            // READ ALL MESSAGE


            if (mssg == "code") _code = true;

            return missatge.ToString();

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

        public static string SendMessageToServer(byte[] mssg, string serverIp, int portIp)
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

        public static string RecivedFile(TcpListener listener, string filePath)
        {
            try
            {
                while (listener.Pending())
                    using (var client = listener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    using (var output = File.Create(filePath + "recivedFile.zip"))
                    {
                        Console.WriteLine("[SYSTEM] - Client connected. Starting to receive the file");

                        // read the file in chunks of 1KB
                        var buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            output.Write(buffer, 0, bytesRead);
                    }

                return "[SYSTEM] - File recived!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return @"[ERROR] - System can't recive the file!";
            }
        }

        public void ReciveFile2(TcpListener listener, string filePath)
        {
            //try
            //{
            //    string message = "Accept the Incoming File ";
            //    string caption = "Incoming Connection";

            //    DialogResult result;

            //    if (Listener.Pending())
            //    {
            //        client = Listener.AcceptTcpClient();
            //        netstream = client.GetStream();
            //        Status = "Connected to a client\n";
            //        result = MessageBox.Show(message, caption, buttons);

            //        if (result == System.Windows.Forms.DialogResult.Yes)
            //        {
            //            string SaveFileName = string.Empty;
            //            SaveFileDialog DialogSave = new SaveFileDialog();
            //            DialogSave.Filter = "All files (*.*)|*.*";
            //            DialogSave.RestoreDirectory = true;
            //            DialogSave.Title = "Where do you want to save the file?";
            //            DialogSave.InitialDirectory = @"C:/";
            //            if (DialogSave.ShowDialog() == DialogResult.OK)
            //                SaveFileName = DialogSave.FileName;
            //            if (SaveFileName != string.Empty)
            //            {
            //                int totalrecbytes = 0;
            //                FileStream Fs = new FileStream
            // (SaveFileName, FileMode.OpenOrCreate, FileAccess.Write);
            //                while ((RecBytes = netstream.Read
            //     (RecData, 0, RecData.Length)) > 0)
            //                {
            //                    Fs.Write(RecData, 0, RecBytes);
            //                    totalrecbytes += RecBytes;
            //                }
            //                Fs.Close();
            //            }
            //            netstream.Close();
            //            client.Close();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    //netstream.Close();
            //}
        }
    }
}