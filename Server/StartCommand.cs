using LanPartyUtility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanPartyUtility.Server
{
    public class StartCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public StartCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            try
            {
                LobbyManagerService.Host.Open();
                this.viewModel.IsLobbyManagerOnline = true;
                this.viewModel.Terminal.WriteLine("LobbyManager started");
            }
            catch (CommunicationException e)
            {
                LobbyManagerService.Host.Abort();
            }
        }
    }
}
