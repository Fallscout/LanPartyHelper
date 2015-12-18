
using System.Runtime.Serialization;
namespace LanPartyUtility.Common
{
    [DataContract]
    public class Player
    {
        public Player(string hostname, string nickname, string ipAddress, string subnetmask)
        {
            this.hostname = hostname;
            this.nickname = nickname;
            this.ipAddress = ipAddress;
            this.subnetmask = subnetmask;
        }

        [DataMember]
        private int id;
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        [DataMember]
        private string hostname;
        public string Hostname
        {
            get { return this.hostname; }
            set { this.hostname = value; }
        }

        [DataMember]
        private string nickname;
        public string Nickname
        {
            get { return this.nickname; }
            set { this.nickname = value; }
        }

        [DataMember]
        private string ipAddress;
        public string IPAddress
        {
            get { return this.ipAddress; }
            set { this.ipAddress = value; }
        }

        [DataMember]
        private string subnetmask;
        public string Subnetmask
        {
            get { return this.subnetmask; }
            set { this.subnetmask = value; }
        }
    }
}
