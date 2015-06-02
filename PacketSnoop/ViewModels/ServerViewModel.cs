using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Windows;

using Reactive.Bindings;

using PacketSnoop.Models;

namespace PacketSnoop.ViewModels
{
    public class ServerViewModel
    {
        public ServerViewModel()
        {
            const int defaultPort = 11000;

            IsRunning.Where(isRunning => isRunning)
                .Subscribe(_ => ReceivePackets());

            var anyPackets = Packets.ToCollectionChanged()
                .Select(_ => Packets.Count > 0);

            ClearPackets = anyPackets.CombineLatest(IsRunning, (packets, isRunning) => packets && !isRunning)
                .ToReactiveCommand(false);
            ClearPackets.Subscribe(_ => Packets.Clear());

            Port = new ReactiveProperty<string>(defaultPort.ToString()).SetValidateAttribute(() => Port);
            ValidationErrors = Port.ObserveErrorChanged
                .Select(errors => errors == null ? null : errors.OfType<string>().FirstOrDefault())
                .ToReactiveProperty();

            ToggleRunning = Port.ObserveHasErrors.Select(hasErrors => !hasErrors).ToReactiveCommand();
            ToggleRunning.Subscribe(_ => IsRunning.Value = !IsRunning.Value);
        }

        public ObservableCollection<Packet> Packets { get; } = new ObservableCollection<Packet>();

        public ReactiveProperty<bool> IsRunning { get; } = new ReactiveProperty<bool>();

        [Range(1, 65535, ErrorMessage = "Range 1 .. 65535")]
        [Required(ErrorMessage = "Port is required")]
        public ReactiveProperty<string> Port { get; }

        public ReactiveProperty<string> ValidationErrors { get; }

        public ReactiveCommand ToggleRunning { get; }

        public ReactiveCommand ClearPackets { get; }

        private void ReceivePackets()
        {
            var packets = Observable.Using(
                () => new UdpClient(int.Parse(Port.Value)),
                client => Observable.FromAsync(client.ReceiveAsync).Repeat());
            packets.TakeUntil(IsRunning.Where(isRunning => !isRunning))
                 .Timestamp()
                 .Select(packet => new Packet(packet))
                 .ObserveOnDispatcher()
                 .Subscribe(Packets.Add, OnError);
        }

        private void OnError(Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            IsRunning.Value = false;
        }
    }
}
