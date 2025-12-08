using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FarmsteadMap.WPF
{
    public partial class MyMapsView : UserControl
    {
        private MapShortDTO? _selectedMap;
        private bool _isRenaming;
        private long _currentUserId;
        private string _currentUsername;

        // Подія для переходу в редактор
        public event EventHandler<NavigateEventArgs>? RequestNavigation;

        // Подія для виходу (logout)
        public event EventHandler? LogoutRequest;

        public MyMapsView()
        {
            InitializeComponent();
        }

        // Цей метод викликає MainWindow, щоб передати дані юзера
        public void SetCurrentUser(long userId, string username)
        {
            _currentUserId = userId;
            _currentUsername = username;

            // Одразу завантажуємо мапи
            if (_currentUserId > 0) LoadMapsAsync();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentUserId > 0) LoadMapsAsync();
        }

        private async void LoadMapsAsync()
        {
            try
            {
                // Отримуємо сервіс з контейнера
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var mapService = scope.ServiceProvider.GetRequiredService<IMapService>();
                    var response = await mapService.GetUserMapsAsync(_currentUserId);

                    if (response.Success)
                    {
                        MapItemsControl.ItemsSource = response.Data;
                    }
                    else
                    {
                        MessageBox.Show(response.Error, "Помилка завантаження");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
        }

        // --- КЛІК ПО МАПІ (ВІДКРИТТЯ РЕДАКТОРА) ---
        private void OpenMap_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is MapShortDTO map)
            {
                RequestNavigation?.Invoke(this, new NavigateEventArgs
                {
                    TargetView = "UserMapView",
                    Data = (int)map.Id // Передаємо ID мапи
                });
            }
        }

        // --- КОНТЕКСТНЕ МЕНЮ ---
        private void ContextMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            _selectedMap = button.DataContext as MapShortDTO;
            button.ContextMenu.IsOpen = true;
            e.Handled = true; // Щоб не спрацював клік по плитці
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMap == null) return;
            _isRenaming = true;
            PopupTitle.Text = "Перейменувати мапу";
            CreateMapButton.Content = "Зберегти";
            NewMapNameTextBox.Text = _selectedMap.Name;
            AddMapPopup.Visibility = Visibility.Visible;
            NewMapNameTextBox.Focus();
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMap == null) return;

            var res = MessageBox.Show($"Видалити мапу '{_selectedMap.Name}'?", "Видалення", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                using (var scope = App.AppHost!.Services.CreateScope())
                {
                    var mapService = scope.ServiceProvider.GetRequiredService<IMapService>();
                    await mapService.DeleteMapAsync(_selectedMap.Id);
                    LoadMapsAsync();
                }
            }
        }

        // --- СТВОРЕННЯ МАПИ ---
        private void AddMapTile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isRenaming = false;
            PopupTitle.Text = "Створити нову мапу";
            CreateMapButton.Content = "Створити";
            NewMapNameTextBox.Text = "";
            AddMapPopup.Visibility = Visibility.Visible;
            NewMapNameTextBox.Focus();
        }

        private async void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NewMapNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("Введіть назву"); return; }

            using (var scope = App.AppHost!.Services.CreateScope())
            {
                var mapService = scope.ServiceProvider.GetRequiredService<IMapService>();

                if (_isRenaming && _selectedMap != null)
                {
                    // Редагування назви
                    var updateDto = new UpdateMapDTO { Id = _selectedMap.Id, Name = name };
                    await mapService.UpdateMapAsync(updateDto);
                }
                else
                {
                    // Створення нової
                    var createDto = new CreateMapDTO
                    {
                        Name = name,
                        IsPrivate = false,
                        UserId = _currentUserId,
                        MapJson = null // Сервіс створить дефолтний JSON
                    };

                    var result = await mapService.CreateMapAsync(createDto);

                    if (result.Success && !_isRenaming)
                    {
                        // Одразу переходимо в редактор
                        AddMapPopup.Visibility = Visibility.Collapsed;
                        RequestNavigation?.Invoke(this, new NavigateEventArgs
                        {
                            TargetView = "UserMapView",
                            Data = (int)result.Data // ID нової мапи
                        });
                        return;
                    }
                }
            }

            AddMapPopup.Visibility = Visibility.Collapsed;
            LoadMapsAsync();
        }

        private void CancelMapButton_Click(object sender, RoutedEventArgs e)
        {
            AddMapPopup.Visibility = Visibility.Collapsed;
        }
    }
}