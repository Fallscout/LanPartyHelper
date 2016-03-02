using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanPartyUtility.Server
{
    interface ITerminalCommand
    {
        Action<ServerTerminal, string[]> Command { get; set; }
    }
}
