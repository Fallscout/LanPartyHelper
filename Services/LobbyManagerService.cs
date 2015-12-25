using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanPartyUtility.Services
{

    public class LobbyManagerService : ILobbyManager
    {
        public static int PlayerCount { get; set; }
        public static ObservableCollection<Player> Players { get; set; }
        public static List<ILobbyManagerCallback> Channels { get; set; }

        static LobbyManagerService()
        {
            PlayerCount = 0;
            Players = new ObservableCollection<Player>();
            Channels = new List<ILobbyManagerCallback>();
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

            PlayerCount += 1;
            player.Id = PlayerCount;
            Players.Add(player);

            Channels.Add(OperationContext.Current.GetCallbackChannel<ILobbyManagerCallback>());

            foreach (ILobbyManagerCallback channel in Channels)
            {
                try
                {
                    channel.RefreshPlayerList(Players);
                }
                catch (CommunicationObjectAbortedException ex)
                {
                    Channels.Remove(channel);
                }
            }

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

                foreach (ILobbyManagerCallback channel in Channels)
                {
                    channel.RefreshPlayerList(Players);
                }
            }
        }
    }
}
