using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanPartyUtility.Client
{
    public class DisconnectCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public DisconnectCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //return this.viewModel.IsConnected == true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.viewModel.LobbyClient.Disconnect(this.viewModel.Self.Id);
            this.viewModel.IsConnected = false;
        }
    }
}
