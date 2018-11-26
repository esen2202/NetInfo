using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetInfo
{
    class Program
    {
        private static AdapterInfo adapterInfo;

        static void Main(string[] args)
        {
            adapterInfo = AdapterInfo.CreateInstance();

            adapterInfo.listAdapter.ForEach(adapter =>
            {
                ShowAdapters(adapter);
            });

            Console.Read();
        }

        private static void ShowAdapters(AdapterObject adapter)
        {
            Console.WriteLine("====== {0} ======", adapter.Description);
            Console.WriteLine();
            Console.WriteLine("  Interface type .................... : {0}", adapter.NetworkInterfaceType);
            Console.WriteLine("  Physical Address .................. : {0}", adapter.PhysicalAddress);
            Console.WriteLine("  Is receive only.................... : {0}", adapter.IsReceiveOnly);
            Console.WriteLine("  Multicast.......................... : {0}", adapter.SupportMulticast);
            Console.WriteLine();
            Console.WriteLine("  IP Address ........................ : {0}", adapter.IpAddress);
            Console.WriteLine("  Subnet     ........................ : {0}", adapter.SubnetMask);
            Console.WriteLine("  Gateway    ........................ : {0}", adapter.Gateway);
            Console.WriteLine();
            Console.WriteLine("  DNS suffix ........................ : {0}", adapter.DnsSuffix);
            Console.WriteLine("  DNS enabled ....................... : {0}", adapter.IsDnsEnabled);
            Console.WriteLine("  Dynamically configured DNS ........ : {0}", adapter.IsDynamicDnsEnabled);
            Console.WriteLine();
            Console.WriteLine("  Index ............................. : {0}", adapter.Index);
            Console.WriteLine("  MTU ............................... : {0}", adapter.Mtu);
            Console.WriteLine("  APIPA active....................... : {0}", adapter.IsAutomaticPrivateAddressingActive);
            Console.WriteLine("  APIPA enabled...................... : {0}", adapter.IsAutomaticPrivateAddressingEnabled);
            Console.WriteLine("  Forwarding enabled................. : {0}", adapter.IsForwardingEnabled);
            Console.WriteLine("  Uses WINS ......................... : {0}", adapter.UsesWins);
            Console.WriteLine("  DHCP enabled....................... : {0}", adapter.IsDHCPEnabled);
            Console.WriteLine();
        }
    }
}
