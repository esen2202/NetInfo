using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NetInfo
{
    class AdapterInfo
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
                            UsesWins = adapterPropV4.UsesWins
                        });
                    }
                }

            }
        }
    }

    class AdapterObject
    {

        #region Base Info
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string networkInterfaceType;

        public string NetworkInterfaceType
        {
            get { return networkInterfaceType; }
            set { networkInterfaceType = value; }
        }

        private string physicalAddress;

        public string PhysicalAddress
        {
            get { return physicalAddress; }
            set { physicalAddress = value; }
        }

        private bool isReceiveOnly;

        public bool IsReceiveOnly
        {
            get { return isReceiveOnly; }
            set { isReceiveOnly = value; }
        }

        private bool supportsMulticast;

        public bool SupportMulticast
        {
            get { return supportsMulticast; }
            set { supportsMulticast = value; }
        }
        #endregion

        #region IP
        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        private string subnetMask;

        public string SubnetMask
        {
            get { return subnetMask; }
            set { subnetMask = value; }
        }

        private string gateway;

        public string Gateway
        {
            get { return gateway; }
            set { gateway = value; }
        }
        #endregion

        #region DNS
        private string dnsSuffix;

        public string DnsSuffix
        {
            get { return dnsSuffix; }
            set { dnsSuffix = value; }
        }

        private bool isDnsEnabled;

        public bool IsDnsEnabled
        {
            get { return isDnsEnabled; }
            set { isDnsEnabled = value; }
        }

        private bool isDynamicDnsEnabled;

        public bool IsDynamicDnsEnabled
        {
            get { return isDynamicDnsEnabled; }
            set { isDynamicDnsEnabled = value; }
        }
        #endregion

        #region IP4 Specific data

        private int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private int mtu;

        public int Mtu
        {
            get { return mtu; }
            set { mtu = value; }
        }

        private bool isAutomaticPrivateAddressingActive;

        public bool IsAutomaticPrivateAddressingActive
        {
            get { return isAutomaticPrivateAddressingActive; }
            set { isAutomaticPrivateAddressingActive = value; }
        }

        private bool isAutomaticPrivateAddressingEnabled;

        public bool IsAutomaticPrivateAddressingEnabled
        {
            get { return isAutomaticPrivateAddressingEnabled; }
            set { isAutomaticPrivateAddressingEnabled = value; }
        }

        private bool isForwardingEnabled;

        public bool IsForwardingEnabled
        {
            get { return isForwardingEnabled; }
            set { isForwardingEnabled = value; }
        }

        private bool usesWins;

        public bool UsesWins
        {
            get { return usesWins; }
            set { usesWins = value; }
        }

        #endregion
    }
}
