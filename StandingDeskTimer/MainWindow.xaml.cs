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
                        setBadgeNumber(remainingTime.Minutes);
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
                            setBadgeNumber(remainingTime.Minutes);
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
                        setBadgeNumber(remainingTime.Minutes);
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

            if (remainingTime.Seconds == 0)
            {
                    setBadgeNumber(remainingTime.Minutes);
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
            //Task.Delay(10000).ContinueWith(t => AppNotificationManager.Default.RemoveByIdAsync(toast.Id));
        }

        private void setBadgeNumber(int num)
        {
            // Get the blank badge XML payload for a badge number
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

            // Set the value of the badge in the XML to our number
            XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", num.ToString());


            // Create the badge notification
            BadgeNotification badge = new BadgeNotification(badgeXml);

            // Create the badge updater for the application
            BadgeUpdater badgeUpdater =
                BadgeUpdateManager.CreateBadgeUpdaterForApplication();

            // And update the badge
            badgeUpdater.Update(badge);

        }

        private void setPauseBadge()
        {
            string badgeGlyphValue = "paused";

            // Get the blank badge XML payload for a badge glyph
            XmlDocument badgeXml =
                BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);

            // Set the value of the badge in the XML to our glyph value
            XmlElement badgeElement =
                badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", badgeGlyphValue);

            // Create the badge notification
            BadgeNotification badge = new BadgeNotification(badgeXml);

            // Create the badge updater for the application
            BadgeUpdater badgeUpdater =
                BadgeUpdateManager.CreateBadgeUpdaterForApplication();

            // And update the badge
            badgeUpdater.Update(badge);
        }

        private void clearBadge()
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
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
    }
}
