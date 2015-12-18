using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LanPartyUtility.Services
{

    public delegate void PlayerConnectedEventHandler(object sender, LobbyEventArgs e);

    public class LobbyManagerService : ILobbyManager
    {
        public static event PlayerConnectedEventHandler PlayerConnected;
        public static int PlayerCount { get; set; }

        protected virtual void OnPlayerConnected(LobbyEventArgs e)
        {
            var handler = PlayerConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public int Connect(Player player)
        {
            PlayerCount += 1;
            player.Id = PlayerCount;
            OnPlayerConnected(new LobbyEventArgs(player));

            return PlayerCount;
        }

        public void Disconnect(int id)
        {

        }
    }
}
