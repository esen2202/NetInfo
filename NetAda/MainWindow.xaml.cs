using NetInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetAda
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Acrylic
        internal enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        #endregion

        private static AdapterInfo adapterInfo;

        private ListAdapter listAdapter;

        public ListAdapter ListAdapter 
        {
            get { return listAdapter; }
            set { listAdapter = value; }
        }

        private void ListAdapter_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChangedEvent("ListAdapter");
        }

        private void PropertyChangedEvent(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;



        private void GetAdapterList()
        {
            if (adapterInfo != null) adapterInfo.RefreshInfos();

            adapterInfo = AdapterInfo.CreateInstance();
            ListAdapter = new ListAdapter();

            adapterInfo.listAdapter.ForEach(adapter =>
            {
                var adp = ListAdapter.Where(x => x.Description == adapter.Description).FirstOrDefault();
                if (adp != null)
                {
                    adp.IsDHCPEnabled = adapter.IsDHCPEnabled;
                }
                else
                {
                    ListAdapter.Add(adapter);
                }
               
            });

        }

        public MainWindow()
        {
            InitializeComponent();

            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = 0;

            this.DataContext = this;

            GetAdapterList();

            TopMostIconChanger();
        }



        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            EnableBlur();
        }

        private void BtnNetConnections_Click(object sender, RoutedEventArgs e)
        {
            AdapterSettings.StartNetworkConnections();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetAdapterList();
           

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAlwaysTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
            TopMostIconChanger();
        }

        private void TopMostIconChanger()
        {
            iconPin.Kind = this.Topmost ? MaterialDesignThemes.Wpf.PackIconKind.PinOutline : MaterialDesignThemes.Wpf.PackIconKind.PinOffOutline;
        }
    }

    public class ListAdapter : ObservableCollection<AdapterObject>
    {
        public ListAdapter() : base()
        {

        }
    }
}
