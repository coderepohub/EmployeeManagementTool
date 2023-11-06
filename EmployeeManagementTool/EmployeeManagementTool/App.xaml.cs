using AutoMapper;
using EmployeeManagementTool.Agent;
using EmployeeManagementTool.Agent.MappingProfiles;
using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models.Confifurations;
using EmployeeManagementTool.RestClient;
using EmployeeManagementTool.RestClient.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace EmployeeManagementTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                   
                    // Configure UrlOptions using IOptions
                    services.Configure<UrlOptions>(hostContext.Configuration.GetSection(UrlOptions.Name));

                    services.AddTransient<IHttpClientProviders, HttpClientProviders>();
                    services.AddTransient<IEmployeeAgent, EmployeeAgent>();
                    services.AddTransient<IEmployeeRestClient, EmployeeRestClient>();

                    //Register Mapper
                    // Auto Mapper Configurations
                    var mapperConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new EmployeeMappingProfile());
                    });

                    IMapper mapper = mapperConfig.CreateMapper();
                    services.AddSingleton(mapper);
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
