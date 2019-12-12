
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

/*********************************************************

CommunicationSetup.cs stores the classes that set up a TCP or UDP connection and starting sockets.

Still working on UDP communication

 *********************************************************/

namespace R2000_Library
{
    public class Connect
    {
        public void connecttcp()
        {
            try
            {
                //Reqesting a Port and Handle from the R2000, Storing them in Var.Port and Var.Handle

                //Variables
                string responseFromR2000;
                //Create a request for the URL.
                WebRequest request = WebRequest.Create($"http://{Var.IPaddress}/cmd/request_handle_tcp");
                //Get the response.
                WebResponse response = request.GetResponse();
                Console.WriteLine("Establishing Communication \r\n\r\nRequesting TCP Handle and Port\r\nSending: http://" + Var.IPaddress + "/cmd/request_handle_tcp");
                //Display the status.
                Console.WriteLine("Connection Status: " + ((HttpWebResponse)response).StatusDescription);

                //Get the stream returned by the server.
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromR2000 = reader.ReadToEnd();
                }
                response.Close();

                //Finding and storing the Port and Handle number. 
                int PortIndex = responseFromR2000.IndexOf("port");
                int HandleIndex = responseFromR2000.IndexOf("handle");

                string Portgroup = responseFromR2000.Substring((PortIndex + 6), 6);
                Var.Port = Convert.ToInt32(Portgroup.Substring(0, (Portgroup.IndexOf(','))));

                string Handlegroup = responseFromR2000.Substring((HandleIndex + 9), 17);
                Var.Handle = Handlegroup.Substring(0, (Handlegroup.IndexOf('"')));

                Console.WriteLine($"Port = {Var.Port}");
                Console.WriteLine($"TCP Handle = {Var.Handle}\r\n");

                Var.connection = true;
            }
            catch
            {
                Console.WriteLine("Failed to Establish Communication");
                Var.connection = false;
            }
        }
        public void gettcpsocket()
        {
            //Sending the http command to connect to previouse established Port
            try
            {
                Var.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Establishing TCP Connection to " + Var.IPaddress + " at port " + Var.Port);
                Var.Socket.Connect(IPAddress.Parse(Var.IPaddress), Var.Port);
                Console.WriteLine("Connection established\r\n");
                Var.connection = true;
            }
            catch
            {
                Console.WriteLine("Failed to establish TCP socket");
                Var.connection = false;
            }

        }

        public void connectudp()
        {
            var connect = new Connect();
            connect.connecttcp();

            try
            {
                //Variables
                string responseFromR2000;
                //Create a request for the URL.
                WebRequest request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/request_handle_udp?address=" + Var.IPaddress + "&port=" + Var.Port + "&packet_type=" + Var.ScanDataType);
                //Get the response.
                WebResponse response = request.GetResponse();
                Console.WriteLine("Requesting UDP Handle \r\nSending: http://" + Var.IPaddress + "/cmd/request_handle_udp?address=" + Var.IPaddress + "&port=" + Var.Port + "&packet_type=" + Var.ScanDataType);
                //Display the status.
                Console.WriteLine("Connection Status: " + ((HttpWebResponse)response).StatusDescription);

                //Get the stream returned by the server.
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromR2000 = reader.ReadToEnd();
                    Console.WriteLine("responseFromR2000 = " + responseFromR2000);
                }
                response.Close();

                int HandleIndex = responseFromR2000.IndexOf("handle");

                string Handlegroup = responseFromR2000.Substring((HandleIndex + 9), 17);
                Var.Handle = Handlegroup.Substring(0, (Handlegroup.IndexOf('"')));

                Console.WriteLine($"UDP Handle = {Var.Handle}\r\n");

                Var.connection = true;
            }
            catch
            {
                Console.WriteLine("Failed to Establish Communication");
                Var.connection = false;
            }
        }


        public void getudpsocket()
        {
            try
            {
                Var.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Console.WriteLine("Establishing UDP Connection to " + Var.IPaddress + " at port " + Var.Port);
                Var.Socket.Connect(IPAddress.Parse(Var.IPaddress), Var.Port);
                Console.WriteLine("Connection established\r\n");
                Var.connection = true;
            }
            catch
            {
                Console.WriteLine("Failed to establish UDP socket");
                Var.connection = false;
            }

        }
    }
}