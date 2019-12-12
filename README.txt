/*********************************************************
{Under constant development and updates}

Hello and Welcome!

The library name is R2000_Library, please put "using R2000_Library;" at the top of your code when using. 

Please take note of the Config.txt file. This is were all parameters are set for the R2000. Please see configuration.config(); below to read this file. 

There are 4 Classes that need to be instantiated. Data.cs, Connect.cs, Command.cs and Configuration.cs. Below in an examle. 

if running through command prompt:

    public static void Main(string[] args)
    {
        Configuration configuration = new Configuration();
        Connect connect = new Connect();
        Command command = new Command();
        Data data = new Data();
        .
        .
        *** Main program ***
        .
        .
    }
   

Opitons for you to call on: (there may be more options for each class but some are under construction and others might be for interal use)

configuration.config();  //***This is mandatory to run***! This class stores all information that was stored in the Config.txt file, there might be problems reading this file depending on it's location.
                         // Please see Configuration.cs (lines 15 or 17) to change the location of the config.txt file. 

connect.*******();    // This class houses all the classes that establish communication via TCP or UDP and sockets. connecttcp() or connectudp() must be run after config()
        
        connect.connecttcp();    // This program requests a TCP handle and port from the R2000 so you can send and recieve information
        
        connect.gettcpsocket();  // Establishes a TCP socket with the R2000 to recieve data 

command.*******();    // This class houses all the classes that send Http commands to the R2000, options are listed below
        
        command.startstream();          // This program will start the data stream on the handle and socket that was earlier specified
        
        command.stopstream();           // This program will stop the data stream that was started on the handle and socket
        
        command.watchdog();             // This is a mandatory program to keep the handle from closing. Handle will automatically close in 60 seconds if watchdog isnt refreshed at least every 60 seconds. 
        
        command.handlerelease();        // This program terminates the handle (This should be used if the watchdog is turned off)
        
        command.setparameters();        // This program sends all the parameters to the R2000 that were stored into Config.txt

        command.setscanoutputconfig();  // This program sets data configuration on set handle
        
        command.getparameters();        // This program retrieves all current parameters stored on the R2000

        command.getscanoutputconfig();  // This program retrireves all current scan output settings on handle
        
        command.factoryreset();         // This program restores the R2000 to factory settings

data.*******();       // This class houses all the classes that handles the data
        
        data.initialize();          // THIS IS MANDITORY TO RUN IF YOU WANT DATA!!! This program counts how many packets are sent per scan and the total number of bytes recieved per scan. This also configures a lot of settings.
        
        data.bulkdata();            // This brings in all the bulk data including the headers of each packet. YOU MUST RUN data.initialize(); FIRST FOR THIS TO WORK!!!!!

        data.background();          // This takes the average of 3 scans to make a background stored in Var.background[], it has been useful for different applications 
        
        data.tcprecieve();          // This reads TCP data, mostly used interally and other tools use this to bring in data. You need to designate the number of bytes you wish to recieve, example: data.tcprecieve(8);

        data.magiccheck();          // This checks the raw data from the R2000 to make sure all data is present and no errors
        
        data.magicfinder();         // Used interally if there are packet errors (usually due to buffer overload. the hardware being used isn't fast enough.). This will stop the data stream, clear the buffer, then start again. 

Var.*******();          // Global variables are stored here for easier access
            
        Please see class for list and discripiton of each variable





Recommendations for building code and structure:

configuration.config();             // This must be run at the begining of your code to read the Config.txt file
connect.connecttcp();               // Requests a TCP handle and port from R2000
connect.gettcpsocket();             // Establishes tcp socket to send data

command.setparameters();            // Sends stored parameters to R2000
command.getparameters();            // Requests list of all stored parameters

command.setscanoutputconfig();      // Sends stored scan output config settings
command.getscanoutputconfig();      // Requests list of all stored settings on handle

command.watchdog();                 // To keep handle open, send this command repeatedly at intervals <60 seconds

command.startstream();              // Initiates data transmission on perviously stored socket

data.initialize();                  // Reads and parameterize data about full scan (accepts one scan worth of packets)
data.bulkdatatcp();                 // Stores all scan data in bulk form for speed and efficiency 

command.stopstream();               // Stops the data stream when your scan is finished (should also be used in proper shutdown)
command.handlerelease();            // Manually releases handle. Handle will be lost anyway after 60 seconds or shutdown. 
        
 *********************************************************/