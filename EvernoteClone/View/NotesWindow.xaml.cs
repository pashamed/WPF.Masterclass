
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
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

        public Notes()
        {
            InitializeComponent();
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
    }
}
