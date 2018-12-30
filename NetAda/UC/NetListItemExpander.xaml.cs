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
    public partial class NetListItemExpander : UserControl
    {
        public NetListItemExpander()
        {
            InitializeComponent();

            //IsDHCPIconChanger((bool)chkIsDHCPEnabled.IsChecked);
            IsDHCPIconChanger(false);
            IsNetworkIconChanger(false);
            this.DataContextChanged += NetListItem_DataContextChanged;
        }

        private void NetListItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsDHCPIconChanger(((NetInfo.AdapterObject)this.DataContext).IsDHCPEnabled);
            IsNetworkIconChanger(((NetInfo.AdapterObject)this.DataContext).IsOperationalStatusUp);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void IsDHCPIconChanger(bool status)
        {
            iconDHCP.Kind = status ? MaterialDesignThemes.Wpf.PackIconKind.Check : MaterialDesignThemes.Wpf.PackIconKind.Cancel;
            iconDHCP.Foreground = status ? new SolidColorBrush(Color.FromArgb(255, 59, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 97, 89));
            ExpAdapter.IsExpanded = true;
        }

        private void IsNetworkIconChanger(bool status)
        {
            iconNetworkStatus.Kind = status ? MaterialDesignThemes.Wpf.PackIconKind.LanConnect : MaterialDesignThemes.Wpf.PackIconKind.LanDisconnect;
            iconNetworkStatus.Foreground = status ? new SolidColorBrush(Color.FromArgb(255, 59, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 97, 89));

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
