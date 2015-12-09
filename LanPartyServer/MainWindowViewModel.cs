using LanPartyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyServer
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Model model;

        public MainWindowViewModel()
        {
            this.model = Model.Instance;
            this.startCmd = new StartCommand();
        }

        private StartCommand startCmd;
        public StartCommand StartCmd
        {
            get { return this.startCmd; }
            set { this.startCmd = value; }
        }
    }
}
