using LanPartyUtility.Sdk;

namespace LanPartyUtility.Server.Plugins
{
    public class ClearCommand : ITerminalCommand
    {
        public ClearCommand()
        {
            this.Name = "clear";
        }

        public string Name { get; set; }

        public void Execute(ITerminal terminal, string[] args)
        {
            terminal.Clear();
        }
    }
}
