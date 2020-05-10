using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace PxTimer
{
    public class Model : INotifyPropertyChanged
    {
        public DispatcherTimer Timer { get; } = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private TimeSpan timeLeft;

        public TimeSpan TimeLeft
        {
            get => timeLeft;
            set
            {
                if (TimeLeft == value)
                {
                    return;
                }

                timeLeft = value;
                if (timeLeft < TimeSpan.Zero)
                {
                    timeLeft = TimeSpan.Zero;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LastMinuteStarted;

        public Model()
        {
            TimeLeft = ReadDefaultTimeLeft();

            Timer.Tick += (s, e) => TimeLeft -= Timer.Interval;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(TimeLeft) && Timer.IsEnabled)
                {
                    if (TimeLeft <= TimeSpan.Zero)
                    {
                        Timer.Stop();
                        Process.Start("shutdown", "/s /t 0");
                    }
                    else if (TimeLeft == TimeSpan.FromMinutes(1))
                    {
                        LastMinuteStarted?.Invoke(this, EventArgs.Empty);
                    }
                }
            };
        }

        public TimeSpan ReadDefaultTimeLeft()
        {
            string text = File.ReadAllText("settings.json");
            JsonDocument document = JsonDocument.Parse(text);
            JsonElement element = document.RootElement.GetProperty("defaultTimeLeftInMinutes");
            return TimeSpan.FromMinutes(element.GetInt32());
        }

        public void LowerBrightness()
        {
            try
            {
                var managementClass = new ManagementClass(@"root\WMI", "WmiMonitorBrightnessMethods", null);

                foreach (ManagementObject managementObject in managementClass.GetInstances())
                {
                    const int timeout = 1;
                    const int brightness = 0;
                    managementObject.InvokeMethod("WmiSetBrightness", new object[] { timeout, brightness });
                }
            }
            catch (ManagementException ex)
            {
                MessageBox.Show($"Lower brightness is not supported on this computer ({ex.Message.Trim()}).", "Lower brightness");
            }
        }
    }
}
