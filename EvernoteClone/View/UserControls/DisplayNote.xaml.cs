using EvernoteClone.Model;
using System.Windows;
using System.Windows.Controls;

namespace EvernoteClone.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayNote.xaml
    /// </summary>
    public partial class DisplayNote : UserControl
    {
        public Note Note
        {
            get { return (Note)GetValue(NotebookProperty); }
            set { SetValue(NotebookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookProperty =
            DependencyProperty.Register(nameof(Note), typeof(Note), typeof(DisplayNote), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayNote? noteUserControl = (d as DisplayNote);
            noteUserControl.DataContext ??= (d as DisplayNote)?.Note;
        }

        public DisplayNote()
        {
            InitializeComponent();
        }
    }
}