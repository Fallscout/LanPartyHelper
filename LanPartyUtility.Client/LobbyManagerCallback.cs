using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using LanPartyUtility.Sdk;
using System;
using System.Collections.ObjectModel;
using System.IO;

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
            string[] directories = Directory.GetDirectories(Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH"));
            string[] files = Directory.GetFiles(Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH"));

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
