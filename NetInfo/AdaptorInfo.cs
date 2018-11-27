using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetInfo
{
    public class AdapterInfo
    {
        private static AdapterInfo adapterInfo;
        private static object objLock = new object();

        private NetworkInterface[] adapters;
        public List<AdapterObject> listAdapter;

        private AdapterInfo()
        {
            adapters = NetworkInterface.GetAllNetworkInterfaces();
            listAdapter = new List<AdapterObject>();
            RefreshInfos();
        }

        public static AdapterInfo CreateInstance()
        {
            if (adapterInfo == null)
                lock (objLock)
                {
                    if (adapterInfo == null)
                        adapterInfo = new AdapterInfo();
                }

            return adapterInfo;
        }

        public void RefreshInfos()
        {
            listAdapter.Clear();

            foreach (NetworkInterface adapter in adapters)
            {
                var ipProp = adapter.GetIPProperties();

                foreach (UnicastIPAddressInformation ip in ipProp.UnicastAddresses)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up &&
                        ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IPInterfaceProperties adapterProp = adapter.GetIPProperties();
                        IPv4InterfaceProperties adapterPropV4 = adapterProp.GetIPv4Properties();
                        GatewayIPAddressInformationCollection gate = adapter.GetIPProperties().GatewayAddresses;

                        listAdapter.Add(new AdapterObject
                        {
                            Description = adapter.Description,
                            NetworkInterfaceType = adapter.NetworkInterfaceType.ToString(),
                            PhysicalAddress = adapter.GetPhysicalAddress().ToString(),
                            IsReceiveOnly = adapter.IsReceiveOnly,
                            SupportMulticast = adapter.SupportsMulticast,

                            IpAddress = ip.Address.ToString(),
                            SubnetMask = ip.IPv4Mask.ToString(),
                            Gateway = gate.Any() ? gate.FirstOrDefault().Address.ToString() : "",

                            DnsSuffix = adapterProp.DnsSuffix,
                            IsDnsEnabled = adapterProp.IsDnsEnabled,
                            IsDynamicDnsEnabled = adapterProp.IsDynamicDnsEnabled,

                            Index = adapterPropV4.Index,
                            Mtu = adapterPropV4.Mtu,
                            IsAutomaticPrivateAddressingActive = adapterPropV4.IsAutomaticPrivateAddressingActive,
                            IsAutomaticPrivateAddressingEnabled = adapterPropV4.IsAutomaticPrivateAddressingEnabled,
                            IsForwardingEnabled = adapterPropV4.IsForwardingEnabled,
                            UsesWins = adapterPropV4.UsesWins,
                            IsDHCPEnabled = adapterPropV4.IsDhcpEnabled
                           
                        });
                    }
                }

            }
        }
    }
}
