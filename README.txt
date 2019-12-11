/*********************************************************
Hello and Welcome!

The library name is R2000_Library, please put "using R2000_Library;" at the top of your code when using. 

There are 4 Classes that need to be instantiated. Data.cs, Connect.cs, Command.cs and Configuration.cs. Below in an examle. 

if running through command prompt:

    public static void Main(string[] args)
    {
        
        Data data = new Data();
        Connect connect = new Connect();
        Command command = new Command();
        Configuration configuration = new Configuration();
        .
        .
        *** Main program ***
        .
        .
    }
   

Opitons for you to call on:

configuration.config();  //***This is mandatory to run***! This class stores all information that was stored in the Config.txt file, there might be problems reading this file depending on it's location.
                         // Please see Configuration.cs (lines 15 or 17) to change the location of the config.txt file. 

connect.*******();    // This section of code houses all the classes that establish communication via TCP or UDP and sockets. connecttcp() or connectudp() must be run after config()
        
        connect.connecttcp();    // This program requests a TCP handle and port from the R2000 so you can send and recieve information
        
        connect.connectudp();    // This program requests a UDP handle. It first calls on connect.connecttcp() to get a usable port then sends a second request for a UDP handle connection 
        
        connect.gettcpsocket();  // Establishes a TCP socket with the R2000 to recieve data
        
        connect.getudpsocket();  // Establishes a UDP socket with the R2000 to recieve data

command.*******();    // This section of code houses all the classes that send Http commands to the R2000, options are listed below
        
        command.startstream();      // This program will start the data stream on the handle and socket that was earlier specified
        
        command.stopstream();       // This program will stop the data stream that was started on the handle and socket
        
        command.watchdog();         // This is a mandatory program to keep the handle from closing. Handle will automatically close in 60 seconds if watchdog isnt refreshed at least every 60 seconds. 
        
        command.handlerelease();    // This program terminates the handle (This should be used if the watchdog is turned off)
        
        command.setparameters();    // This program sends all the parameters to the R2000 that were stored into Config.txt
        
        command.getparameters();    // This program retrieves all current settings stored on the R2000
        
        command.factoryreset();     // This program restores the R2000 to factory settings

data.*******();       // This section of cod houses all the classes that handles the data
        
        data.initialize();          // THIS IS MANDITORY TO RUN IF YOU WANT DATA!!! This program counts how many packets are sent per scan and the total number of bytes recieved per scan
        
        data.bulkdata();            // This brings in all the bulk data including the headers of each packet. YOU MUST RUN data.initialize(); FIRST FOR THIS TO WORK!!!!!
        
        data.tcprecieve();          // This reads TCP data, mostly used interally and other tools use this to bring in data. You need to designate the number of bytes you wish to recieve, example: data.tcprecieve(8);
        
        data.udprecieve();          // This reads UDP data
        
        data.magicfinder();         // Work in progress, used interally if there are packet errors to find the begining of the packet again. 
        
 *********************************************************/