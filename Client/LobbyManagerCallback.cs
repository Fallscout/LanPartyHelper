using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanPartyUtility.Client
{
    public class LobbyManagerCallback : ILobbyManagerCallback
    {
        public static ObservableCollection<Player> Players { get; set; }
        private static SynchronizationContext Context { get; set; }

        static LobbyManagerCallback()
        {
            Players = new ObservableCollection<Player>();
            Context = SynchronizationContext.Current;
        }

        public void RefreshPlayerList(Player[] players)
        {
            Context.Post(new SendOrPostCallback(o =>
            {
                Players.Clear();
                foreach (Player player in players)
                {
                    Players.Add(player);
                }
            }), players);
            
        }
    }
}
