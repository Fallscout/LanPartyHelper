using LanPartyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyClient
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Model model;

        public MainWindowViewModel()
        {
            this.model = Model.Instance;
            this.connectCmd = new ConnectCommand();
        }

        private ConnectCommand connectCmd;
        public ConnectCommand ConnectCmd
        {
            get { return this.connectCmd; }
            set { this.connectCmd = value; }
        }
    }
}
