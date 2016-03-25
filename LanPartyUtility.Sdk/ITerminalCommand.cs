
using System;
namespace LanPartyUtility.Sdk
{
    public interface ITerminalCommand
    {
        string Name { get; set; }

        void Execute(ITerminal terminal, string[] args);
    }
}
