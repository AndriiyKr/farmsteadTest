using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace FarmsteadMap.WPF
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private LoginView _loginView;
        private MapsMenuView _mapsMenuView;
        private NoConnectionView _noConnectionView;
        private UserMapView _userMapView;
        private string _currentUsername;
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckConnectionAndNavigate();
        }

        private async Task CheckConnectionAndNavigate()
        {
            bool hasInternet = await CheckInternetConnectionAsync();
            if (hasInternet)
            {
                ShowLoginView();
            }
            else
            {
                ShowNoConnectionView();
            }
        }

        private void ShowLoginView()
        {
            if (_loginView == null)
            {
                _loginView = _serviceProvider.GetRequiredService<LoginView>();
                _loginView.LoginSuccess += OnLoginSuccess; 
            }
            MainContentHost.Content = _loginView;
        }

        private void ShowMapsMenuView(string username)
        {
            if (_mapsMenuView == null)
            {
                _mapsMenuView = new MapsMenuView();
                _mapsMenuView.LogoutRequest += OnLogoutRequest;
                _mapsMenuView.RequestNavigation += OnRequestNavigation;
            }
            _mapsMenuView.Username = username;
            MainContentHost.Content = _mapsMenuView;
        }

        private void OnRequestNavigation(object sender, NavigateEventArgs e)
        {
            if (e.TargetView == "UserMapView")
            {
                ShowUserMapView(e.Data as int?);
            }
        }

        private void ShowUserMapView(int? mapId)
        {
            if (!mapId.HasValue) return; 

            if (_userMapView == null)
            {
                _userMapView = new UserMapView();
                _userMapView.RequestNavigateBack += OnRequestNavigateBackFromMap;
            }

            _userMapView.LoadMap(mapId.Value, _currentUsername); 
            MainContentHost.Content = _userMapView; 
        }

        private void OnRequestNavigateBackFromMap(object sender, EventArgs e)
        {
            MainContentHost.Content = _mapsMenuView;
        }

        private void ShowNoConnectionView()
        {
            if (_noConnectionView == null)
            {
                _noConnectionView = new NoConnectionView();
                _noConnectionView.RetryClicked += OnRetryClicked; 
            }
            MainContentHost.Content = _noConnectionView;
        }

        private void OnLoginSuccess(object sender, LoginSuccessEventArgs e)
        {
            _currentUsername = e.Username;
            ShowMapsMenuView(e.Username);
        }

        private void OnLogoutRequest(object sender, System.EventArgs e)
        {
            CheckConnectionAndNavigate();
        }

        private async void OnRetryClicked(object sender, System.EventArgs e)
        {
            await CheckConnectionAndNavigate();
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = System.TimeSpan.FromSeconds(3);
                    var response = await client.GetAsync("http://clients3.google.com/generate_204");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}