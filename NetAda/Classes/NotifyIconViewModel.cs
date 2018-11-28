using Hardcodet.Wpf.TaskbarNotification;
using NetAda.Views;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NetAda.Classes
{

    public class NotifyIconViewModel
    {

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => true,
                    CommandAction = () =>
                    {
                        if (Application.Current.MainWindow == null)
                        {

                            // ((App)Application.Current).notifyIcon.ShowCustomBalloon(PopupWindow,PopupAnimation.Slide,4000);

                            Application.Current.MainWindow = new Views.MainWindow();

                            Application.Current.MainWindow.Show();
                        }
                        else if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                        {
                            WindowNormal();
                        }
                        else if (Application.Current.MainWindow.WindowState == WindowState.Normal)
                        {
                            WindowMinimized();
                        }
                    }
                };
            }
        }


        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => WindowMinimized(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        private void WindowMinimized()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private static void WindowNormal()
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
