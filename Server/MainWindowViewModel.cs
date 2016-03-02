using LanPartyUtility.Common;
using LanPartyUtility.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LanPartyUtility.Server
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.isLobbyManagerOnline = false;

            LobbyManagerService.PlayerConnected += LobbyManagerService_PlayerConnected;
            LobbyManagerService.PlayerDisconnected += LobbyManagerService_PlayerDisconnected;

            this.StartCmd = new StartCommand(this);
            this.StopCmd = new StopCommand(this);
        }

        void LobbyManagerService_PlayerDisconnected(object sender, LobbyManagerEventArgs args)
        {
            this.Terminal.WriteLine(String.Format("[{0}] {1} ({2}) disconnected", DateTime.Now.ToLongTimeString(),
                args.Player.Nickname, args.Player.Hostname));
        }

        void LobbyManagerService_PlayerConnected(object sender, LobbyManagerEventArgs args)
        {
            this.Terminal.WriteLine(String.Format("[{0}] {1} ({2}) connected with IP-Address: {3}", DateTime.Now.ToLongTimeString(),
                args.Player.Nickname, args.Player.Hostname, args.Player.IPAddress));
        }

        public void OnWindowClosing(object sender, EventArgs e)
        {
            if (this.IsLobbyManagerOnline)
            {
                this.StopCmd.Execute(null);
            }
        }

        private bool isLobbyManagerOnline;
        public bool IsLobbyManagerOnline
        {
            get { return this.isLobbyManagerOnline; }
            set
            {
                this.isLobbyManagerOnline = value;
                OnPropertyChanged("IsLobbyManagerOnline");
            }
        }

        private Player selectedPlayer;
        public Player SelectedPlayer
        {
            get { return this.selectedPlayer; }
            set
            {
                this.selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
            }
        }

        public ObservableCollection<Player> Players
        {
            get { return LobbyManagerService.Players; }
            set
            {
                LobbyManagerService.Players = value;
                OnPropertyChanged("Players");
            }
        }

        public ServerTerminal Terminal { get; set; }

        public ICommand StartCmd { get; private set; }
        public ICommand StopCmd { get; private set; }
    }
}
