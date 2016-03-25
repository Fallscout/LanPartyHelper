using LanPartyUtility.Common;
using LanPartyUtility.Sdk;
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
            LobbyManagerService.PlayerConnected += (s, e) =>
            {
                this.Terminal.WriteLine(String.Format("[{0}] {1} ({2}) connected with IP-Address: {3}", DateTime.Now.ToLongTimeString(),
                    e.Player.Nickname, e.Player.Hostname, e.Player.IPAddress));
            };

            LobbyManagerService.PlayerDisconnected += (s, e) =>
            {
                this.Terminal.WriteLine(String.Format("[{0}] {1} ({2}) disconnected", DateTime.Now.ToLongTimeString(),
                  e.Player.Nickname, e.Player.Hostname));
            };

            #region Define ToggleLobbyManagerCmd

            this.ToggleLobbyManagerCmd = new DelegateCommand(param =>
            {
                if ((bool)param)
                {
                    try
                    {
                        //Execute Visual Studio as administrator!
                        LobbyManagerService.InstantiateHost();
                        LobbyManagerService.Host.Open();
                        this.IsLobbyManagerOnline = true;

                        this.Terminal.WriteLine("LobbyManager started");
                    }
                    catch (CommunicationException e)
                    {
                        LobbyManagerService.Host.Abort();
                        this.IsLobbyManagerOnline = false;
                        this.Terminal.WriteLine(String.Format("Failed to start LobbyManager: {0}", e.Message));
                    }
                }
                else
                {
                    LobbyManagerService.Host.Close();
                    this.IsLobbyManagerOnline = false;
                    this.Terminal.WriteLine("LobbyManager closed");
                }
            });

            #endregion

            this.ToggleFtpCmd = new DelegateCommand(param =>
            {

            });

            this.Terminal = new ServerTerminal(this);
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

        private bool isFtpOnline;
        public bool IsFtpOnline
        {
            get { return this.isFtpOnline; }
            set
            {
                this.isFtpOnline = value;
                OnPropertyChanged("IsFtpOnline");
            }
        }

        public async void OnWindowClosing(object sender, EventArgs e)
        {
            if (this.IsLobbyManagerOnline)
            {
                await Task.Run(() => this.ToggleLobbyManagerCmd.Execute(false));
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

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return this.selectedGame; }
            set
            {
                this.selectedGame = value;
                OnPropertyChanged("SelectedGame");
            }
        }

        private ObservableCollection<Game> games;
        public ObservableCollection<Game> Games
        {
            get { return this.games; }
            set
            {
                this.games = value;
                OnPropertyChanged("Games");
            }
        }

        public ServerTerminal Terminal { get; set; }

        public ICommand ToggleLobbyManagerCmd { get; private set; }
        public ICommand ToggleFtpCmd { get; private set; }
    }
}
