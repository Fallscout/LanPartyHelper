using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace LanPartyUtility.Services
{
    public delegate void PlayerConnectedEventHandler(object sender, LobbyManagerEventArgs args);
    public delegate void PlayerDisconnectedEventHandler(object sender, LobbyManagerEventArgs args);

    public class LobbyManagerService : ILobbyManager
    {
        public static ServiceHost Host { get; private set; }
        public static int PlayerCount { get; set; }
        public static ObservableCollection<Player> Players { get; set; }
        public static Dictionary<string, ILobbyManagerCallback> Channels { get; set; }

        public static event PlayerConnectedEventHandler PlayerConnected;
        public static event PlayerDisconnectedEventHandler PlayerDisconnected;

        static LobbyManagerService()
        {
            Host = new ServiceHost(typeof(LobbyManagerService), new Uri("net.tcp://localhost:3745/LanPartyUtility"));
            Host.AddServiceEndpoint(typeof(ILobbyManager), new NetTcpBinding(), "LobbyManagerService");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = new Uri("http://localhost:8000/LanPartyUtility");
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            Host.Description.Behaviors.Add(smb);

            Host.Closing += Host_Closing;

            PlayerCount = 0;
            Players = new ObservableCollection<Player>();
            Channels = new Dictionary<string, ILobbyManagerCallback>();
        }

        static void Host_Closing(object sender, EventArgs e)
        {
            Players.Clear();
            PlayerCount = 0;
            Channels.Clear();
        }

        protected virtual void OnPlayerConnected(LobbyManagerEventArgs args)
        {
            var handler = PlayerConnected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnPlayerDisconnected(LobbyManagerEventArgs args)
        {
            var handler = PlayerDisconnected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public int Connect(Player player)
        {
            Player pl = (from p in Players
                         where p.IPAddress == player.IPAddress
                         select p).FirstOrDefault();

            if (pl != null)
            {
                throw new InvalidOperationException(String.Format("IP-Address {0} is already in use.", player.IPAddress));
            }

            player.Id = ++PlayerCount;
            Players.Add(player);

            Channels.Add(player.IPAddress, OperationContext.Current.GetCallbackChannel<ILobbyManagerCallback>());

            foreach (var channel in Channels)
            {
                try
                {
                    channel.Value.RefreshPlayerList(Players);
                }
                catch (CommunicationObjectAbortedException)
                {
                    Channels.Remove(channel.Key);
                }
            }

            OnPlayerConnected(new LobbyManagerEventArgs(player));

            return PlayerCount;
        }

        public void Disconnect(int id)
        {
            Player player = (from p in Players
                             where p.Id == id
                             select p).FirstOrDefault();

            if (player != null)
            {
                Players.Remove(player);

                foreach (var channel in Channels)
                {
                    channel.Value.RefreshPlayerList(Players);
                }

                OnPlayerDisconnected(new LobbyManagerEventArgs(player));
            }
        }
    }
}
