using LandmarkAI.POCO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LandmarkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png, *.jpg)|*.png; *.jpg;*.jpeg|All files (*.*)| *.*";
            dialog.InitialDirectory = "D:\\Torrents\\Windows Presentation Foundation Masterclass\\09. REST and AI\\3.1 Resources - REST and AI - Training the AI\\WPF-1040-TrainAI-resources";
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                selectedImage.Source = new BitmapImage(new Uri(fileName));
                MakePrediction(fileName);
            }
        }

        private async void MakePrediction(string fileName)
        {
            using HttpClient client = new HttpClient();
            var uri = new Uri("https://northeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/72a617ec-b8be-4983-9edd-066e44ea0994/classify/iterations/Initial%20Iteration/image");
            client.DefaultRequestHeaders.Add("Prediction-Key", "02f8080593ed464ba832a1c493cc3514");
            var image = File.ReadAllBytes(fileName);

            using var content = new ByteArrayContent(image);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            var response = await client.PostAsync(uri, content);
            var responseStream = await response.Content.ReadAsStreamAsync();
            predictionsListView.ItemsSource = (JsonSerializer.Deserialize<CustomVision>(responseStream)).predictions;
        }
    }
}
