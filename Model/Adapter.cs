using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AdapterConfiguration: IModel
    {
        public long Id { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }

        public string DHCPServer { get; set; }
        public string DNSServer1 { get; set; }
        public string DNSServer2 { get; set; }

        public override string ToString()
        {
            return "[" + Name + "] " + IpAddress.ToString() + " - " + SubnetMask.ToString() + " - " + Gateway.ToString();
        }
    }

}
