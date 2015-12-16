using LanPartyClasses;
using System.Windows.Data;

namespace LanPartyServer
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {

        }

        private Player selectedPlayer;
        public Player SelectedPlayer
        {
            get { return selectedPlayer; }
            set { selectedPlayer = value; }
        }

        private ListCollectionView players;
        public ListCollectionView Players
        {
            get { return players; }
            set { players = value; }
        }

        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; }
        }

        private ListCollectionView games;
        public ListCollectionView Games
        {
            get { return games; }
            set { games = value; }
        }

        private TCPServerModule tcpModule;
    }
}
