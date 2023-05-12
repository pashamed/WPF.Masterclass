
using EvernoteClone.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for Notes.xaml
    /// </summary>
    public partial class Notes : Window
    {
        NotesVM? VM;
        public Notes()
        {
            InitializeComponent();

            VM = Resources["vm"] as NotesVM;
            VM.NoteChanged += VM_NoteChanged;

            fontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontSizeComboBox.ItemsSource = new List<double>() { 8,9,10,11,12,14,16,28,48};
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if(App.CurrentUser == null)
            {
                Login loginWindow = new Login();
                loginWindow.ShowDialog();
                VM.GetNotebooks();
            }
        }

        private void VM_NoteChanged(object? sender, Model.Note e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if(e is not null)
            {
                if (!e.FileLocation.IsNullOrEmpty() && File.Exists(e.FileLocation))
                {
                    FileStream fileStream = new FileStream(e.FileLocation, FileMode.Open);
                    var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                    contents.Load(fileStream, DataFormats.Rtf);
                    fileStream.Close();
                }
            }           
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void speechButton_Click(object sender, RoutedEventArgs e)
        {
            string region = "your region";
            string key = "your key";

            var speechConfig = SpeechConfig.FromSubscription(key, region);
            using (var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                using (var recognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(speechConfig, audioConfig))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            }
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int amountOfCharacters = (new TextRange(contentRichTextBox.Document.ContentStart,contentRichTextBox.Document.ContentEnd)).Text.Length;
            statusTextBlock.Text = $"Document legth: {amountOfCharacters} characters";
        }      

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (contentRichTextBox.Selection.GetPropertyValue(FontWeightProperty).Equals(FontWeights.Bold)) 
                boldButton.IsChecked = true;
            
            if(contentRichTextBox.Selection.GetPropertyValue(FontStyleProperty).Equals(FontStyles.Italic))
                italicButton.IsChecked= true;
            else 
                italicButton.IsChecked = false;
            
            if (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline))
                underlineButton.IsChecked = true;
            else 
                underlineButton.IsChecked = false;

            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
        }

        // tooggle buttons events
        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton)?.IsChecked ?? false == true)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);

        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton)?.IsChecked ?? false == true)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection collection;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out collection);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, collection);
            }
                

        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton)?.IsChecked ?? false == true)
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, FontStyles.Normal);

        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(fontSizeComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, $"{VM.SelectedNote.Id}.rtf");
            VM.SelectedNote.FileLocation = rtfFile;
            FileStream fileStream = new FileStream(rtfFile, FileMode.Create);
            var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
            contents.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();

            VM.SaveToDb();
        }
    }
}
