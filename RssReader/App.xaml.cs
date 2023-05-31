using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RssReader.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RssReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        public App()
        {           
            AppHost = Host.CreateDefaultBuilder().ConfigureServices(
                services =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<IRssHelper, RssHelper>();
                    //services.AddTransient<IRssHelper, FakeRssHelper>();
                }).Build();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startUpForm = AppHost!.Services.GetRequiredService<MainWindow>();
            startUpForm.DataContext = new MainVM(AppHost!.Services.GetService<IRssHelper>());
            startUpForm.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppHost?.StopAsync();
            base.OnExit(e);
        }
    }
}
