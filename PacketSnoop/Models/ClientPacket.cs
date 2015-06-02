using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;

using Reactive.Bindings;

namespace PacketSnoop.Models
{
    public class ClientPacket
    {
        private readonly IPEndPoint _endPoint;

        public ClientPacket(byte[] datagram, IPEndPoint endPoint, TimeSpan interval)
        {
            _endPoint = endPoint;
            Datagram = datagram;
            Interval = interval;

            IsRunning.Where(x => x)
                .Subscribe(_ => SendPackets());

            ToggleRunning.Subscribe(_ => IsRunning.Value = !IsRunning.Value);
        }

        public byte[] Datagram { get; }

        public IPAddress Address => _endPoint.Address;

        public int Port => _endPoint.Port;

        public TimeSpan Interval { get; }

        public ReactiveProperty<bool> IsRunning { get; } = new ReactiveProperty<bool>();

        public ReactiveCommand ToggleRunning { get; } = new ReactiveCommand();

        private void SendPackets()
        {
            Observable.Using(
                () => new UdpClient(),
                client => Observable.Interval(Interval)
                            .TakeUntil(IsRunning.Where(isRunning => !isRunning))
                            .Select(_ => client.Send(Datagram, Datagram.Length, _endPoint)))
                .Publish()
                .Connect();
        }
    }
}
