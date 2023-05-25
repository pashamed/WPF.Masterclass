using EvernoteClone.ViewModel;
using System;
using System.Windows;

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginVM? vm;

        public Login()
        {
            InitializeComponent();

            vm = Resources["vm"] as LoginVM;
            vm.Authenticated += Vm_Authenticated;
        }

        private void Vm_Authenticated(object? sender, EventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}