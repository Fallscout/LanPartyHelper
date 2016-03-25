using LanPartyUtility.Sdk;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace LanPartyUtility.Services
{
    [ServiceContract(Namespace="http://fallscout.com", SessionMode=SessionMode.Required, CallbackContract=typeof(ILobbyManagerCallback))]
    public interface ILobbyManager
    {
        [OperationContract]
        int Connect(Player player);

        [OperationContract(IsOneWay = true)]
        void Disconnect(int id);
    }

    public interface ILobbyManagerCallback
    {
        [OperationContract(IsOneWay=true)]
        void RefreshPlayerList(ObservableCollection<Player> players);

        [OperationContract]
        string ScanDirectory();
    }
}
