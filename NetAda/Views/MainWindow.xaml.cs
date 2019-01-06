using NetInfo;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;

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

        private Storyboard openSideBarStoryboard, closeSideBarStoryboard;
        private Storyboard openConfigurationStoryboard, closeConfigurationStoryboard;
    

        private bool pinnedSideBar = false;

        public MainWindow()
        {
            InitializeComponent();

            //this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - this.Width;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;

            this.DataContext = new ViewModels.ViewModelMainWindow();

            this.Topmost = true;

            borderConfiguration.Width = 0;
            PinnedIconChanger();

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //EnableBlur();
            openSideBarStoryboard = new Storyboard();
            closeSideBarStoryboard = new Storyboard();
            openConfigurationStoryboard = new Storyboard();
            closeConfigurationStoryboard = new Storyboard();

            openSideBarStoryboard.Completed += OpenStoryboard_Completed;
            closeSideBarStoryboard.Completed += CloseStoryboard_Completed;
            openConfigurationStoryboard.Completed += OpenConfigurationStoryboard_Completed;
            closeConfigurationStoryboard.Completed += CloseConfigurationStoryboard_Completed;
        }

        private void CloseConfigurationStoryboard_Completed(object sender, EventArgs e)
        {
             
        }

        private void OpenConfigurationStoryboard_Completed(object sender, EventArgs e)
        {
             
        }

        private void CloseStoryboard_Completed(object sender, EventArgs e)
        {
            borderOpenSideBar.Visibility = Visibility.Visible;
        }

        private void OpenStoryboard_Completed(object sender, EventArgs e)
        {
            borderOpenSideBar.Visibility = Visibility.Collapsed;
        }

        private void BorderWidthAnimation(Border border, Storyboard storyboard, Double widthStart, Double widthFinish)
        {
            DoubleAnimation animation = new DoubleAnimation(widthStart, widthFinish, TimeSpan.FromMilliseconds(500));
            animation.EasingFunction = new PowerEase();
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Width)"));

            storyboard.Begin();
        }

        private void BorderSideBar_OnMouseEnter(object sender, MouseEventArgs e)
        {
            closeSideBarStoryboard.Stop();
            borderOpenSideBar.Visibility = Visibility.Collapsed;
            BorderWidthAnimation(borderSideBar, openSideBarStoryboard, borderSideBar.Width, 320);
        }

        private void BorderSideBar_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (borderSideBar.Width >= 320 && !pinnedSideBar && borderConfiguration.Width == 0)
                BorderWidthAnimation(borderSideBar, closeSideBarStoryboard, borderSideBar.Width, 0);

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
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
            BorderWidthAnimation(borderConfiguration, openConfigurationStoryboard, borderConfiguration.Width, 320);
        }

        private void BtnAlwaysTop_Click(object sender, RoutedEventArgs e)
        {
            pinnedSideBar = !pinnedSideBar;
            PinnedIconChanger();
        }

        private void PinnedIconChanger()
        {
            iconPin.Kind = pinnedSideBar ? MaterialDesignThemes.Wpf.PackIconKind.PinOutline : MaterialDesignThemes.Wpf.PackIconKind.PinOffOutline;
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
            if (lbNetList.Visibility == Visibility.Collapsed)
            {
                lbNetList.Visibility = Visibility.Visible;
                this.SizeToContent = SizeToContent.Manual;
                this.Height = SystemParameters.WorkArea.Height;
            }
            else
            {
                this.SizeToContent = SizeToContent.Height;
                lbNetList.Visibility = Visibility.Collapsed;
            }

            ShowHideIconChanger();
        }

        private void ShowHideIconChanger()
        {
            iconShowHide.Kind = lbNetList.Visibility == Visibility.Collapsed ? MaterialDesignThemes.Wpf.PackIconKind.ArrowExpand : MaterialDesignThemes.Wpf.PackIconKind.ArrowCollapse;
        }

        private void BtnCopyGlobalIP_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxbGlobalIP.Text);
        }

        private void BtnHideConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            BorderWidthAnimation(borderConfiguration, closeConfigurationStoryboard, borderConfiguration.Width, 0);
        }
    }


}
