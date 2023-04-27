using DesktopContacts.Models;
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
using System.Windows.Shapes;

namespace DesktopContacts
{
    /// <summary>
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetails : Window
    {
        private Contact? _contact;
        private readonly ContactContext _contactContext = new ContactContext();

        public ContactDetails(Contact contact)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _contact = contact;
            contactsListView.ItemsSource = new List<Contact>() { _contact };
            Closing += (s, e) =>
            {
                _contactContext.SaveChangesAsync();
                _contactContext.DisposeAsync();
            };
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        { 
            _contactContext.Contacts.Update(_contact);
            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            _contactContext.Contacts.Remove(_contact);
            _contactContext.SaveChangesAsync();
            Close();
        }
    }
}
