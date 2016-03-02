using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanPartyUtility.Client
{
    public class LobbyManagerCallback : ILobbyManagerCallback
    {
        public static ObservableCollection<Player> Players { get; set; }

        static LobbyManagerCallback()
        {
            Players = new ObservableCollection<Player>();
        }

        public void RefreshPlayerList(Player[] players)
        {
            Players.Clear();
            foreach (Player player in players)
            {
                Players.Add(player);
            }
        }

        public string ScanDirectory()
        {
            string[] directories = Directory.GetDirectories("C:\\Users\\ce.Team4");
            string[] files = Directory.GetFiles("C:\\Users\\ce.Team4");

            string result = String.Empty;

            foreach(string directory in directories) {
                result += directory + "\n";
            }

            foreach(string file in files) {
                result += files + "\n";
            }

            return result;
        }
    }
}
