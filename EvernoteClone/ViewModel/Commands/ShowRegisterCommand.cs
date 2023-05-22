using System;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class ShowRegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }

        public event EventHandler? CanExecuteChanged;

        public ShowRegisterCommand(LoginVM loginVM) => VM = loginVM;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.SwitchViews();
        }
    }
}