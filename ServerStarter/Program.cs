using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStarter
{
    class Program
    {
        private static Process myProcess = new Process();
        static void Main(string[] args)
        {
            NamedPipeServerStream Pipe = new NamedPipeServerStream("Interconnect", PipeDirection.In);
            StreamReader sr = new StreamReader(Pipe);
            string data;
            while (true)
            {
                if (Pipe.IsConnected == true)
                {
                    try
                    {
                        data = sr.ReadLine();
                        if (data == "Start") 
                        {
                            StartACCServer();
                        }
                        if (data == "Stop")
                        {
                            StopACCServer();
                        }
                        Pipe.Disconnect();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Waiting for connection...");
                    Pipe.WaitForConnection();
                    Console.WriteLine("Client connected!");
                    Console.WriteLine("Waiting for orders...");
                }
            }
        }
        static void StartACCServer()
        {
            Console.WriteLine("Starting ACC server...");
            myProcess.StartInfo.FileName = @"C:\Program Files (x86)\Steam\steamapps\common\Assetto Corsa Competizione Dedicated Server\server\accServer.exe";
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.WorkingDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\Assetto Corsa Competizione Dedicated Server\server";
            myProcess.StartInfo.CreateNoWindow = false;
            myProcess.Start();
        }
        static void StopACCServer() 
        {
            Console.WriteLine("Stopping ACC server...");
            try
            {
                myProcess.CloseMainWindow();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
}
