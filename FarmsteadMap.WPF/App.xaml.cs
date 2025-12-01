// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.IO;
    using System.Windows;
    using FarmsteadMap.BLL.Profiles;
    using FarmsteadMap.BLL.Services;
    using FarmsteadMap.DAL;
    using FarmsteadMap.DAL.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Головний клас програми, що забезпечує логіку взаємодії.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Налаштовує хост програми (конфігурацію, сервіси, DI).
        /// </summary>
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
                        string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection") !;
                        options.UseNpgsql(connectionString);
                    });

                    services.AddAutoMapper(typeof(AutoMapperProfile));

                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<IUserAccountService, UserAccountService>();
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<LoginView>();
                    services.AddTransient<MapsMenuView>();
                    services.AddTransient<NoConnectionView>();
                    services.AddTransient<UserMapView>();
                    services.AddTransient<SettingsView>();
                })
                .Build();
        }

        /// <summary>
        /// Gets the static instance of the configured program host (IHost).
        /// Отримує статичний екземпляр налаштованого хоста програми (IHost).
        /// </summary>
        public static IHost? AppHost { get; private set; }

        /// <summary>
        /// Обробник події запуску програми.
        /// </summary>
        /// <param name="e">Аргументи запуску.</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            SplashScreenWindow splashScreen = new ();
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
                    MessageBox.Show(
                        $"Критична помилка при запуску MainWindow:\n\n{ex.Message}\n\nПеревірте InnerException: {ex.InnerException?.Message}",
                        "Помилка Dependency Injection",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            };
        }

        /// <summary>
        /// Обробник події виходу з програми.
        /// </summary>
        /// <param name="e">Аргументи виходу.</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}