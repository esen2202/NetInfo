using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core
{
    public class CommandLine : ICommandLine
    {
        public List<string> ReturnData { get; set; }
        public event EventHandler OnProcessCompleted;

        public CommandLine()
        {
            ReturnData = new List<string>();
        }
        public void Execute(string action)
        {
            var process = new Process();
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        WindowStyle = ProcessWindowStyle.Hidden,
            //        CreateNoWindow = true,
            //        FileName = "cmd.exe",
            //        UseShellExecute = false,
            //        RedirectStandardOutput = true,
            //        Arguments = "/c " + action
            //    }
            //};
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = action;
            process.EnableRaisingEvents = true;

            process.OutputDataReceived += Process_OutputDataReceived;
            process.Exited += Process_Exited;
            process.Start();





        }
        private void Process_Exited(object sender, EventArgs e)
        {
            var pro = (Process)sender;
            var output = pro.StandardOutput.ReadToEnd();
            var test = output.Replace(Environment.NewLine, "");
            if(test == "")
            ReturnData.Add("Success");
            else
            {
                var results = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (var item in results)
                    ReturnData.Add(item);
            }

            OnProcessCompleted?.Invoke(sender,e);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ReturnData.Add(e.Data);
        }
    }
}
