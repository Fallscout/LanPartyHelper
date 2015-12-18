using LanPartyUtility.Client.Proxy;
using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanPartyUtility.Client
{
    public class ConnectCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            LobbyManagerClient client = new LobbyManagerClient();

            client.Connect(new Player("CE01", "Chris", "127.0.0.1", "255.255.255.0"));
        }
    }
}
