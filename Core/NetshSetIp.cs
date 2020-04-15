using Model;
using System;

namespace Core
{
    public class NetshSetIp
    {
        public string ResultData { get; set; }
        public event EventHandler OnProcessCompleted;
        public NetshSetIp(string interfaceName, AdapterConfiguration adapterConfig = null)
        {
            CommandLine cmd = new CommandLine();
            cmd.OnProcessCompleted += delegate (object sender, EventArgs e)
            {
                string data = "";
                cmd.ReturnData.ForEach(item =>
                {
                    data += item.ToString();
                });
                ResultData = data.Trim() == "" ? "Success" : data;

                OnProcessCompleted?.Invoke(sender, new EventArgs() {});
            };
            cmd.Execute(CreateCommand(interfaceName, adapterConfig));
        }

        public string CreateCommand(string interfaceName, AdapterConfiguration adapterConfig = null)
        {
            if (adapterConfig == null)
            {
                return "/c netsh interface ip set address \"" + interfaceName + "\" dhcp &  netsh interface ip set dns \"" + interfaceName + "\" dhcp";
            }
            else
            {
                string command = "/c netsh interface ip set address \"" + interfaceName + "\" static " + adapterConfig.IpAddress +
                  " " + adapterConfig.SubnetMask + " " + adapterConfig.Gateway;
                if (adapterConfig.DNSServer1 != null)
                    command += " & netsh interface ip set dns \"" +
                  interfaceName + "\" static " + adapterConfig.DNSServer1;
                return command;
            }
        }
    }
}