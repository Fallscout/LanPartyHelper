using System;

namespace LanPartyUtility.Common
{
    public class LobbyEventArgs : EventArgs
    {
        public LobbyEventArgs(Player newPlayer)
        {
            this.newPlayer = newPlayer;
        }

        private Player newPlayer;
        public Player NewPlayer
        {
            get { return this.newPlayer; }
            set { this.newPlayer = value; }
        }
    }
}
