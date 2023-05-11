using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RegisterCommand(LoginVM vM) => VM = vM;

        public bool CanExecute(object? parameter)
        {
            User? user = parameter as User;

            if (parameter as User == null) return false;
            if (string.IsNullOrEmpty(user.Username)) return false;
            if (string.IsNullOrEmpty(user.Password)) return false;
            if (string.IsNullOrEmpty(user.ConfirmPassword)) return false;
            if (user.Password != user.ConfirmPassword) return false;
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.RegisterAsync();
        }
    }
}
