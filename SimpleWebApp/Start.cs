
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Server init in progress....");
        AsyncServer Server = new AsyncServer();

        Console.WriteLine("Server starting....");
        Server.StartListen();

        Console.WriteLine("Server is exiting.");
    }
}