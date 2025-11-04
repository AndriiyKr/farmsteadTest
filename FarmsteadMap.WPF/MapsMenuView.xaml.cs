using System;
using System.Windows;
using System.Windows.Controls;

namespace FarmsteadMap.WPF
{
    public partial class MapsMenuView : UserControl
    {
        public event EventHandler LogoutRequest;
        public event EventHandler<NavigateEventArgs> RequestNavigation;
        private readonly UserControl _myMapsView;
        private readonly UserControl _settingsView;

        public string Username
        {
            set { WelcomeTextBlock.Text = $"Вітаємо, {value}!"; }
        }

        public MapsMenuView()
        {
            InitializeComponent();
            var myMapsViewInstance = new MyMapsView();
            myMapsViewInstance.RequestNavigation += (s, e) => RequestNavigation?.Invoke(this, e);
            _myMapsView = myMapsViewInstance; 

            _settingsView = new SettingsView();

            NavigateTo(MyMapsButton, _myMapsView);
        }

        private void MyMapsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(sender as Button, _myMapsView);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(sender as Button, _settingsView);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequest?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateTo(Button activeButton, UserControl view)
        {
            MainContentArea.Content = view;

            MyMapsButton.Tag = null;
            SettingsButton.Tag = null;

            if (activeButton != null)
            {
                activeButton.Tag = "Active";
            }

            MyMapsButton.Style = (Style)FindResource("HeaderNavButton");
            SettingsButton.Style = (Style)FindResource("HeaderNavButton");
        }
    }
}