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

            Uri baseAddress = new Uri("http://localhost:3745/LanPartyUtility");

            this.viewModel.LobbyManagerHost = new ServiceHost(typeof(LobbyManagerService), baseAddress);

            try
            {
                this.viewModel.LobbyManagerHost.AddServiceEndpoint(typeof(ILobbyManager), new WSHttpBinding(), "LobbyManagerService");

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                this.viewModel.LobbyManagerHost.Description.Behaviors.Add(smb);

                viewModel.LobbyManagerHost.Open();
            }
            catch (CommunicationException e)
            {
                viewModel.LobbyManagerHost.Abort();
            }
        }
    }
}
