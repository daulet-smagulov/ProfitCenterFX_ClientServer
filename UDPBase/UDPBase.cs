using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace UDPBase
{
    public class UdpBase
    {
        protected UdpClient Client;

        public UdpBase(IPAddress address, int port)
        {
            Client = new UdpClient();
            Client.Connect(address, port);
        }

        public UdpBase(string hostname, int port)
        {
            Client = new UdpClient();
            Client.Connect(hostname, port);
        }

        public UdpBase(int port, AddressFamily family)
        {
            Client = new UdpClient(port, family);
        }

        public void Send(string message, int port, ILogger log)
        {
            byte[] datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length);
        }

        //public async Task<Received> ReceiveAsync(ILogger log)
        //{
        //    var result = await Client.ReceiveAsync();
        //    string message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length);
        //    return new Received(message, result.RemoteEndPoint);
        //}

        public async Task<string> ReceiveAsync(ILogger log)
        {
            var result = await Client.ReceiveAsync();
            return Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length);
        }

        public string Receive(ILogger log)
        {
            if (Client.Available > 0)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] datagram = Client.Receive(ref ep);
                return Encoding.ASCII.GetString(datagram);
            }
            log.Error("Client is anavailable");
            return string.Empty;
        }
    }
}
