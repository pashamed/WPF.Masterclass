using EvernoteClone.Model;
using EvernoteClone.View;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        DatabaseHelperContext _repository = new DatabaseHelperContext();
        MsSqlDbProvider _MsDbProvider = new MsSqlDbProvider();
        FirebaseDbProvider _FirebaseDbProvider = new FirebaseDbProvider();
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
        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                NoteChanged?.Invoke(this, this.selectedNote);
            }
        }


        private Visibility isVisible;

        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditingCommand EndEditingCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<Note> NoteChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            IsVisible = Visibility.Collapsed;
        }

        public async void CreateNotebook()
        {
            Notebook notebook = new Notebook()
            {
                Name = "Notebook",
                User = await _MsDbProvider.GetById<User>(App.CurrentUser.Id)
            };
            (bool, bool) saved = (await _MsDbProvider.Create(notebook), await _FirebaseDbProvider.Create(notebook));
            if (saved.Item1 == true && saved.Item2 == true)
            {
                //TO DO: show success status
            }
            GetNotebooks();
        }

        public async void CreateNote(Notebook notebook)
        {
            Note note = new Note()
            {
                Notebook = await _MsDbProvider.GetById<Notebook>(notebook.Id),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = $"Note for {DateTime.Now.Date.ToShortDateString()}",
                FileLocation = "location"
            };
            (bool, bool) saved = (await _MsDbProvider.Create(note), await _FirebaseDbProvider.Create(note));
            if (saved.Item1 == true && saved.Item2 == true)
            {
                //TO DO: show success status
            }
            GetNotes();
        }

        public async void GetNotebooks()
        {
            var notebooks = await _MsDbProvider.GetUserNotebooks(App.CurrentUser);
            (List<Notebook>, List<Notebook>) compare = (await _MsDbProvider.GetAll <Notebook>(), await _FirebaseDbProvider.GetAll<Notebook>());
            notebooks = compare.Item1.Where(x => x.User.Id == App.CurrentUser.Id).Except(compare.Item2.Where(x => x.User.Id == App.CurrentUser.Id)).ToList();
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

        public void StartEditing()
        {
            IsVisible = Visibility.Visible;
        }

        public void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
            _MsDbProvider.Update(notebook);
            GetNotebooks();
        }

        public async Task SaveToDb()
        {
            await _repository.SaveChangesAsync();
        }
    }
}
