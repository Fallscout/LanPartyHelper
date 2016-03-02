using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
