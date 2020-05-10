using System.Windows;

namespace PxTimer
{
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            viewModel.Model.LastMinuteStarted += (sender, e) =>
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }
                Activate();
            };
        }
    }
}
