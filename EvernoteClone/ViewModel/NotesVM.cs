using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        private DatabaseHelperContext _repository = new DatabaseHelperContext();
        private MsSqlDbProvider _MsDbProvider = new MsSqlDbProvider();
        private FirebaseDbProvider _FirebaseDbProvider = new FirebaseDbProvider();
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

        private Visibility isNotebookVisible;

        public Visibility IsNotebookVisible
        {
            get { return isNotebookVisible; }
            set
            {
                isNotebookVisible = value;
                OnPropertyChanged(nameof(isNotebookVisible));
            }
        }

        private Visibility isNoteVisible;

        public Visibility IsNoteVisible
        {
            get { return isNoteVisible; }
            set
            {
                isNoteVisible = value;
                OnPropertyChanged(nameof(IsNoteVisible));
            }
        }



        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditingCommand EndEditingCommand { get; set; }
        public DeleteCommand DeleteCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<Note> NoteChanged;

        public NotesVM()
        {
            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);
            DeleteCommand = new DeleteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            IsNotebookVisible = Visibility.Collapsed;
            IsNoteVisible = Visibility.Collapsed;
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
                //TODO: show success status
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
            (List<Notebook>, List<Notebook>) compare = (await _MsDbProvider.GetAll<Notebook>(), await _FirebaseDbProvider.GetAll<Notebook>());
            var notebooks = compare.Item1.Where(x => x.User.Id == App.CurrentUser.Id).Except(compare.Item2.Where(x => x.User.Id == App.CurrentUser.Id)).ToList();
            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private async void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                (List<Note>, List<Note>) compare = (await _MsDbProvider.GetAll<Note>(), await _FirebaseDbProvider.GetAll<Note>());
                var notes = compare.Item1.Where(x => x.Notebook.Id == selectedNotebook.Id).Except(compare.Item2.Where(x => x.Notebook.Id == selectedNotebook.Id)).ToList();
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

        public void StartEditing(object obj)
        {
            if (obj.GetType().Name == "NotebookProxy" | obj is Notebook)
            {
                IsNotebookVisible = Visibility.Visible;
            }
            if (obj.GetType().Name == "NoteProxy" | obj is Note)
            {
                IsNoteVisible = Visibility.Visible;
            }
        }

        public async void StopEditing<T>(T entity) where T : class, IHasId<string>
        {
            IsNotebookVisible = Visibility.Collapsed;
            IsNoteVisible = Visibility.Collapsed;
            await _MsDbProvider.Update<T>(entity);
            await _FirebaseDbProvider.Update<T>(entity);
            GetNotebooks();
        }

        public async void Delete<T>(T entity) where T : class, IHasId<string>
        {
            await _MsDbProvider.Delete<T>(entity);
            await _FirebaseDbProvider.Delete<T>(entity);
            GetNotebooks();
            GetNotes();
        }

        public async Task SaveToDb()
        {
            await _repository.SaveChangesAsync();
        }
    }
}