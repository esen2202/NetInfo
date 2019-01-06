using System;
using System.Diagnostics;
using System.Management;

namespace NetInfo
{
    public static class AdapterSettings
    {
        public static void StartNetworkConnections()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("NCPA.cpl");
            startInfo.UseShellExecute = true;

            Process.Start(startInfo);
        }

        public static void SetStaticConfiguration(string myadapterDescription,  string gateway,  string subnetMask,  string address)
        {
          

            var adapterConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var networkCollection = adapterConfig.GetInstances();

            foreach (ManagementObject adapter in networkCollection)
            {
                string description = adapter["Description"] as string;
                if (string.Compare(description,
                        myadapterDescription, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    try
                    {
                        // Set DefaultGateway
                        var newGateway = adapter.GetMethodParameters("SetGateways");
                        newGateway["DefaultIPGateway"] = new string[] { gateway };
                        newGateway["GatewayCostMetric"] = new int[] { 1 };

                        // Set IPAddress and Subnet Mask
                        var newAddress = adapter.GetMethodParameters("EnableStatic");
                        newAddress["IPAddress"] = new string[] { address };
                        newAddress["SubnetMask"] = new string[] { subnetMask };

                        adapter.InvokeMethod("EnableStatic", newAddress, null);
                        adapter.InvokeMethod("SetGateways", newGateway, null);

                        Console.WriteLine("Updated to static IP address!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to Set IP : " + ex.Message);
                    }
                }
            }
        }

        public static void SetDHCP(string adapterDescription)
        {
            var adapterConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var networkCollection = adapterConfig.GetInstances();

            foreach (ManagementObject adapter in networkCollection)
            {
                string description = adapter["Description"] as string;
                if (string.Compare(description,
                        adapterDescription, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    try
                    {
                        adapter.InvokeMethod("EnableDHCP", null);

                        Console.WriteLine("Updated Dynamic address!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to Set IP : " + ex.Message);
                    }
                }
            }
        }

    }
}
