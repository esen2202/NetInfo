using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetAda.UC
{
    /// <summary>
    /// Interaction logic for NetList.xaml
    /// </summary>
    public partial class NetList : UserControl
    {
        public NetList()
        {
            InitializeComponent();

            this.DataContextChanged += NetListItem_DataContextChanged;
        }

        private void NetListItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsNetworkIconChanger(((NetInfo.AdapterObject)this.DataContext).IsOperationalStatusUp);
        }

        private void IsNetworkIconChanger(bool status)
        {
            iconNetworkStatus.Kind = status ? MaterialDesignThemes.Wpf.PackIconKind.LanConnect : MaterialDesignThemes.Wpf.PackIconKind.LanDisconnect;
            iconNetworkStatus.Foreground = status ? new SolidColorBrush(Color.FromArgb(255, 59, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 97, 89));

        }
    }
}
