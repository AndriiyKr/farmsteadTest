namespace FarmsteadMap.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Меню з вибором розділів (Мої мапи, Налаштування).
    /// </summary>
    public partial class MapsMenuView : UserControl
    {
        private long currentUserId;
        private string currentUsername = string.Empty;

        public MapsMenuView()
        {
            this.InitializeComponent();
        }

        public event EventHandler? LogoutRequest;
        public event EventHandler<NavigateEventArgs>? RequestNavigation;

        // Метод, який викликає MainWindow при вході
        public void SetCurrentUser(long userId, string username)
        {
            this.currentUserId = userId;
            this.currentUsername = username;

            // Оновлюємо текст привітання
            if (this.WelcomeTextBlock != null)
            {
                this.WelcomeTextBlock.Text = $"Вітаємо, {username}";
            }

            // За замовчуванням відкриваємо вкладку "Мої мапи"
            this.ShowMyMaps();
        }

        // --- Обробники кнопок меню ---

        private void MyMapsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMyMaps();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowSettings();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.LogoutRequest?.Invoke(this, EventArgs.Empty);
        }

        // --- Логіка перемикання View ---

        private void ShowMyMaps()
        {
            var myMapsView = new MyMapsView();
            myMapsView.SetCurrentUser(this.currentUserId, this.currentUsername);

            myMapsView.RequestNavigation += (s, e) =>
            {
                this.RequestNavigation?.Invoke(this, e);
            };

            this.MainContentArea.Content = myMapsView;
            this.UpdateButtonStyles("MyMaps");
        }

        private void ShowSettings()
        {
            try
            {
                // Отримуємо View з контейнера
                var settingsView = App.AppHost!.Services.GetRequiredService<SettingsView>();

                // --- ВИПРАВЛЕННЯ: Явно завантажуємо дані користувача ---
                settingsView.LoadUserData(this.currentUserId);
                // ------------------------------------------------------
                this.MainContentArea.Content = settingsView;
                this.UpdateButtonStyles("Settings");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося відкрити налаштування: {ex.Message}");
            }
        }

        private void UpdateButtonStyles(string activeKey)
        {
            if (this.MyMapsButton != null)
                this.MyMapsButton.Tag = activeKey == "MyMaps" ? "Active" : "";

            if (this.SettingsButton != null)
                this.SettingsButton.Tag = activeKey == "Settings" ? "Active" : "";
        }
    }
}