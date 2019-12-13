using System;
using System.IO;
using System.Net;

/*********************************************************
Command.cs stores the Http commands that you can send to the R2000

A simple command protocol using HTTP requests (and responses) is provided in order to parametrize and control the
sensor. The HTTP can be accessed using a standard web browser or by establishing temporary TCP/IP connections to
the HTTP port.

The HTTP command protocol provides a simple unified way to control sensor operation for application software. HTTP
commands are used to configure sensor measurement as well as to read and change sensor parameters.

Each HTTP command is constructed as Uniform Resource Identifier

    Structure: 
    http://<sensor IP address>/cmd/<cmd_name>?<argument1=value>&<argument2=value>

    Example: http://169.254.194.202/cmd/request_handle_tcp
    or       http://169.254.194.202/cmd/set_parameter?samples_per_scan=1200

The console feedback can be disabled by commenting out the Console.WriteLine(s) that are in each class

For more details, please see the R2000 manual https://files.pepperl-fuchs.com/webcat/navi/productInfo/doct/doct3469e.pdf?v=20190712154957

 *********************************************************/
namespace R2000_Library
{
    public class Command
    {
        //Starting the data stream 
        public void startstream()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/start_scanoutput?handle=" + Var.Handle);
            Console.WriteLine("Starting data stream \r\nSending: http://" + Var.IPaddress + "/cmd/start_scanoutput?handle=" + Var.Handle);
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                //Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }

        //Stopping data stream 
        public void stopstream()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/stop_scanoutput?handle=" + Var.Handle);
            Console.WriteLine("Stopping data stream \r\nSending: http://" + Var.IPaddress + "/cmd/stop_scanoutput?handle=" + Var.Handle);
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                //Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }

        //Feeding the watchdog, this needs done ever 60 seconds to keep the connection from closing
        public void watchdog()
        {
            try
            {
                //Feeding the Watchdog
                WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/feed_watchdog?handle=" + Var.Handle);
                WebResponse Response = Request.GetResponse();

                //Console.WriteLine("Feeding Watchdog \r\nSending: http://" + Var.IPaddress + "/cmd/feed_watchdog?handle=" + Var.Handle);
                using (Stream dataStream = Response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    Var.responseFromR2000 = reader.ReadToEnd();
                    //Console.WriteLine("Response: \r\n"+Var.responseFromR2000);
                }
                errorcheck();
            }
            catch
            {
                Console.WriteLine("Watchdog failure!!!\r\n");
            }
        }

        //Terminating the handle if you didn't need to keep it open, handle will close automatically after 60 seconds if watchdog is not fed.
        public void handlerelease()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/release_handle?handle=" + Var.Handle);
            Console.WriteLine("Releasing handle \r\nSending: http://" + Var.IPaddress + "/cmd/release_handle?handle=" + Var.Handle);
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                //Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }

        //All parameters from Config.txt are sent to the R2000
        public void setparameters()
        {
            Console.WriteLine("Setting Parameters");
            // Parameter setup using the set_parameter function.
            string[] setparameterlist = { "samples_per_scan", "scan_direction", "scan_frequency", "filter_type", "filter_width", "hmi_display_mode", "hmi_static_text_1", "hmi_static_text_2" };
            object[] varparameterlist = { Var.SamplesPerScan, Var.ScanDirection, Var.ScanFrequency, Var.FilterType, Var.FilterWidth, Var.HMIDisplayMode, Var.HMIDisplayText1, Var.HMIDisplayText2 };
            for (int a = 0; a < setparameterlist.GetLength(0); a++)
            {
                WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/set_parameter?" + setparameterlist[a] + "=" + varparameterlist[a]);
                Console.WriteLine("Setting " + setparameterlist[a] + " = " + varparameterlist[a] + "\r\nSending: http://" + Var.IPaddress + "/cmd/set_parameter?" + setparameterlist[a] + "=" + varparameterlist[a]);
                WebResponse Response = Request.GetResponse();
                using (Stream dataStream = Response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    Var.responseFromR2000 = reader.ReadToEnd();
                    //Console.WriteLine("Response: \r\n"+Var.responseFromR2000);
                }
                errorcheck();
            }
            Console.WriteLine("\r\n");
        }
        public void setscanoutputconfig()
        {
            Console.WriteLine("Setting Scan Output Config");
            // Parameter setup using the set_scanoutput_config function. 
            string[] setscanouputlist = { "packet_type", "start_angle", "max_num_points_scan", "skip_scans", "watchdog", "watchdogtimeout"};
            object[] varscanoutputlist = { Var.ScanDataType, Var.ScanStartAngle, Var.maxnumpointsscan, Var.SkipScans, Var.Watchdog, Var.WatchdogTimout};
            for (int a = 0; a < setscanouputlist.GetLength(0); a++)
            {
                WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/set_scanoutput_config?handle=" + Var.Handle + "&" + setscanouputlist[a] + "=" + varscanoutputlist[a]);
                Console.WriteLine("Setting " + setscanouputlist[a] + " = " + varscanoutputlist[a] + "\r\nSending: http://" + Var.IPaddress + "/cmd/set_scanoutput_config?handle=" + Var.Handle + "&" + setscanouputlist[a] + "=" + varscanoutputlist[a]);
                WebResponse Response = Request.GetResponse();
                using (Stream dataStream = Response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    Var.responseFromR2000 = reader.ReadToEnd();
                    //Console.WriteLine("Response: \r\n"+Var.responseFromR2000);
                }
                errorcheck();
            }
            Console.WriteLine("\r\n");
        }

        //Request for all current parameters
        public void getparameters()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/get_parameter");
            Console.WriteLine("Parameters Settings \r\nSending: http://" + Var.IPaddress + "/cmd/get_parameter?");
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }

        //Request for all current scan output configurations on Var.Handle
        public void getscanoutputconfig()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/get_scanoutput_config?handle=" + Var.Handle);
            Console.WriteLine("\r\nScan Output Settings \r\nSending: http://" + Var.IPaddress + "/cmd/get_scanoutput_config?handle=" + Var.Handle);
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }
        //Factory reset of the device
        public void factoryreset()
        {
            WebRequest Request = WebRequest.Create("http://" + Var.IPaddress + "/cmd/factory_reset");
            Console.WriteLine("Factory Resetting R2000 \r\nSending: http://" + Var.IPaddress + "/cmd/factory_reset");
            WebResponse Response = Request.GetResponse();
            using (Stream dataStream = Response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                Var.responseFromR2000 = reader.ReadToEnd();
                //Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
            }
            errorcheck();
        }

        public void errorcheck()
        {
            // Recognized that an error occured and exits system. 
            int num = Var.responseFromR2000.IndexOf("error_code");
            string error = Var.responseFromR2000.Substring(num + 12, 1);
            //Console.WriteLine("error code = " + error);

            if (error == "0")
            {
                //Console.WriteLine("no error to report");
            }
            else
            {
                Console.WriteLine("Response: \r\n" + Var.responseFromR2000);
                Console.WriteLine("Command not successful, exiting system");
                Environment.Exit(0);
            }

        }

    }
}

