using LanPartyUtility.Sdk;
using System;

namespace LanPartyUtility.Services
{
    public class LobbyManagerEventArgs : EventArgs
    {
        public LobbyManagerEventArgs(Player player)
        {
            this.Player = player;
        }

        public Player Player { get; set; }
    }
}
