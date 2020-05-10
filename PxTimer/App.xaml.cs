using System.Windows;

namespace PxTimer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var model = new Model();
            var viewModel = new ViewModel(model);
            var view = new MainWindow(viewModel);
            view.Show();
        }
    }
}
