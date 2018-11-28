using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Net;

namespace NetInfo
{
 
    public class WMIHelper
    {
        #region Public Static

        public static void SetDHCP(string nicName)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (System.Management.ManagementObject mo in moc)
            {
                // Make sure this is a IP enabled device. Not something like memory card or VM Ware
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = null;
                        ManagementBaseObject enableDHCP = mo.InvokeMethod("EnableDHCP", null, null);
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                    }
                }
            }
        }

        public static void SetIP(string nicName, string IpAddresses, string SubnetMask, string Gateway, string DnsSearchOrder)
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                // Make sure this is a IP enabled device. Not something like memory card or VM Ware
                if ((bool)mo["IPEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {

                        ManagementBaseObject newIP = mo.GetMethodParameters("EnableStatic");
                        ManagementBaseObject newGate = mo.GetMethodParameters("SetGateways");
                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");

                        newGate["DefaultIPGateway"] = new string[] { Gateway };
                        newGate["GatewayCostMetric"] = new int[] { 1 };

                        newIP["IPAddress"] = IpAddresses.Split(',');
                        newIP["SubnetMask"] = new string[] { SubnetMask };

                        newDNS["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');

                        ManagementBaseObject setIP = mo.InvokeMethod("EnableStatic", newIP, null);
                        ManagementBaseObject setGateways = mo.InvokeMethod("SetGateways", newGate, null);
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                        break;
                    }
                }
            }
        }

        public static void GetIP(string nicName, out string[] ipAdresses, out string[] subnets, out string[] gateways, out string[] dnses)
        {
            ipAdresses = null;
            subnets = null;
            gateways = null;
            dnses = null;

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                // Make sure this is a IP enabled device. Not something like memory card or VM Ware
                if ((bool)mo["ipEnabled"])
                {
                    if (mo["Caption"].Equals(nicName))
                    {
                        ipAdresses = (string[])mo["IPAddress"];
                        subnets = (string[])mo["IPSubnet"];
                        gateways = (string[])mo["DefaultIPGateway"];
                        dnses = (string[])mo["DNSServerSearchOrder"];

                        break;
                    }
                }
            }
        }

        public static List<string> GetNICNames()
        {
            List<string> nicNames = new List<string>();

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["ipEnabled"])
                {
                    nicNames.Add((string)mo["Caption"]);
                }
            }

            return nicNames;
        }

        #endregion
    }
}
