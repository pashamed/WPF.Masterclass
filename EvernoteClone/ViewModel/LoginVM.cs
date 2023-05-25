using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        private DatabaseHelperContext _repository;
        private LocalAuthHelper _localAuthHelper;
        private bool isShowingRegister = false;
        private User? user;

        public User User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        private Visibility loginVis;

        public Visibility LoginVis
        {
            get { return loginVis; }
            set
            {
                loginVis = value;
                OnPropertyChanged();
            }
        }

        private Visibility registerVis;

        public Visibility RegisterVis
        {
            get { return registerVis; }
            set
            {
                registerVis = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler Authenticated;

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }

        public LoginVM()
        {
            LoginVis = Visibility.Visible;
            RegisterVis = Visibility.Collapsed;

            RegisterCommand = new RegisterCommand(this);
            LoginCommand = new LoginCommand(this);
            ShowRegisterCommand = new ShowRegisterCommand(this);

            User = new User();
            _repository = new DatabaseHelperContext();
            _localAuthHelper = new LocalAuthHelper();
        }

        public void SwitchViews()
        {
            isShowingRegister = !isShowingRegister;
            if (isShowingRegister)
            {
                RegisterVis = Visibility.Visible;
                LoginVis = Visibility.Collapsed;
            }
            else
            {
                RegisterVis = Visibility.Collapsed;
                LoginVis = Visibility.Visible;
            }
        }

        public async void Login()
        {
            App.CurrentUser = await _localAuthHelper.LoginAsync(User);
            (bool,bool) logedIn = (await FirebaseAuthHelper.LoginAsync(User), App.CurrentUser is not null);
            if(logedIn.Item1 | logedIn.Item2)
            {
                Authenticated?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void RegisterAsync()
        {
            bool result = await FirebaseAuthHelper.RegisterAsync(User);
            User = App.CurrentUser;
            await _repository.AddAsync(User);
            if (result)
            {
                Authenticated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}