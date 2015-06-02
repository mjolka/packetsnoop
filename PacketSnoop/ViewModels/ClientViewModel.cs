using System;
using System.Net;
using System.Text;

using Reactive.Bindings;

using PacketSnoop.Models;

namespace PacketSnoop.ViewModels
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, 11000);
            var packets = new[]
            {
                new { Message = "hello", Interval = TimeSpan.FromSeconds(1) },
                new { Message = "world", Interval = TimeSpan.FromSeconds(2) }
            };
            foreach (var packet in packets)
            {
                Packets.Add(new ClientPacket(Encoding.UTF8.GetBytes(packet.Message), endPoint, packet.Interval));
            }

            new Views.Server().Show();
        }

        public ReactiveCollection<ClientPacket> Packets { get; } = new ReactiveCollection<ClientPacket>();
    }
}
