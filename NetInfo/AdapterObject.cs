using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NetInfo
{
    public class AdapterObject : INotifyPropertyChanged
    {

        #region Base Info

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(() => Name);
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(()=>Description);
            }
        }

        private string networkInterfaceType;

        public string NetworkInterfaceType
        {
            get { return networkInterfaceType; }
            set
            {
                networkInterfaceType = value;
                OnPropertyChanged(()=>NetworkInterfaceType);
            }
        }

        private string physicalAddress;

        public string PhysicalAddress
        {
            get { return physicalAddress; }
            set
            {
                physicalAddress = value;
                OnPropertyChanged(()=>PhysicalAddress);
            }
        }

        private bool isReceiveOnly;

        public bool IsReceiveOnly
        {
            get { return isReceiveOnly; }
            set
            {
                isReceiveOnly = value;
                OnPropertyChanged(()=>IsReceiveOnly);
            }
        }

        private bool supportsMulticast;

        public bool SupportMulticast
        {
            get { return supportsMulticast; }
            set
            {
                supportsMulticast = value;
                OnPropertyChanged(()=>SupportMulticast);
            }
        }

        private bool isOperationalStatusUp;

        public bool IsOperationalStatusUp
        {
            get { return isOperationalStatusUp; }
            set
            {
                isOperationalStatusUp = value;
                OnPropertyChanged(() => IsOperationalStatusUp);
            }
        }

        private long speed;

        public long Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                OnPropertyChanged(() => Speed);
            }
        }

        private bool internet;

        public bool Internet
        {
            get { return internet; }
            set
            {
                internet = value;
                OnPropertyChanged(() => Internet);
            }
        }


        #endregion

        #region IP
        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
                OnPropertyChanged(()=>IpAddress);
            }
        }

        private string subnetMask;

        public string SubnetMask
        {
            get { return subnetMask; }
            set
            {
                subnetMask = value;
                OnPropertyChanged(()=>SubnetMask);
            }
        }

        private string gateway;

        public string Gateway
        {
            get { return gateway; }
            set
            {
                gateway = value;
                OnPropertyChanged(()=>Gateway);
            }
        }
        #endregion

        #region DNS
        private string dnsSuffix;

        public string DnsSuffix
        {
            get { return dnsSuffix; }
            set
            {
                dnsSuffix = value;
                OnPropertyChanged(()=>DnsSuffix);
            }
        }

        private bool isDnsEnabled;

        public bool IsDnsEnabled
        {
            get { return isDnsEnabled; }
            set
            {
                isDnsEnabled = value;
                OnPropertyChanged(()=>IsDnsEnabled);
            }
        }

        private bool isDynamicDnsEnabled;

        public bool IsDynamicDnsEnabled
        {
            get { return isDynamicDnsEnabled; }
            set
            {
                isDynamicDnsEnabled = value;
                OnPropertyChanged(()=>IsDynamicDnsEnabled);
            }
        }
        #endregion

        #region IP4 Specific data

        private int index;

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                OnPropertyChanged(()=>Index);
            }
        }

        private int mtu;

        public int Mtu
        {
            get { return mtu; }
            set
            {
                mtu = value;
                OnPropertyChanged(()=>Mtu);
            }
        }

        private bool isAutomaticPrivateAddressingActive;

        public bool IsAutomaticPrivateAddressingActive
        {
            get { return isAutomaticPrivateAddressingActive; }
            set
            {
                isAutomaticPrivateAddressingActive = value;
                OnPropertyChanged(()=>IsAutomaticPrivateAddressingActive);
            }
        }

        private bool isAutomaticPrivateAddressingEnabled;

        public bool IsAutomaticPrivateAddressingEnabled
        {
            get { return isAutomaticPrivateAddressingEnabled; }
            set
            {
                isAutomaticPrivateAddressingEnabled = value;
                OnPropertyChanged(()=>IsAutomaticPrivateAddressingEnabled);
            }
        }

        private bool isForwardingEnabled;

        public bool IsForwardingEnabled
        {
            get { return isForwardingEnabled; }
            set
            {
                isForwardingEnabled = value;
                OnPropertyChanged(()=>IsForwardingEnabled);
            }
        }

        private bool usesWins;

        public bool UsesWins
        {
            get { return usesWins; }
            set
            {
                usesWins = value;
                OnPropertyChanged(()=>UsesWins);
            }
        }

        private bool isDHCPEnabled;

        public bool IsDHCPEnabled
        {
            get { return isDHCPEnabled; }
            set
            {
                isDHCPEnabled = value;
                OnPropertyChanged(()=>IsDHCPEnabled);
            }
        }

        private string dhcpServer;

        public string DHCPServer
        {
            get { return dhcpServer; }
            set
            {
                dhcpServer = value;
                OnPropertyChanged(() => DHCPServer);
            }
        }

        private string dnsServer1;

        public string DNSServer1
        {
            get { return dnsServer1; }
            set
            {
                dnsServer1 = value;
                OnPropertyChanged(() => DNSServer1);
            }
        }

        private string dnsServer2;

        public string DNSServer2
        {
            get { return dnsServer2; }
            set
            {
                dnsServer2 = value;
                OnPropertyChanged(() => DNSServer2);
            }
        }

        #endregion


        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null)
            {
                OnPropertyChanged(memberExpression.Member.Name);
            }
        }

        #endregion
    }
}
