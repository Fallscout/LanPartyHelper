using LanPartyUtility.Common;
using LanPartyUtility.Services;
using System;
using System.Collections.ObjectModel;
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
            this.LobbyManagerHost = null;

            this.StartCmd = new StartCommand(this);
            this.StopCmd = new StopCommand(this);
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

        public ServiceHost LobbyManagerHost { get; set; }

        public ObservableCollection<Player> Players
        {
            get { return LobbyManagerService.Players; }
            set
            {
                LobbyManagerService.Players = value;
                OnPropertyChanged("Players");
            }
        }

        public ICommand StartCmd { get; private set; }
        public ICommand StopCmd { get; private set; }
    }
}
