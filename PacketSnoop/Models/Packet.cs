using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Text;

namespace PacketSnoop.Models
{
    public class Packet
    {
        public Packet(Timestamped<UdpReceiveResult> receiveResult)
        {
            Timestamp = receiveResult.Timestamp.LocalDateTime;
            Address = receiveResult.Value.RemoteEndPoint.Address;
            Port = receiveResult.Value.RemoteEndPoint.Port;
            Contents = Encoding.UTF8.GetString(receiveResult.Value.Buffer);
        }

        public DateTime Timestamp { get; }

        public IPAddress Address { get; }

        public int Port { get; }

        public string Contents { get; }
    }
}
