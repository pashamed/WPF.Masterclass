using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class DeleteCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        NotesVM NotesVM;

        public DeleteCommand(NotesVM vM) { NotesVM = vM; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter as Notebook is not null)
            {
                NotesVM.Delete<Notebook>(parameter as Notebook);
            }
            else if (parameter as Note is not null)
            {
                NotesVM.Delete<Note>(parameter as Note);
            }

        }
    }
}
