using System.Diagnostics;

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

    }
}
