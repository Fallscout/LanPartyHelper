using LanPartyUtility.Sdk;
using System;

namespace LanPartyUtility.Common
{
    public class DelegateTerminalCommand : ITerminalCommand
    {
        public DelegateTerminalCommand(string name, Action<ITerminal, string[]> command)
        {
            this.Name = name;
            this.Command = command;
        }

        public Action<ITerminal, string[]> Command { get; set; }

        public string Name { get; set; }

        public void Execute(ITerminal terminal, string[] args)
        {
            this.Command(terminal, args);
        }
    }
}
