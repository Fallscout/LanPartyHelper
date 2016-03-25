
using System.Collections.ObjectModel;
namespace LanPartyUtility.Sdk
{
    public interface ITerminal
    {
        ObservableCollection<Player> Players { get; }
        Player SelectedPlayer { get; set; }
        ObservableCollection<Game> Games { get; }
        Game SelectedGame { get; set; }

        void WriteLine(string text);
        void Clear();
    }
}
