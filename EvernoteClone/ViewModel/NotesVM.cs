using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
    internal class NotesVM : INotifyPropertyChanged
    {
        DatabaseHelperContext _repository = new DatabaseHelperContext();
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged(nameof(SelectedNotebook));
                GetNotes();
            }
        }
       
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();
            GetNotebooks();
        }

        public void CreateNotebook()
        {
            Notebook notebook = new Notebook()
            {
                Name = "Notebook",
                User = new User
                {
                    Name = "Pavlo",
                    Lastname = "Med",
                    Username = "pashamed",
                    Password = "asda"
                }
                
            };
            _repository.Add(notebook);
            _repository.SaveChanges();
            GetNotebooks();
        }

        public void CreateNote(Notebook notebook)
        {
            Note note = new Note()
            {
                Notebook = notebook,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Note for {DateTime.Now.ToString()}",
                FileLocation = "location"
            };
            _repository.Add(note);
            _repository.SaveChanges();
            GetNotes();
        }

        private void GetNotebooks()
        {
            var notebooks = from c in _repository.Notebooks
                            select c;
            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = from c in _repository.Notes
                            where c.Notebook == SelectedNotebook
                            select c;
                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
