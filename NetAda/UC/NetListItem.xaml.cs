using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NetAda.UC
{
    /// <summary>
    /// Interaction logic for NetListItem.xaml
    /// </summary>
    public partial class NetListItem : UserControl
    {
        public NetListItem()
        {
            InitializeComponent();

            //IsDHCPIconChanger((bool)chkIsDHCPEnabled.IsChecked);
            IsDHCPIconChanger(true);

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ChkIsDHCPEnabled_Checked(object sender, RoutedEventArgs e)
        {
            //IsDHCPIconChanger((bool)chkIsDHCPEnabled.IsChecked);
        }

        private void IsDHCPIconChanger(bool status)
        {
            iconDHCP.Kind = status ? MaterialDesignThemes.Wpf.PackIconKind.Check : MaterialDesignThemes.Wpf.PackIconKind.Cancel;
            iconDHCP.Foreground = status ? new SolidColorBrush(Color.FromArgb(255, 59, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 97, 89));
            ExpAdapter.IsExpanded = status;
        }

        private void BtnCopyIPAddress_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxbIPAddress.Text);
        }

        private void BtnCopyGateway_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxbGateway.Text);
        }
    }
}
