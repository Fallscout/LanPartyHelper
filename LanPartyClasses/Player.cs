using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyClasses
{
    public class Player
    {
        public Player()
        {

        }

        private TcpClient socket;
        public TcpClient Socket
        {
            get { return this.socket; }
            set { this.socket = value; }
        }

        private string hostname;
        public string Hostname
        {
            get { return this.hostname; }
            set { this.hostname = value; }
        }

        private string nickname;
        public string Nickname
        {
            get { return this.nickname; }
            set { this.nickname = value; }
        }

        private string ipAddress;
        public string IPAddress
        {
            get { return this.ipAddress; }
            set { this.ipAddress = value; }
        }

        private string subnetmask;
        public string Subnetmask
        {
            get { return this.subnetmask; }
            set { this.subnetmask = value; }
        }
    }
}
