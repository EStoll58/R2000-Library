using System;

using System.Net.Sockets;
// Global variables stored by the program are located here. 
namespace R2000_Library
{
    public static class Var
    {
        //IP Address of the R2000
        public static string IPaddress;

        //The scan resolution in 360 degrees; Valid values are 25200,16800,12600,10080,8400,7200,6300,5600,5040,4200,3600,3150,2800,2520,2400,2100,1800,1680,1440,1200,900,800,720,600,480,450,400,360
        public static double SamplesPerScan;
        //Direction of R2000 spin; cw or ccw
        public static string ScanDirection;
        //R2000 spin frequency;integer values from 10-100hz, depending on model
        public static int ScanFrequency;
        //Setting the filter type; none, average, median, maximum, remission
        public static string FilterType;
        //How many points are grouped together for the filter; Valid Values are 2,4,8,16
        public static int FilterWidth;
        //Scan packet type that is being sent by R2000; A (Distance only), B (Distance and amplitude), C (Distance and amplitude compact)
        public static string ScanDataType;
        //Starting angle location; Valid values are -180 to +180
        public static int ScanStartAngle;
        public static int ScanFieldAngle;
        public static int byteamount;
        public static int headersize;
        public static int packetamount;
        public static int[] packetsize;
        public static byte[] rawdata;
        public static byte[] rawmeasurmentdata;

        public static double maxnumpointsscan;
        public static int Port;
        public static string Handle;
        public static Socket Socket;
        public static bool connection;
        public static byte[] data;

        public static string HMIDisplayType;
        public static string HMIDisplayText1;
        public static string HMIDisplayText2;

        public static int remainingbytes;
        public static int x;
        public static int lastpacketnumber;
        public static string responseFromR2000;
        public static int numscanpoints;
        public static decimal[] angulardata;
        public static int[] measurmentdata;
        public static int[] background;
        public static int maxrange;





    }
}
