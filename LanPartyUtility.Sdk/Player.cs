using System.Runtime.Serialization;

namespace LanPartyUtility.Sdk
{
    [DataContract]
    public class Player
    {
        public Player(string hostname, string ipAddress, string subnetmask)
        {
            this.id = -1;
            this.hostname = hostname;
            this.ipAddress = ipAddress;
            this.subnetmask = subnetmask;
        }

        public Player() { }

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
