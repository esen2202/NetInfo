using NetInfo;
using System;
using System.Runtime.InteropServices;
using System.Windows;
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

        private Storyboard openStoryboard;
        private Storyboard closeStoryboard;

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
            PinnedIconChanger();
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //EnableBlur();
            openStoryboard = new Storyboard();
            closeStoryboard = new Storyboard();

            openStoryboard.Completed += OpenStoryboard_Completed;
            closeStoryboard.Completed += CloseStoryboard_Completed;
        }

        private void CloseStoryboard_Completed(object sender, EventArgs e)
        {
            borderOpenSideBar.Visibility = Visibility.Visible;
        }

        private void OpenStoryboard_Completed(object sender, EventArgs e)
        {
            borderOpenSideBar.Visibility = Visibility.Collapsed;
        }

        private void OpenCloseBorder(Action action)
        {
            action?.Invoke();
        }

        private void OpenBorderAnimation()
        {

            DoubleAnimation animation = new DoubleAnimation(borderSideBar.Width, 320, TimeSpan.FromMilliseconds(500));

            animation.EasingFunction = new PowerEase();
            openStoryboard.Children.Add(animation);

            Storyboard.SetTarget(animation, borderSideBar);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Width)"));

            openStoryboard.Begin();
        }

        private void CloseBorderAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation(borderSideBar.Width, 0, TimeSpan.FromMilliseconds(500));
            animation.EasingFunction = new PowerEase();
            closeStoryboard.Children.Add(animation);

            Storyboard.SetTarget(animation, borderSideBar);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Width)"));

            closeStoryboard.Begin();

        }

        private void BorderSideBar_OnMouseEnter(object sender, MouseEventArgs e)
        {
            closeStoryboard.Stop();
            borderOpenSideBar.Visibility = Visibility.Collapsed;
            OpenCloseBorder(OpenBorderAnimation);
        }

        private void BorderSideBar_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (borderSideBar.Width >= 320 && !pinnedSideBar)
                OpenCloseBorder(CloseBorderAnimation);

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
    }


}
