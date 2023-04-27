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
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window
    {
        private readonly ContactContext _contactContext = new ContactContext();

        public NewContactWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Closing += (s, e) => _contactContext.DisposeAsync();
        }

        private async void save_Button_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = new Contact()
            {
                Email = emailTextBox.Text,
                Name = nameTextBox.Text,
                Phone = phoneTextBox.Text,
            };
            _contactContext.Contacts.Add(contact);
            await _contactContext.SaveChangesAsync();
            Close();
        }

        private void contactTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Clear();
        }
    }
}
