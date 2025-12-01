// <copyright file="MyMapsView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Забезпечує логіку взаємодії для MyMapsView.xaml.
    /// </summary>
    public partial class MyMapsView : UserControl
    {
        private Map? selectedMap; // Виправлено SA1309 та CS8618
        private bool isRenaming;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyMapsView"/> class.
        /// </summary>
        public MyMapsView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Подія, що запитує навігацію до іншого View.
        /// </summary>
        public event EventHandler<NavigateEventArgs>? RequestNavigation; // Виправлено CS8618

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await this.LoadMapsAsync();
        }

        private async Task LoadMapsAsync()
        {
            // TODO: Замінити це на виклик BLL
            // (потрібен BLL-сервіс, який повертає мапи для поточного юзера)
            var maps = await MockMapService.GetUserMapsAsync(); // 1 - це ID юзера (зараз заглушка)
            this.MapItemsControl.ItemsSource = maps;
        }

        // --- Обробники плиток ---
        private void ContextMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            this.selectedMap = button.DataContext as Map; // Виправлено CS8601
            button.ContextMenu.IsOpen = true;
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e) // Виправлено CS1998 (прибрано async)
        {
            if (this.selectedMap == null)
            {
                return; // Виправлено SA1503
            }

            this.isRenaming = true;
            this.PopupTitle.Text = "Перейменувати мапу";
            this.CreateMapButton.Content = "Зберегти";
            this.NewMapNameTextBox.Text = this.selectedMap.Name;

            this.AddMapPopup.Visibility = Visibility.Visible;
            this.NewMapNameTextBox.Focus();
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedMap == null)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Ви впевнені, що хочете видалити мапу '{this.selectedMap.Name}'?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // TODO: Замінити на виклик BLL
                await MockMapService.DeleteMapAsync(this.selectedMap.Id);
                await this.LoadMapsAsync();
            }
        }

        // --- Логіка "Створити нову" ---
        private void AddMapTile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.isRenaming = false;
            this.PopupTitle.Text = "Створити нову мапу";
            this.CreateMapButton.Content = "Створити";
            this.NewMapNameTextBox.Text = string.Empty; // Виправлено SA1122

            this.AddMapPopup.Visibility = Visibility.Visible;
            this.NewMapNameTextBox.Focus();
        }

        private async void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = this.NewMapNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Назва не може бути пустою.", "Помилка");
                return;
            }

            if (this.isRenaming)
            {
                // TODO: Замінити на виклик BLL
                await MockMapService.RenameMapAsync(this.selectedMap!.Id, newName);

                this.AddMapPopup.Visibility = Visibility.Collapsed;
                await this.LoadMapsAsync();
            }
            else
            {
                // TODO: Замінити на виклик BLL
                var newMap = await MockMapService.CreateMapAsync(newName); // 1 - ID юзера

                this.AddMapPopup.Visibility = Visibility.Collapsed;
                this.RequestNavigation?.Invoke(this, new NavigateEventArgs
                {
                    TargetView = "UserMapView",
                    Data = newMap.Id, // Виправлено SA1413
                });
            }
        }

        private void CancelMapButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddMapPopup.Visibility = Visibility.Collapsed;
        }
    }
}