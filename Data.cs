using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace R2000_Library
{
    public class Data
    {
        bool allpackets = false;
        bool magicok = true;

        public void tcprecieve(int size)
        {
            // A simple tcp recieve structure, if needed the buffer and timeout can be adjusted
            Var.Socket.ReceiveTimeout = 3000;
            Var.Socket.ReceiveBufferSize = 150000; // At 25200 data points and 360 degree field of veiw, 106576 bytes are sent per scan.  
            Var.data = new byte[size];
            try
            {
                Var.Socket.Receive(Var.data);
                //Console.WriteLine("\r\nVar.data = \r\n" + string.Join(" ",Var.data));   
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0} Error code: {1}.", e.Message, e.ErrorCode);
            }
        }

        public void udprecieve()
        {
            // Currently not working. Needs fixed. 
            UdpClient receivingUdpClient = new UdpClient(Var.Port);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(Var.IPaddress), Var.Port);
            Console.WriteLine("Remote IP End Point = " + RemoteIpEndPoint);

            try
            {
                Var.data = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                Console.WriteLine(string.Join(" ", Var.data));
            }
            catch
            {
                Console.WriteLine("udp recieve failure");
            }
        }

        public void initialize()
        {

            /******************************
            This needs to be run if bulkdatatcp(); is going to be used.
            Accepts packets individually, reads and catagorizes information about each. 
            Only accepts 1 full scan worth of packets. 

            Also calculates and stores the angular value of each data point in Var.angulardata

            Important variables from here are:

            Var.packetamount = the number of packets in a single scan, this currently can range from 1 to 76
            Var.byteamount = the number of bytes in an entire scan
            Var.packetsize[] = list of the packet sizes stored in an array
            Var.headersize = the size of the header for each packet
            Var.numscanpoints = how many points are in a full scan
            Var.angulardata[] = list of corresponding angles to scan data

            *******************************/

            Var.numscanpoints = 0;
            int magic;
            int byteamount = 0;
            Var.packetamount = 1;
            int packetcount = 1;


            tcprecieve(2);
            magic = (Var.data[0] + (Var.data[1] << 8));
            //Console.WriteLine("magic = " + magic);

            if (magic == 41564)
            {
                Thread.Sleep(250); // this is to verify that the data will be there. 
                tcprecieve(6);
                int packet_type = (Var.data[0] + (Var.data[1] << 8));

                int packet_size = ((Var.data[2]) + ((Var.data[3]) << 8) + ((Var.data[4]) << 16) + ((Var.data[5]) << 24));

                tcprecieve(packet_size - 8);
                byte[] payload = Var.data;

                int payload_offset = (payload[0] + (payload[1] << 8));
                int scan_number = (payload[2] + (payload[3] << 8));
                int packet_number = (payload[4] + (payload[5] << 8));
                int scan_points = (payload[30] + (payload[31] << 8));
                int numpoints_packet = (payload[32] + (payload[33] << 8));
                int first_index = (payload[34] + (payload[35] << 8));
                int first_angle = (payload[36] + (payload[37] << 8) + (payload[38] << 16) + (payload[39] << 24));
                int angle_increment = (payload[40] + (payload[41] << 8) + (payload[42] << 16) + (payload[43] << 24));

                // Console.WriteLine("packet_type = " + packet_type);
                // Console.WriteLine("packet_size = " + packet_size);
                // Console.WriteLine("payload_offset = " + payload_offset);
                // Console.WriteLine("scan_number = " + scan_number);
                // Console.WriteLine("packet_number = " + packet_number);
                // Console.WriteLine("scan_points = " + scan_points);
                // Console.WriteLine("numpoints_packet = " + numpoints_packet);
                // Console.WriteLine("first_index = " + first_index);
                // Console.WriteLine("first_angle = " + first_angle);
                // Console.WriteLine("angle_increment = " + angle_increment);

                Var.headersize = payload_offset;

                Var.packetamount = (int)(Decimal.Ceiling((decimal)scan_points / (decimal)numpoints_packet));
                //Console.WriteLine("packet amount = " + Var.packetamount);

                packetcount = Var.packetamount;

                Var.packetsize = new int[Var.packetamount];
                Var.packetsize[0] = packet_size;

                byteamount = byteamount + packet_size;
                //Console.WriteLine("byteamount = " + byteamount);

                Var.numscanpoints = numpoints_packet;
                //Console.WriteLine("Scan points = " + Var.numscanpoints);

                Console.WriteLine(" \r\n ");
            }
            else
            {
                Console.WriteLine("First iteration Error: Magic != 41564");
                return;
            }

            if (packetcount > 1)
            {
                for (int i = 0; i < packetcount - 1; i++)
                {
                    tcprecieve(2);
                    magic = (Var.data[0] + (Var.data[1] << 8));
                    //Console.WriteLine("magic = " + magic);

                    if (magic == 41564)
                    {
                        tcprecieve(6);
                        int packet_type = (Var.data[0] + (Var.data[1] << 8));

                        int packet_size = ((Var.data[2]) + ((Var.data[3]) << 8) + ((Var.data[4]) << 16) + ((Var.data[5]) << 24));

                        tcprecieve(packet_size - 8);
                        byte[] payload = Var.data;

                        int payload_offset = (payload[0] + (payload[1] << 8));
                        int scan_number = (payload[2] + (payload[3] << 8));
                        int packet_number = (payload[4] + (payload[5] << 8));
                        int scan_points = (payload[30] + (payload[31] << 8));
                        int numpoints_packet = (payload[32] + (payload[33] << 8));
                        int first_index = (payload[34] + (payload[35] << 8));
                        int first_angle = (payload[36] + (payload[37] << 8) + (payload[38] << 16) + (payload[39] << 24));
                        int angle_increment = (payload[40] + (payload[41] << 8) + (payload[42] << 16) + (payload[43] << 24));

                        // Console.WriteLine("packet_type = " + packet_type);
                        // Console.WriteLine("packet_size = " + packet_size);
                        // Console.WriteLine("payload_offset = " + payload_offset);
                        // Console.WriteLine("scan_number = " + scan_number);
                        // Console.WriteLine("packet_number = " + packet_number);
                        // Console.WriteLine("scan_points = " + scan_points);
                        // Console.WriteLine("numpoints_packet = " + numpoints_packet);
                        // Console.WriteLine("first_index = " + first_index);
                        // Console.WriteLine("first_angle = " + first_angle);
                        // Console.WriteLine("angle_increment = " + angle_increment);

                        byteamount = byteamount + packet_size;
                        //Console.WriteLine("byteamount = " + byteamount);
                        //Console.WriteLine(" \r\n ");

                        Var.numscanpoints = Var.numscanpoints + numpoints_packet;
                        //Console.WriteLine("Scan points = " + Var.numscanpoints);

                        Var.packetsize[i + 1] = packet_size;
                        Var.rawdata = new Byte[Var.data.Length];
                        Buffer.BlockCopy(Var.data, 0, Var.rawdata, 0, Var.data.Length);

                    }
                    else
                    {
                        Console.WriteLine("\r\nVar.data = \r\n" + string.Join(" ", Var.data));
                        Console.WriteLine("Raw Data =\r\n " + string.Join(" ", Var.rawdata));
                        Console.WriteLine("Magic was not found, system exit");
                        return;
                    }
                }
            }
            else
            {
                //Console.WriteLine("there was only 1 packet");
            }
            Var.byteamount = byteamount;
            //Console.WriteLine("Scan points = " + Var.numscanpoints);

            //Initializing angular increment array
            Var.angulardata = new decimal[Var.numscanpoints];

            decimal degrees = Math.Round(decimal.Divide(Convert.ToInt32(Var.ScanFieldAngle), Convert.ToInt32(Var.numscanpoints)), 6);
            //Console.WriteLine("degrees = " + degrees);
            //Console.WriteLine("scanfieldangle = " + Var.ScanFieldAngle);
            //Console.WriteLine("numscanpoints = " + Var.numscanpoints);
            if (Var.FilterType == "none")
            {
                if (Var.ScanDirection == "ccw")
                {
                    for (int i = 0; i < Var.numscanpoints; i++)
                    {
                        Var.angulardata[i] = ((Var.ScanStartAngle / 10000) + (i * degrees));
                    }
                }
                else
                {
                    for (int i = 0; i < Var.numscanpoints; i++)
                    {
                        Var.angulardata[i] = ((Var.ScanStartAngle / 10000) - (i * degrees));
                    }
                }
            }
            else
            {
                if (Var.ScanDirection == "ccw")
                {
                    for (int i = 0; i < Var.numscanpoints; i++)
                    {
                        Var.angulardata[i] = ((Var.ScanStartAngle / 10000) + (i * degrees) + (degrees / 2));
                    }
                }
                else
                {
                    for (int i = 0; i < Var.numscanpoints; i++)
                    {
                        Var.angulardata[i] = ((Var.ScanStartAngle / 10000) - (i * degrees) - (degrees / 2));
                    }
                }
            }

            //Console.WriteLine("Angular Data =\r\n " + string.Join(" ", Var.angulardata));
        }


        public void bulkdatatcp()
        {
            /**************************** 
            This is an optimized data recieve/store/check/convert program

            To increase speed and efficiency, all packets/bytes are accepted at once. This byte amount is calculated in initialize()
            Typically the software can execute and run faster than the hardware. This causes errors because full scans have not been sent yet creating zeros in the data.
            To fix this problem packetcheck() verifies if all packets have been sent for a full scan.
            If not complete, the program will attempt to recieve the remaining bytes of information to complete a full scan.
            After all data from a single scan have been accepted, a magiccheck() is executed to make sure all packets are in the correct place.
            If all conditions are satisfied then the data is stripped of all headers, placing the distance information in Var.rawmeasurement.
            Var.rawmeasurementdata (which is still in byte form) is converted into an integer array Var.measurementdata

            *****************************/
            allpackets = false;
            try
            {
                tcprecieve(Var.byteamount);
                Var.rawdata = new byte[Var.byteamount];
                Buffer.BlockCopy(Var.data, 0, Var.rawdata, 0, Var.byteamount);
                //Console.WriteLine("Raw Data =\r\n " + string.Join(" ",Var.rawdata));
            }
            // This will recieve the amount of bytes in a single scan and store it in the variable rawdata
            // I am using Buffer.BlockCopy because it is aimed at fast byte-level primitive array copying, array.copy could be used as well but it is slower
            catch
            {
                Console.WriteLine("bulk data recieve failed, rawdata problem");
            }

            // Search Var.rawdata to see if all packets and inforamtion is there.
            // If no information, accept remaining packets with new tcprecieve length on Var.data, then add to exsisting Var.rawdata, then try again.
            // If all packets there, allpackets == true. 

            packetcheck();
            while (allpackets == false && magicok == true)
            {
                int remainingpackets = Var.packetamount - Var.x;
                //Console.WriteLine("Remaining Packets = " + remainingpackets);

                // need to find out how many bytes are int the packets remaining
                Var.remainingbytes = 0;
                for (int i = 0; i < remainingpackets; i++)
                {
                    Var.remainingbytes = Var.packetsize[Var.x + i] + Var.remainingbytes;
                }
                //Console.WriteLine("Remaining bytes = " + Var.remainingbytes);
                tcprecieve(Var.remainingbytes);

                // new data is in Var.data, need to add to Var.rawdata
                // need to know how much data is in Var.rawdata
                int rawdataamount = Var.byteamount - Var.remainingbytes;
                Buffer.BlockCopy(Var.data, 0, Var.rawdata, rawdataamount, Var.remainingbytes);
                //Console.WriteLine("\r\nNew Var.rawdata = \r\n" + string.Join(" ",Var.rawdata));

                // then read data and try to run packetcheck() again
                packetcheck();

            }

            if (allpackets == true && magicok == true)
            {
                try
                {
                    magiccheck();
                    int location = 0;
                    int offset = 0;
                    byte[] rawmeasurmentdata = new byte[Var.byteamount - (Var.packetamount * Var.headersize)];

                    if (magicok == true)
                    {
                        //Console.WriteLine("All good!!");
                        //Console.WriteLine("Last packet number was:" + Var.lastpacketnumber + "\r\n");
                        for (int i = 0; i < Var.packetamount; i++)
                        {
                            if (i == 0)
                            {
                                offset = Var.headersize;
                            }
                            else
                            {
                                offset = Var.packetsize[i - 1] + offset;
                            }
                            //Console.WriteLine("\r\ni = " + i );
                            //Console.WriteLine("offset = " + offset);
                            Buffer.BlockCopy(Var.rawdata, offset, rawmeasurmentdata, location, Var.packetsize[i] - Var.headersize);
                            location = location + Var.packetsize[i] - Var.headersize;
                        }
                        //Console.WriteLine("Raw Measurement Data =\r\n " + string.Join(" ",rawmeasurmentdata));

                        // Now the data needs to be converted into usable data.
                        Var.measurmentdata = new int[Var.numscanpoints];
                        //Console.WriteLine("numscanpoints = " + Var.numscanpoints);

                        for (int i = 0; i < (Var.numscanpoints); i++)
                        {
                            Var.measurmentdata[i] = ((rawmeasurmentdata[i * 4]) + (rawmeasurmentdata[(i * 4) + 1] << 8) + (rawmeasurmentdata[(i * 4) + 2] << 16) + (rawmeasurmentdata[(i * 4) + 3] << 24));
                        }
                        //Console.WriteLine("Measurement Data =\r\n " + string.Join(" ",Var.measurmentdata));
                        //Measurement data stored in Var.measurementdata


                    }
                    else
                    {
                        magicfinder();
                    }
                }
                // Stripping the headers out of all the data and storing it as Var.measurmentdata
                catch
                {
                    Console.WriteLine("bulk data recieve failed, measurement data problem");
                }
            }
        }


        public void background()
        {
            //Takes the average of 3 scans for the background
            //Stores data in Var.background
            bulkdatatcp(); //Initializing some values and parameters that are needed for this class

            int[] background1 = new int[Var.measurmentdata.Length];
            int[] background2 = new int[Var.measurmentdata.Length];
            int[] background3 = new int[Var.measurmentdata.Length];
            Var.background = new int[Var.measurmentdata.Length];

            bulkdatatcp();
            for (int i = 0; i < Var.measurmentdata.Length; i++)
            {
                if (Var.measurmentdata[i] == -1 || Var.measurmentdata[i] > Var.maxrange)
                {
                    background1[i] = Var.maxrange;
                }
                else
                {
                    background1[i] = Var.measurmentdata[i];
                }
            }

            bulkdatatcp();
            for (int i = 0; i < Var.measurmentdata.Length; i++)
            {
                if (Var.measurmentdata[i] == -1 || Var.measurmentdata[i] > Var.maxrange)
                {
                    background2[i] = Var.maxrange;
                }
                else
                {
                    background2[i] = Var.measurmentdata[i];
                }
            }

            bulkdatatcp();
            for (int i = 0; i < Var.measurmentdata.Length; i++)
            {
                if (Var.measurmentdata[i] == -1 || Var.measurmentdata[i] > Var.maxrange)
                {
                    background3[i] = Var.maxrange;
                }
                else
                {
                    background3[i] = Var.measurmentdata[i];
                }
            }

            for (int i = 0; i < Var.measurmentdata.Length; i++)
            {
                Var.background[i] = ((background1[i] + background2[i] + background3[i]) / 3);
            }

            Console.WriteLine("Background = \r\n" + string.Join(" ", Var.background));
        }

        public void packetcheck()
        {
            // The program check the first 4 bytes of each packet (where it is supposed to be) to verify each packet is there
            int offset = 0;
            for (int i = 0; i < Var.packetamount; i++)
            {
                if (i == 0)
                {
                    offset = 0;
                }
                else
                {
                    offset = Var.packetsize[i - 1] + offset;
                }
                int value1 = Var.rawdata[offset];
                int value2 = Var.rawdata[offset + 1];
                int value3 = Var.rawdata[offset + 2];
                int value4 = Var.rawdata[offset + 3];
                if (value1 == 0 && value2 == 0 && value3 == 0 && value4 == 0)
                {
                    allpackets = false;
                    //Console.WriteLine("all packets = false");
                    Var.x = i;

                    break;
                }
                else
                {
                    Var.lastpacketnumber = (Var.rawdata[offset + 12] + (Var.rawdata[offset + 13] << 8));
                    //Console.WriteLine("lastpacketnumber = " + Var.lastpacketnumber);

                    allpackets = true;
                    //Console.WriteLine("all packets = true");
                }
            }
        }

        public void magiccheck()
        {
            // Verifies that all packets begin with magic
            for (int i = 0; i < Var.packetamount; i++)
            {
                int offset = 0;
                if (i == 0)
                {
                    offset = 0;
                }
                else
                {
                    offset = Var.packetsize[i - 1] + offset;
                }
                int value1 = Var.rawdata[offset];
                int value2 = Var.rawdata[offset + 1];
                if (((value1 != 0 || value2 != 0) && (value1 != 92 || value2 != 162)) || (Var.lastpacketnumber != Var.packetamount))
                {

                    magicok = false;
                    Console.WriteLine("Buffer is full and causing data error. Resetting data stream.\r\n");
                    //Console.WriteLine("magicok = false");
                    //Console.WriteLine("Last packet number was:" + Var.lastpacketnumber + "\r\n");
                    break;
                }
                else
                {
                    magicok = true;
                    //Console.WriteLine("magicok = true");
                }
            }
        }

        public void magicfinder()
        {
            // If data error, scan is stopped, buffer is cleared, then scan begins agian. 
            try
            {
                int a = 10000;
                var command = new Command();
                command.stopstream();
                bool good = false;
                while (good == false)
                {
                    int c = a - 100;
                    tcprecieve(a);
                    for (int d = a; d >= c; d--)
                    {
                        int b = Var.data[d - 1];
                        if (b == 0)
                        {
                            if (d == c)
                            {
                                //Console.WriteLine("Buffer empty, Starting new stream\r\n");
                                good = true;
                                magicok = true;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("Buffer not empty, trying again.");
                            good = false;
                            break;

                        }
                    }
                }
                //Console.WriteLine("\r\nAll Buffer data, Var.data = \r\n" + string.Join(" ",Var.data)); 
                command.startstream();
            }
            catch
            {
                Console.WriteLine("Magic Finder Error");
            }

        }


    }
}