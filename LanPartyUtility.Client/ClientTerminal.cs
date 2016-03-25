using LanPartyUtility.Common;

namespace LanPartyUtility.Client
{
    public class ClientTerminal : Terminal
    {

        public override System.Collections.ObjectModel.ObservableCollection<Sdk.Player> Players
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Sdk.Player SelectedPlayer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override System.Collections.ObjectModel.ObservableCollection<Sdk.Game> Games
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Sdk.Game SelectedGame
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
