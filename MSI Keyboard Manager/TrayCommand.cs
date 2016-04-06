using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MSI_Keyboard_Manager
{
    public class TrayCommand : ICommand
    {
        private MainWindow _window;

        public TrayCommand(MainWindow window)
        {
            _window = window;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _window.Visibility = Visibility.Visible;
            _window.NotifyIcon.Visibility = Visibility.Collapsed;
        }

        public event EventHandler CanExecuteChanged;
    }
}
