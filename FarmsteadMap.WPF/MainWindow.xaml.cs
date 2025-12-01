// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Головне вікно програми, яке виступає в ролі хоста для різних UserControl (View).
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider serviceProvider;
        private LoginView? loginView;
        private MapsMenuView? mapsMenuView;
        private NoConnectionView? noConnectionView;
        private UserMapView? userMapView;
        private string? currentUsername;
        private long currentUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="serviceProvider">Постачальник сервісів (DI).</param>
        public MainWindow(IServiceProvider serviceProvider)
        {
            this.InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.Loaded += this.MainWindow_Loaded;
        }

        private static async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = System.TimeSpan.FromSeconds(3);
                var response = await client.GetAsync("http://clients3.google.com/generate_204");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await this.CheckConnectionAndNavigate();
        }

        private async Task CheckConnectionAndNavigate()
        {
            bool hasInternet = await CheckInternetConnectionAsync();
            if (hasInternet)
            {
                this.ShowLoginView();
            }
            else
            {
                this.ShowNoConnectionView();
            }
        }

        private void ShowLoginView()
        {
            if (this.loginView == null)
            {
                this.loginView = this.serviceProvider.GetRequiredService<LoginView>();
                this.loginView.LoginSuccess += this.OnLoginSuccess;
            }

            this.MainContentHost.Content = this.loginView;
        }

        private void ShowMapsMenuView(long userId, string username)
        {
            if (this.mapsMenuView == null)
            {
                this.mapsMenuView = this.serviceProvider.GetRequiredService<MapsMenuView>();
                this.mapsMenuView.LogoutRequest += this.OnLogoutRequest;
                this.mapsMenuView.RequestNavigation += this.OnRequestNavigation;
            }

            this.mapsMenuView.SetCurrentUser(userId, username);
            this.MainContentHost.Content = this.mapsMenuView;
        }

        private void OnRequestNavigation(object? sender, NavigateEventArgs e)
        {
            if (e.TargetView == "UserMapView")
            {
                this.ShowUserMapView(e.Data as int?);
            }
        }

        private void ShowUserMapView(int? mapId)
        {
            if (!mapId.HasValue)
            {
                return;
            }

            if (this.userMapView == null)
            {
                this.userMapView = new UserMapView();
                this.userMapView.RequestNavigateBack += this.OnRequestNavigateBackFromMap;
                this.userMapView.RequestLogout += this.OnLogoutRequest;
            }

            this.userMapView.LoadMap(mapId.Value, this.currentUsername!);
            this.MainContentHost.Content = this.userMapView;
        }

        private void OnRequestNavigateBackFromMap(object? sender, EventArgs e)
        {
            this.MainContentHost.Content = this.mapsMenuView;
        }

        private void ShowNoConnectionView()
        {
            if (this.noConnectionView == null)
            {
                this.noConnectionView = new NoConnectionView();
                this.noConnectionView.RetryClicked += this.OnRetryClicked;
            }

            this.MainContentHost.Content = this.noConnectionView;
        }

        private void OnLoginSuccess(object? sender, LoginSuccessEventArgs e)
        {
            this.currentUsername = e.Username;
            this.currentUserId = e.UserId;
            this.ShowMapsMenuView(this.currentUserId, this.currentUsername);
        }

        private async void OnLogoutRequest(object? sender, System.EventArgs e)
        {
            await this.CheckConnectionAndNavigate();
        }

        private async void OnRetryClicked(object? sender, System.EventArgs e)
        {
            await this.CheckConnectionAndNavigate();
        }
    }
}