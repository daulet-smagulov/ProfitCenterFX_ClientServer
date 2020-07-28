using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using UDPBase;
using Utils;
using Utils.DataAnalysis;

namespace UDPServer
{
    class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //Read config file
            Config config = ConfigParser.Parse("Config.xml", log);

            //Create values generator
            var valuesGenerator = new ValuesGenerator(config.MinValue, config.MaxValue, 1000);

            bool exit = false;
            Task.Factory.StartNew(() =>
            {
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
                exit = true;
            });

            //create a new server
            UdpBase server = new UdpBase("localhost", config.Port);

            log.Info("Start generating numbers");
            Console.WriteLine("Start generating numbers");

            while (!exit)
            {
                double value = valuesGenerator.Next();
                Console.WriteLine(value);
                log.Trace("Generated number: " + value);
                server.Send(value.ToString(), config.Port, log);
                //Thread.Sleep(500);
            }

            log.Info("Stopped");
            Console.WriteLine("Server stopped");
            server.Send("Server stopped", config.Port, log);
        }
    }
}
