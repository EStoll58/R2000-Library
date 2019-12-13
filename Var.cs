using System;

using System.Net.Sockets;
// Global variables stored by the program are located here. 
namespace R2000_Library
{
    public static class Var
    {   
        // Values read from Config.txt
        public static string IPaddress; // IP address of R2000
        public static double SamplesPerScan; //The scan resolution in 360 degrees; Valid values are 25200,16800,12600,10080,8400,7200,6300,5600,5040,4200,3600,3150,2800,2520,2400,2100,1800,1680,1440,1200,900,800,720,600,480,450,400,360       
        public static string ScanDirection; //Direction of R2000 spin; cw or ccw        
        public static int ScanFrequency; //R2000 spin frequency;integer values from 10-100hz, depending on model      
        public static string FilterType; //Setting the filter type; none, average, median, maximum, remission
        public static int FilterWidth; //How many points are grouped together for the filter; Valid Values are 2,4,8,16 
        public static string HMIDisplayMode; //stores what type of display you wish to see
        public static string HMIDisplayText1; //top line text
        public static string HMIDisplayText2; //bottom line text

        public static string ScanDataType; //Scan packet type that is being sent by R2000; A (Distance only), B (Distance and amplitude), C (Distance and amplitude compact) 
        public static int ScanStartAngle; //Starting angle location; Valid values are -180 to +180
        public static int ScanFieldAngle; //field of view or sector of data you wish to recieve       
        public static int SkipScans;
        public static string Watchdog;
        public static int WatchdogTimout;
        public static int maxrange; //used in data.background(); to create a background suppression feature

        // Retrieved information from R2000
        public static int Port;
        public static string Handle;
        public static Socket Socket;

        // Calculated values
        public static double maxnumpointsscan; // This value is calculated out from ScanFieldAngle, then it is sent to the R2000.  
        public static int byteamount;
        public static int headersize;
        public static int packetamount;
        public static int[] packetsize;
        public static byte[] rawdata;
        public static byte[] rawmeasurmentdata;
        public static bool connection;
        public static byte[] data;
        public static int remainingbytes;
        public static int x;
        public static int lastpacketnumber;
        public static string responseFromR2000;
        public static int numscanpoints;
        public static decimal[] angulardata;
        public static int[] measurmentdata;
        public static int[] background;
        





    }
}
