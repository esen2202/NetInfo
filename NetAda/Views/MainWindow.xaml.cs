using NetInfo;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace NetAda.Views
{

    public partial class MainWindow : Window
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
   
        public MainWindow()
        {
            InitializeComponent();

            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = 0;

            this.MaxHeight = SystemParameters.WorkArea.Height;
            this.DataContext = new ViewModels.ViewModelMainWindow();

            TopMostIconChanger();

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
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


        private void LbNetList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if(lbNetList.SelectedItem != null)
            //{
            //    var item = lbNetList.SelectedItem as AdapterObject;
            //    MessageBox.Show(item.Description);
            //} 
        }

        private void BtnShowHideList_OnClick(object sender, RoutedEventArgs e)
        {
            lbNetList.Visibility =
                lbNetList.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            ShowHideIconChanger();
        }

        private void ShowHideIconChanger()
        {
            iconShowHide.Kind = lbNetList.Visibility == Visibility.Collapsed ? MaterialDesignThemes.Wpf.PackIconKind.ArrowExpand : MaterialDesignThemes.Wpf.PackIconKind.ArrowCollapse;
        }
    }


}
