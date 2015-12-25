using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LanPartyUtility.Client
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            #region Instantiate Player

            string ip = String.Empty;

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface inter in interfaces)
            {
                IPInterfaceProperties prop = inter.GetIPProperties();
                if (prop.GatewayAddresses.Count > 0 && prop.DnsAddresses.Count > 0)
                {
                    ip = prop.DnsAddresses[0].ToString();
                }
            }

            this.Self = new Player(Dns.GetHostName(), ip, "255.255.255.0");

            #endregion

            this.ConnectCmd = new ConnectCommand(this);
            this.DisconnectCmd = new DisconnectCommand(this);
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            if (this.IsConnected)
            {
                this.DisconnectCmd.Execute(null);
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set
            {
                this.isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        private Player self;
        public Player Self
        {
            get { return this.self; }
            set
            {
                this.self = value;
                OnPropertyChanged("Self");
            }
        }

        public ObservableCollection<Player> Players
        {
            get { return LobbyManagerCallback.Players; }
            set
            {
                LobbyManagerCallback.Players = value;
                OnPropertyChanged("Players");
            }
        }

        public LobbyManagerClient LobbyClient { get; set; }

        public ICommand ConnectCmd { get; private set; }
        public ICommand DisconnectCmd { get; private set; }
    }
}
