using DesktopContacts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopContacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ContactContext _contactContext = new ContactContext();
        private List<Contact> _contacts = new List<Contact>();

        public MainWindow()
        {
            InitializeComponent();
            ReadDatabase();
            Closing += (s, e) => _contactContext.DisposeAsync();
        }

        private async void ReadDatabase()
        {
            _contacts = await _contactContext.Contacts.ToListAsync();
            if(_contacts is not null)
            {
                _contacts =  _contacts.OrderBy(c => c.Name).OrderBy(c => c.Name).ToList();
                contactsListView.ItemsSource = _contacts;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();

            ReadDatabase();
        }

        private void SearchBoxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? searchTextBox = sender as TextBox;
            var searchedContacts = _contacts.Where(c => c.Name.ToLower().Contains(searchTextBox.Text.ToLower())).ToList();
            contactsListView.ItemsSource = searchedContacts;
        }

        private void contactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedContact = contactsListView.SelectedItem as Contact;
            if(selectedContact != null)
            {
                ContactDetails contactDetailsWindows = new ContactDetails(selectedContact);
                contactDetailsWindows.ShowDialog();
                ReadDatabase();
            }
        }
    }
}
