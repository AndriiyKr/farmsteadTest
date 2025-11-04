using FarmsteadMap.BLL.Profiles;
using FarmsteadMap.BLL.Services;
using FarmsteadMap.DAL;
using FarmsteadMap.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FarmsteadMap.WPF
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("config.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                        options.UseNpgsql(connectionString);
                    });

                    services.AddAutoMapper(typeof(AutoMapperProfile));

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IAuthService, AuthService>();

                    services.AddSingleton<MainWindow>();
                    services.AddTransient<LoginView>();
                    services.AddTransient<MapsMenuView>();
                    services.AddTransient<NoConnectionView>();
                    services.AddTransient<UserMapView>();
                    services.AddTransient<SettingsView>();

                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();

            splashScreen.Closed += (s, args) =>
            {
                try
                {
                    var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
                    mainWindow.Closed += (s_main, args_main) =>
                    {
                        Application.Current.Shutdown();
                    };
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Критична помилка при запуску MainWindow:\n\n{ex.Message}\n\nПеревірте InnerException: {ex.InnerException?.Message}",
                                    "Помилка Dependency Injection",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            };
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }

}