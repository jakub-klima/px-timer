using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PxTimer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public Model Model { get; }

        private string headerText;

        public string HeaderText
        {
            get => headerText;
            private set
            {
                headerText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeaderText)));
            }
        }

        public ICommand StepUpCommand { get; }
        public ICommand StepDownCommand { get; }
        public ICommand LowerBrightnessCommand { get; }
        public ICommand ShowDefaultTimeLeftCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Model model)
        {
            Model = model;
            StepUpCommand = new DelegateCommand(StepUp);
            StepDownCommand = new DelegateCommand(StepDown);
            LowerBrightnessCommand = new DelegateCommand(Model.LowerBrightness);
            ShowDefaultTimeLeftCommand = new DelegateCommand(ShowDefaultTimeLeft);
            HeaderText = GetHeaderText();

            Model.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Model.TimeLeft))
                {
                    HeaderText = GetHeaderText();
                }
            };
        }

        private void StepUp()
        {
            Model.TimeLeft += TimeSpan.FromMinutes(5);
        }

        private void StepDown()
        {
            Model.TimeLeft -= TimeSpan.FromMinutes(5);
        }

        private void ShowDefaultTimeLeft()
        {
            double defaultTimeLeftInMinutes = Model.ReadDefaultTimeLeft().TotalMinutes;
            MessageBox.Show($"Default time left is set to {defaultTimeLeftInMinutes} min. To change it edit settings.json file.", "Default time left");
        }

        private string GetHeaderText()
        {
            return $"Px Timer ({Model.TimeLeft})";
        }
    }
}
