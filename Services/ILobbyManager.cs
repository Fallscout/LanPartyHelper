using LanPartyUtility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LanPartyUtility.Services
{
    [ServiceContract]
    public interface ILobbyManager
    {
        [OperationContract]
        int Connect(Player player);

        [OperationContract]
        void Disconnect(int id);
    }
}
