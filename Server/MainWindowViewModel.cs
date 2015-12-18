using LanPartyUtility.Common;
using LanPartyUtility.Services;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Data;
using System.Windows.Input;

namespace LanPartyUtility.Server
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.selectedPlayer = null;
            this.players = new ListCollectionView(new ObservableCollection<Player>());

            this.selectedGame = null;
            this.games = new ListCollectionView(new ObservableCollection<Game>());

            this.StartCmd = new StartCommand(this);
            this.StopCmd = new StopCommand(this);

            this.isLobbyManagerOnline = false;

            LobbyManagerService.PlayerConnected += lobbyManager_PlayerConnected;
        }

        void lobbyManager_PlayerConnected(object sender, LobbyEventArgs e)
        {
            Players.AddNewItem(e.NewPlayer);
        }

        private bool isLobbyManagerOnline;
        public bool IsLobbyManagerOnline
        {
            get { return this.isLobbyManagerOnline; }
            set
            {
                this.isLobbyManagerOnline = value;
                OnPropertyChanged("IsLobbyManagerOnline");

                if (this.isLobbyManagerOnline == true)
                {
                    this.StartCmd.Execute(null);
                }
                else
                {
                    this.StopCmd.Execute(null);
                }
            }
        }

        public ServiceHost LobbyManagerHost { get; set; }

        private Player selectedPlayer;
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set 
            { 
                selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");
            }
        }

        private ListCollectionView players;
        public ListCollectionView Players
        {
            get { return players; }
            set
            {
                players = value;
                OnPropertyChanged("Players");
            }
        }

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set 
            { 
                selectedGame = value;
                OnPropertyChanged("SelectedGame");
            }
        }

        private ListCollectionView games;
        public ListCollectionView Games
        {
            get { return games; }
            set { games = value; }
        }

        public StartCommand StartCmd { get; private set; }
        public StopCommand StopCmd { get; private set; }
    }
}
