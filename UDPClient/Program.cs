using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NLog;
using UDPBase;
using Utils;
using Utils.DataAnalysis;

namespace UDPClient
{
    class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            //Read config file
            Config config = ConfigParser.Parse("Config.xml", log);

            var calculator = new MomentsCalculator();

            UdpBase client = new UdpBase(config.Port, AddressFamily.InterNetwork);

            bool exit = false;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Q)
                        exit = true;
                    if (key == ConsoleKey.Enter)
                    {
                        double[] moments = calculator.GetValues();
                        Console.WriteLine("Current moments:");
                        Console.WriteLine("    Mean = " + moments[0]);
                        Console.WriteLine("    SD = " + moments[1]);
                        Console.WriteLine("    Median = " + moments[2]);
                        Console.WriteLine("    Mode = " + moments[3]);
                    }
                }
            });

            Task.Factory.StartNew(async () => {
                while (true)
                {
                    string message = await client.ReceiveAsync(log);
                    calculator.Add(Convert.ToDouble(message));
                }
            });

            while (!exit) ;
        }
    }
}
