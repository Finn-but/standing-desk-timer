using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace StandingDeskTimer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        private Window m_window;

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        //protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        //{
        //    m_window = new MainWindow();
        //    m_window.Activate();
        //}

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            // To ensure all Notification handling happens in this process instance, register for
            // NotificationInvoked before calling Register(). Without this a new process will
            // be launched to handle the notification.
            AppNotificationManager notificationManager = AppNotificationManager.Default;
            notificationManager.NotificationInvoked += NotificationManager_NotificationInvoked;
            notificationManager.Register();

            var activatedArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();
            var activationKind = activatedArgs.Kind;
            if (activationKind != ExtendedActivationKind.AppNotification)
            {
                LaunchAndBringToForegroundIfNeeded();
            }
            else
            {
                HandleNotification((AppNotificationActivatedEventArgs)activatedArgs.Data);
            }

        }

        private void NotificationManager_NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            HandleNotification(args);
        }


        private void LaunchAndBringToForegroundIfNeeded()
        {
            if (m_window == null)
            {
                m_window = new MainWindow();
                m_window.Activate();

                // Additionally we show using our helper, since if activated via a app notification, it doesn't
                // activate the window correctly
                WindowHelper.ShowWindow(m_window);
            }
            else
            {
                WindowHelper.ShowWindow(m_window);
            }
        }

        private void HandleNotification(AppNotificationActivatedEventArgs args)
        {
            // Use the dispatcher from the window if present, otherwise the app dispatcher
            var dispatcherQueue = m_window?.DispatcherQueue ?? DispatcherQueue.GetForCurrentThread();


            dispatcherQueue.TryEnqueue(async delegate
            {
                if (args.Argument.Length == 0)
                {
                    LaunchAndBringToForegroundIfNeeded();
                    return;
                }
                switch (args.Arguments["action"])
                {
                    // Send a background message
                    case "sendMessage":
                        string message = args.UserInput["textBox"].ToString();
                        // TODO: Send it

                        // If the UI app isn't open
                        if (m_window == null)
                        {
                            // Close since we're done
                            Process.GetCurrentProcess().Kill();
                        }

                        break;

                    // View a message
                    case "viewMessage":

                        // Launch/bring window to foreground
                        LaunchAndBringToForegroundIfNeeded();

                        // TODO: Open the message
                        break;
                }
            });
        }

        private static class WindowHelper
        {
            [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool SetForegroundWindow(IntPtr hWnd);

            public static void ShowWindow(Window window)
            {
                // Bring the window to the foreground... first get the window handle...
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                // Restore window if minimized... requires DLL import above
                ShowWindow(hwnd, 0x00000009);

                // And call SetForegroundWindow... requires DLL import above
                SetForegroundWindow(hwnd);
            }
        }

    }
}
