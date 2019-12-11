﻿/*********************************************************
Hello and Welcome!
This is the primary program that calls on all other classes
This is where you can build you program and use all the premade classes
Opitons for you to call on:
readconfig.config();  //***This is mandatory to run***! This program stores all information that was stored in the Config.txt file
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