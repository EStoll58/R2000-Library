
using System;
using System.IO;

// This is the first program that needs to be run! 
// The information stored in Config.txt is pulled into the program and stored in Var.cs (The global variables file). 
// ******notes on adding global variables*******

namespace R2000_Library
{
    public class Configuration
    {
        public void config()
        {   
            // There might be problems reading were the Config.txt file is. please see both options below. 
            //string text = System.IO.File.ReadAllText(Environment.CurrentDirectory + @"\Config.txt"); // this is were the file need to be to run, but different software sometimes has problems. 

            string text = System.IO.File.ReadAllText(Environment.CurrentDirectory + @"\..\..\Config.txt"); //When switching to Visual Studio, I had to go up 3 files for the debug to work, or/and you need to add the file location to were your program operates from.

            // if there are any new elements added to config.txt, they need to be added below in the exact spelling. Then store the value read in Var.***** below. 
            string[] stringcharacteristics = { "IPaddress", "SamplesPerScan", "ScanDirection", "ScanFrequency", "FilterType", "FilterWidth", "ScanDataType", "ScanStartAngle", "ScanFieldAngle", "MaxRange", "HMIDisplayMode", "HMIDisplayText1", "HMIDisplayText2","SkipScans","Watchdog","WatchdogTimeout"};

            string[] stringvariables = new string[stringcharacteristics.GetLength(0)];

            //Finding Number of characters in each stringcharacteristic, so that we can then offset by this amount and get to the data
            int[] stringlength = new int[stringcharacteristics.GetLength(0)];
            for (int a = 0; a < stringcharacteristics.GetLength(0); a++)
            {
                string name = stringcharacteristics[a];
                stringlength[a] = name.Length + 1;
            }

            //Getting the data out and storing it in stringvariables
            Console.WriteLine("Reading Config.txt file");
            try
            {
                for (int a = 0; a < stringcharacteristics.GetLength(0); a++)
                {
                    int num = text.IndexOf(stringcharacteristics[a]);
                    //Console.WriteLine("int num = " + num);
                    string substring = text.Substring(num + stringlength[a], 40);
                    //Console.WriteLine("substring = " + substring);
                    stringvariables[a] = substring.Substring(0, (substring.IndexOf(';')));
                    Console.WriteLine(stringcharacteristics[a] + " = " + stringvariables[a]);
                }
            }
            catch
            {
                Console.WriteLine("Config file error, do not change the settings name, only the value!!!");
                return;
            }

            // after adding a new parameter/element, it needs to be manually entered in a Var.*****, which then needs to be added to Var.cs
            // the array placement directly coresponds to the posistion in stringcharacteristics[]
            try
            {

                Var.IPaddress = stringvariables[0];
                Var.SamplesPerScan = Convert.ToInt32(stringvariables[1]);
                Var.ScanDirection = stringvariables[2];
                Var.ScanFrequency = Convert.ToInt32(stringvariables[3]);
                Var.FilterType = stringvariables[4];
                Var.FilterWidth = Convert.ToInt32(stringvariables[5]);
                Var.ScanDataType = stringvariables[6];
                Var.ScanStartAngle = (Convert.ToInt32(stringvariables[7]) * 10000);
                Var.ScanFieldAngle = Convert.ToInt32(stringvariables[8]);
                Var.maxrange = Convert.ToInt32(stringvariables[9]);
                if (Var.FilterType == "none")
                {
                    Var.maxnumpointsscan = Math.Round((Var.SamplesPerScan / 360) * Convert.ToInt32(stringvariables[8]));
                }
                else
                {
                    Var.maxnumpointsscan = Math.Round((Var.SamplesPerScan / 360) * Convert.ToInt32(stringvariables[8]) / Var.FilterWidth);
                }

                Var.HMIDisplayMode = stringvariables[10];
                Var.HMIDisplayText1 = stringvariables[11];
                Var.HMIDisplayText2 = stringvariables[12];
                Var.SkipScans = Convert.ToInt32(stringvariables[13]);
                Var.Watchdog = stringvariables[14];
                Var.WatchdogTimout = Convert.ToInt32(stringvariables[15]);

                Console.WriteLine("Success!\r\n");
            }
            catch
            {
                Console.WriteLine("Error in the Config File, make sure you are using valid values, please reveiw and try agian");
                return;
            }
        }

    }
}