using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using LanPartyUtility.Sdk;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
            this.ServerIpAddress = Properties.Settings.Default.ServerIpAddress;

            #region Instantiate Player

            this.self = new Player();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface inter in interfaces)
            {
                IPInterfaceProperties prop = inter.GetIPProperties();
                if (prop.GatewayAddresses.Count > 0 && prop.DnsAddresses.Count > 0)
                {
                    this.self.IPAddress = prop.UnicastAddresses[1].Address.ToString();
                    this.self.Subnetmask = prop.UnicastAddresses[1].IPv4Mask.ToString();
                    this.self.Hostname = Dns.GetHostName();
                    break;
                }
            }

            #endregion

            #region Define ToggleConnectCmd

            this.ToggleConnectCmd = new DelegateCommand(async param =>
            {
                if ((bool)param)
                {
                    this.LobbyClient = new LobbyManagerClient(
                        new InstanceContext(new LobbyManagerCallback()),
                        new NetTcpBinding(),
                        new EndpointAddress(String.Format(@"net.tcp://{0}/LanPartyUtility/LobbyManagerService", this.ServerIpAddress)));

                    try
                    {
                        this.Self.Id = await this.LobbyClient.ConnectAsync(this.Self);
                        this.IsConnected = true;
                        CommandManager.InvalidateRequerySuggested();
                    }
                    catch (EndpointNotFoundException)
                    {
                        this.IsConnected = false;
                        //todo: write to terminal
                    }
                    catch (CommunicationException)
                    {
                        this.IsConnected = false;
                    }
                }
                else
                {
                    try
                    {
                        this.LobbyClient.Disconnect(this.Self.Id);
                        this.IsConnected = false;
                    }
                    catch (CommunicationObjectFaultedException)
                    {

                    }
                }
            });

            #endregion
        }

        private string serverIpAddress;
        public string ServerIpAddress
        {
            get
            {
                return this.serverIpAddress;
            }
            set
            {
                this.serverIpAddress = value;
                Properties.Settings.Default.ServerIpAddress = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("ServerIpAddress");
            }
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            if (this.IsConnected)
            {
                this.ToggleConnectCmd.Execute(false);
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

        public ICommand ToggleConnectCmd { get; private set; }

        //public ICommand DisconnectCmd { get; private set; }
    }
}
