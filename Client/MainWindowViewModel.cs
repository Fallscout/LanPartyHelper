using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyUtility.Client
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.ConnectCmd = new ConnectCommand();
        }

        public ConnectCommand ConnectCmd { get; private set; }
    }
}
