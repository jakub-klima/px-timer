using System;
using System.Windows.Input;

namespace PxTimer
{
    public class DelegateCommand : ICommand
    {
        private readonly Action executeAction;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action executeAction)
        {
            this.executeAction = executeAction;
        }

        public void Execute(object parameter)
        {
            executeAction();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
