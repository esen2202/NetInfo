using System.Windows;
using System.Windows.Controls;
using NetAda.ViewModels;

namespace NetAda.UC
{
    /// <summary>
    /// Interaction logic for NewConfiguration.xaml
    /// </summary>
    public partial class NewConfiguration : UserControl
    {
        public NewConfiguration()
        {
            InitializeComponent();
            this.DataContext = new ViewModelConfiguration();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
