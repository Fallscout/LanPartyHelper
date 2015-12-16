using LanPartyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyServer
{
    public class TCPServerModule
    {
        private TcpListener listener;
        private bool isRunning;

        public void Start()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            this.listener = new TcpListener(ipAddress, 4589);
            this.listener.Start();
            this.isRunning = true;
        }

        public void Stop()
        {
            this.listener.Stop();
            this.isRunning = false;
        }
    }
}
