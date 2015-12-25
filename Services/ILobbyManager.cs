using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
    }
}
