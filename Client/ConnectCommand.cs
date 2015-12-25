using LanPartyUtility.Client.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LanPartyUtility.Client
{
    public class ConnectCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public ConnectCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
            //return this.viewModel.IsConnected == false;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            this.viewModel.LobbyClient = new LobbyManagerClient(new InstanceContext(new LobbyManagerCallback()));

            await Task.Run(() =>
            {
                this.viewModel.Self.Id = this.viewModel.LobbyClient.Connect(this.viewModel.Self);
            });

            this.viewModel.IsConnected = true;
        }
    }
}
