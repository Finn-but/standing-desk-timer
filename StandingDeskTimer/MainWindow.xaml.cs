using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using System.Threading;
using Windows.Graphics;
using Microsoft.UI;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace StandingDeskTimer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private TimeSpan remainingTripleTwentyTime;
        private TimeSpan remainingTripleTwentyAwayTime;
        private ITaskbarList3 _taskbar;
        private IntPtr _hwnd;

        private Brush ActiveColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 49, 181, 91));
        private Brush InactiveColor;
        private Brush PauseColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 249, 199, 79));
        private Brush PlayColor;

        private bool _isTimerRunning;
        public bool IsTimerRunning
        {
            get { return _isTimerRunning; }
            set { 
                if (_isTimerRunning != value)
                {
                    _isTimerRunning = value;
                    remainingTripleTwentyTime = TimeSpan.FromMinutes(20);
                    remainingTripleTwentyAwayTime = TimeSpan.FromSeconds(0);

                    if (IsStanding)
                    {
                        remainingTime = TimeSpan.FromMinutes(StandingValue);
                        ProgressBar1.Maximum = StandingValue;
                        SetSlider(StandingValue);
                    }
                    else
                    {
                        remainingTime = TimeSpan.FromMinutes(SittingValue);
                        ProgressBar1.Maximum = SittingValue;
                        SetSlider(SittingValue);
                    }
                    SittingTime.Text = TimeSpan.FromMinutes(SittingValue).ToString(@"mm\:ss");
                    StandingTime.Text = TimeSpan.FromMinutes(StandingValue).ToString(@"mm\:ss");

                    if (_isTimerRunning) {
                        PlayButton.Content = "Stop";
                        ActivePanel.Visibility = Visibility.Visible;
                        InactivePanel.Visibility = Visibility.Collapsed;
                        PauseButton.IsEnabled = true;

                        //Needs to be checked
                        timer.Start();
                        setBadgeNumber((int)remainingTime.TotalMinutes);
                        ProgressBar1.Visibility = Visibility.Visible;
                    } else
                    {
                        PlayButton.Content = "Start";
                        ActivePanel.Visibility = Visibility.Collapsed;
                        InactivePanel.Visibility = Visibility.Visible;
                        PauseButton.IsEnabled = false;

                        //Needs to be checked
                        timer.Stop();
                        clearBadge();
                        ProgressBar1.Visibility = Visibility.Collapsed;
                    }
                    
                    IsPaused = false;
                }
            }
        }

        private bool _isPaused;
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    if (_isPaused)
                    {
                        timer.Stop();
                        PauseButton.Content = "Resume";
                        ProgressBar1.Foreground = PauseColor;
                        setPauseBadge();
                    } else
                    {
                        timer.Start();
                        PauseButton.Content = "Pause";
                        ProgressBar1.Foreground = PlayColor;
                        if (IsTimerRunning)
                        {
                            setBadgeNumber((int)Math.Ceiling(remainingTime.TotalMinutes));
                        }
                    }
                }
            }
        }

        private bool _isStanding = true;
        public bool IsStanding
        {
            get { return _isStanding; }
            set
            {
                if (_isStanding != value)
                {
                    _isStanding = value;

                    if (_isStanding)
                    {
                        ProgressBar1.Maximum = StandingValue;
                        SetSlider(StandingValue);
                        Title = "Standing";
                        StandingImage.Visibility = Visibility.Visible;
                        StandingTime.Foreground = ActiveColor;

                        SittingImage.Visibility = Visibility.Collapsed;
                        SittingTime.Foreground = InactiveColor;
                        SittingTime.Text = TimeSpan.FromMinutes(SittingValue).ToString(@"mm\:ss");

                        remainingTime = TimeSpan.FromMinutes(StandingValue);
                    }
                    else
                    {
                        ProgressBar1.Maximum = SittingValue;
                        SetSlider(SittingValue);
                        Title = "Sitting";
                        StandingImage.Visibility = Visibility.Collapsed;
                        StandingTime.Foreground = InactiveColor;
                        StandingTime.Text = TimeSpan.FromMinutes(StandingValue).ToString(@"mm\:ss");

                        SittingImage.Visibility = Visibility.Visible;
                        SittingTime.Foreground = ActiveColor;

                        remainingTime = TimeSpan.FromMinutes(SittingValue);
                    }
                    if (!IsPaused && IsTimerRunning)
                    {
                        setBadgeNumber((int)remainingTime.TotalMinutes);
                    }
                }
            }
        }

        public int StandingValue {
            get { return (int)localSettings.Values["standingValue"]; } 
            set {
                if ((int)localSettings.Values["standingValue"] != value)
                {
                    localSettings.Values["standingValue"] = value;
                }
            } 
        }

        public int SittingValue
        {
            get { return (int)localSettings.Values["sittingValue"]; }
            set
            {
                if ((int)localSettings.Values["sittingValue"] != value)
                {
                    localSettings.Values["sittingValue"] = value;
                }
            }
        }

        private bool _isDragging;
        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {
                if (_isDragging != value)
                {
                    _isDragging = value;
                }
            }
        }

        public bool EnableTripleTwenty
        {
            get { return (bool)localSettings.Values["tripleTwentyValue"]; }
            set
            {
                if ((bool)localSettings.Values["tripleTwentyValue"] != value)
                {
                    localSettings.Values["tripleTwentyValue"] = value;
                }
            }
        }


        public MainWindow()
        {
            Title = "Standing";
            this.AppWindow.SetIcon(Path.Combine(Package.Current.InstalledLocation.Path, "assets/stand.ico"));
            this.AppWindow.Resize(new(1000, 600));

            if (!localSettings.Values.ContainsKey("sittingValue"))
            {
                localSettings.Values["sittingValue"] = 30;
            }

            if (!localSettings.Values.ContainsKey("standingValue"))
            {
                localSettings.Values["standingValue"] = 30;
            }

            if (!localSettings.Values.ContainsKey("tripleTwentyValue"))
            {
                localSettings.Values["tripleTwentyValue"] = false;
            }

            // Initialize timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            remainingTime = TimeSpan.FromMinutes(StandingValue);

            this.InitializeComponent();

            StandingTime.Text = TimeSpan.FromMinutes(StandingValue).ToString(@"mm\:ss");
            SittingTime.Text = TimeSpan.FromMinutes(SittingValue).ToString(@"mm\:ss");
            InactiveColor = SittingTime.Foreground;
            PlayColor = ProgressBar1.Foreground;

            _hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            _taskbar = (ITaskbarList3)new TaskbarInstance();
            _taskbar.HrInit();

            clearBadge();

        }

        private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            IsTimerRunning = !IsTimerRunning;
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            IsStanding = !IsStanding;
        }
        
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            IsPaused = !IsPaused;
        }

        private void Timer_Tick(object sender, object e)
        {
            if (IsDragging)
            {
                return;
            }
            // Update the remaining time
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

            if (remainingTime.Seconds == 0 && remainingTime.Minutes > 0)
            {
                    Debug.WriteLine($"[Badge] Tick condition hit: {remainingTime}, calling setBadgeNumber({(int)remainingTime.TotalMinutes})");
                    setBadgeNumber((int)remainingTime.TotalMinutes);
            }

            // Update the countdown display
            if (IsStanding)
            {
                StandingTime.Text = remainingTime.ToString(@"mm\:ss");
            } else
            {
                SittingTime.Text = remainingTime.ToString(@"mm\:ss");
            }
            SetSlider(remainingTime.TotalMinutes);

            // Check if the countdown has reached zero
            if (remainingTime <= TimeSpan.Zero)
            {
                if (IsStanding)
                {
                    SendNotificationToast("Sit down!");
                } else
                {
                    SendNotificationToast("Stand up!");
                }
                IsStanding = !IsStanding;
            }

            if (EnableTripleTwenty)
            {

                if (remainingTripleTwentyTime > TimeSpan.Zero)
                {
                    remainingTripleTwentyTime = remainingTripleTwentyTime.Subtract(TimeSpan.FromSeconds(1));
                    if (remainingTripleTwentyTime == TimeSpan.Zero)
                    {
                        remainingTripleTwentyAwayTime = TimeSpan.FromSeconds(20);
                        ShowTripleTwenty();
                    }
                } else if (remainingTripleTwentyAwayTime > TimeSpan.Zero)
                {
                    remainingTripleTwentyAwayTime = remainingTripleTwentyAwayTime.Subtract(TimeSpan.FromSeconds(1));
                    UpdateTripleTwenty(20 - remainingTripleTwentyAwayTime.Seconds);
                    if (remainingTripleTwentyAwayTime == TimeSpan.Zero)
                    {
                        remainingTripleTwentyTime = TimeSpan.FromMinutes(20);
                    }
                }
            }
        }

        private async static void SendNotificationToast(string message)
        {
            await AppNotificationManager.Default.RemoveAllAsync();
            var toast = new AppNotificationBuilder()
                .AddText(message)
                .SetScenario(AppNotificationScenario.Reminder)
                .AddButton(new AppNotificationButton("Dismiss")
                .AddArgument("action", "dismiss"))
                .BuildNotification();
            
            AppNotificationManager.Default.Show(toast);
        }

        private void setBadgeNumber(int num)
        {
            try
            {
                using var icon = CreateBadgeNumberIcon(num);
                IntPtr hIcon = icon.GetHicon();
                _taskbar.SetOverlayIcon(_hwnd, hIcon, num.ToString());
                DestroyIcon(hIcon);
                Debug.WriteLine($"[Badge] setBadgeNumber({num}) succeeded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Badge] setBadgeNumber({num}) failed: {ex}");
            }
        }

        private System.Drawing.Bitmap CreateBadgeNumberIcon(int num)
        {
            int dpi = GetDpiForWindow(_hwnd);
            int size = Math.Max(16, dpi * 16 / 96); // scale with display DPI
            var bmp = new System.Drawing.Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using var g = System.Drawing.Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(System.Drawing.Color.Transparent);
            using var bgBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 225, 38, 46));
            g.FillEllipse(bgBrush, 0, 0, size - 1, size - 1);
            string text = num > 99 ? "99+" : num.ToString();
            float fontSize = size * (num > 9 ? 0.55f : 0.68f);
            using var font = new System.Drawing.Font("Segoe UI", fontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            var sf = new System.Drawing.StringFormat
            {
                Alignment = System.Drawing.StringAlignment.Center,
                LineAlignment = System.Drawing.StringAlignment.Center,
                FormatFlags = System.Drawing.StringFormatFlags.NoWrap | System.Drawing.StringFormatFlags.NoClip
            };
            g.DrawString(text, font, System.Drawing.Brushes.White, new System.Drawing.RectangleF(0, 0, size, size), sf);
            return bmp;
        }

        private void setPauseBadge()
        {
            try
            {
                int dpi = GetDpiForWindow(_hwnd);
                int sz = Math.Max(16, dpi * 16 / 96);
                using var bmp = new System.Drawing.Bitmap(sz, sz, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using var g = System.Drawing.Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(System.Drawing.Color.Transparent);
                using var bgBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 249, 199, 79));
                g.FillEllipse(bgBrush, 0, 0, sz - 1, sz - 1);
                using var barBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 80, 60, 0));
                int bx = sz / 4; int bw = sz / 5; int by = (int)(sz * 0.27f); int bh = (int)(sz * 0.46f);
                g.FillRectangle(barBrush, bx, by, bw, bh);
                g.FillRectangle(barBrush, sz - bx - bw, by, bw, bh);
                IntPtr hIcon = bmp.GetHicon();
                _taskbar.SetOverlayIcon(_hwnd, hIcon, "Paused");
                DestroyIcon(hIcon);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Badge] setPauseBadge failed: {ex}");
            }
        }

        private void clearBadge()
        {
            try
            {
                _taskbar?.SetOverlayIcon(_hwnd, IntPtr.Zero, null);
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
            }
            catch (Exception ex) { Debug.WriteLine($"[Badge] clearBadge failed: {ex}"); }
        }

        private bool isProgrammaticChange = false;
        private void SetSlider(double value)
        {
            isProgrammaticChange = true;
            ProgressBar1.Value = value;
            isProgrammaticChange = false;
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!isProgrammaticChange)
            {
                remainingTime = TimeSpan.FromMinutes(Math.Round(e.NewValue));
                if (IsStanding)
                {
                    StandingTime.Text = remainingTime.ToString(@"mm\:ss");
                }
                else
                {
                    SittingTime.Text = remainingTime.ToString(@"mm\:ss");
                }
                if (!IsPaused)
                {
                    setBadgeNumber((int)remainingTime.TotalMinutes);
                }
            }
        }

        private void Slider_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            IsDragging = true;
        }

        private void Slider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            IsDragging = false;
        }

        private void ShowTripleTwenty()
        {
            var notification = new AppNotificationBuilder()
                .AddText("Look at an object 6m away!")
                .SetScenario(AppNotificationScenario.Reminder)
                .AddProgressBar(new AppNotificationProgressBar()
                    .BindStatus()
                    .BindValue()
                    .BindValueStringOverride())
                 .SetTag("TripleTwenty")
                 .SetGroup("TripleTwenty")
                .BuildNotification();

            var data = new AppNotificationProgressData(1);
            data.Value = (double)(0 / 20); // Binds to {progressValue} in xml payload
            data.ValueStringOverride = String.Format("{0}/{1} s", 0, 20); // Binds to {progressValueString} in xml payload
            data.Status = " "; // Binds to {progressStatus} in xml payload

            notification.Progress = data;

            AppNotificationManager.Default.Show(notification);
        }

        private async void UpdateTripleTwenty(int value)
        {
                int total = 20;
                var data = new AppNotificationProgressData(1);
                data.Value = (double)value / total; // Binds to {progressValue} in xml payload
                data.ValueStringOverride = String.Format("{0}/{1} s", value, total); // Binds to {progressValueString} in xml payload
                data.Status = " "; // Binds to {progressStatus} in xml payload
                await AppNotificationManager.Default.UpdateAsync(data, "TripleTwenty", "TripleTwenty");

            if (value == 20)
            {
                await AppNotificationManager.Default.RemoveByTagAndGroupAsync("TripleTwenty", "TripleTwenty");
            }
        }

        [DllImport("user32.dll")] private static extern bool DestroyIcon(IntPtr hIcon);
        [DllImport("user32.dll")] private static extern int GetDpiForWindow(IntPtr hwnd);

        [ComImport, Guid("56FDF344-FD6D-11d0-958A-006097C9A090"), ClassInterface(ClassInterfaceType.None)]
        private class TaskbarInstance { }

        [ComImport, Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskbarList3
        {
            [PreserveSig] void HrInit();
            [PreserveSig] void AddTab(IntPtr hwnd);
            [PreserveSig] void DeleteTab(IntPtr hwnd);
            [PreserveSig] void ActivateTab(IntPtr hwnd);
            [PreserveSig] void SetActiveAlt(IntPtr hwnd);
            [PreserveSig] void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
            [PreserveSig] void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
            [PreserveSig] void SetProgressState(IntPtr hwnd, int tbpFlags);
            [PreserveSig] void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);
            [PreserveSig] void UnregisterTab(IntPtr hwndTab);
            [PreserveSig] void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);
            [PreserveSig] void SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);
            [PreserveSig] void ThumbBarAddButtons(IntPtr hwnd, uint cButtons, IntPtr pButton);
            [PreserveSig] void ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, IntPtr pButton);
            [PreserveSig] void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);
            [PreserveSig] void SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
        }
    }
}
