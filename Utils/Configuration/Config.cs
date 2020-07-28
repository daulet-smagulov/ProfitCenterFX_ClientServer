using System.Net;

namespace Utils
{
    public class Config
    {
        public IPAddress IPAddress { get; }
        public int Port { get; }
        public double MinValue { get; }
        public double MaxValue { get; }

        public Config(string ipAddress, int port, double minValue, double maxValue)
        {
            IPAddress = IPAddress.Parse(ipAddress);
            Port = port;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
