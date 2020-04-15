using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace NetSetTestUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSetAdapter_Click(object sender, RoutedEventArgs e)
        {
            NetInfo.AdapterSettings.SetStaticConfiguration(txtAdapterCaption.Text, txtGateway.Text, txtSubnet.Text, txtIpAddress.Text);

            if ((bool)chkDHCP.IsChecked)
            {
                SetIP("/c netsh interface ip set address \"" + txtAdapterCaption.Text + "\" dhcp & netsh interface ip set dns \"" + txtAdapterCaption.Text + "\" dhcp");
            }
            else
            {
                SetIP("/c netsh interface ip set address \"" + txtAdapterCaption.Text + "\" static " + txtIpAddress.Text +
                  " " + txtSubnet.Text + " " + txtGateway.Text + " & netsh interface ip set dns \"" +
                  txtAdapterCaption.Text + "\" static " + txtIpAddress.Text);
            }
        }

        private void SetIP(string arg)
        {
            var process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = arg;
            process.EnableRaisingEvents = true;

            process.OutputDataReceived += Process_OutputDataReceived;
            process.Exited += Process_Exited;
            process.Start();
            //int exitcode;
            //var result = Execute(arg, out exitcode);
            //MessageBox.Show("");
        }

        private void Process_Exited(object sender, EventArgs e)
        {
           Process a = (Process)sender;
           var output = a.StandardOutput.ReadToEnd();
           var test = output.Replace(Environment.NewLine, ""); 

           MessageBox.Show(string.IsNullOrEmpty(test) ? "Başarılı" : test);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageBox.Show(string.IsNullOrEmpty(e.Data) ? "Başarılı" : e.Data);
        }

        //public IEnumerable<string> Execute(string action, out int exitCode)
        //{
        //    var process = new Process
        //    {
        //        StartInfo = new ProcessStartInfo
        //        {
        //            WindowStyle = ProcessWindowStyle.Hidden,
        //            CreateNoWindow = true,
        //            FileName = "cmd.exe",
        //            UseShellExecute = false,
        //            RedirectStandardOutput = true,
        //            Arguments = "/c " + action
        //        }
        //    };

        //    process.Start();

        //    var lines = new List<string>();
        //    while (!process.StandardOutput.EndOfStream)
        //        lines.Add(process.StandardOutput.ReadLine());

        //    exitCode = process.ExitCode;

        //    return lines;
        //}
    }

   
}
