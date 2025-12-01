<<<<<<< HEAD
﻿// <copyright file="MyMapsView.xaml.cs" company="PlaceholderCompany">
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
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FarmsteadMap.WPF 
{
    public partial class MyMapsView : UserControl
    {
        public event EventHandler<NavigateEventArgs> RequestNavigation;

        private Map _selectedMap; 
        private bool _isRenaming = false;

        public MyMapsView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMapsAsync();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private async Task LoadMapsAsync()
        {
            // TODO: Замінити це на виклик BLL
            // (потрібен BLL-сервіс, який повертає мапи для поточного юзера)
<<<<<<< HEAD
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
=======
            var maps = await MockMapService.GetUserMapsAsync(1); // 1 - це ID юзера (зараз заглушка)
            MapItemsControl.ItemsSource = maps;
        }

        #region Обробники плиток
        private void ContextMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            _selectedMap = button.DataContext as Map;

            button.ContextMenu.IsOpen = true;
        }

        private async void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMap == null) return;
            _isRenaming = true;
            PopupTitle.Text = "Перейменувати мапу";
            CreateMapButton.Content = "Зберегти";
            NewMapNameTextBox.Text = _selectedMap.Name;

            AddMapPopup.Visibility = Visibility.Visible;
            NewMapNameTextBox.Focus();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            if (this.selectedMap == null)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Ви впевнені, що хочете видалити мапу '{this.selectedMap.Name}'?",
=======
            if (_selectedMap == null) return;

            var result = MessageBox.Show(
                $"Ви впевнені, що хочете видалити мапу '{_selectedMap.Name}'?",
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // TODO: Замінити на виклик BLL
<<<<<<< HEAD
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
=======
                await MockMapService.DeleteMapAsync(_selectedMap.Id);
                await LoadMapsAsync();
            }
        }

        #endregion

        #region Логіка "Створити нову"

        private void AddMapTile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isRenaming = false;
            PopupTitle.Text = "Створити нову мапу";
            CreateMapButton.Content = "Створити";
            NewMapNameTextBox.Text = "";

            AddMapPopup.Visibility = Visibility.Visible;
            NewMapNameTextBox.Focus();
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private async void CreateMapButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            string newName = this.NewMapNameTextBox.Text;
=======
            string newName = NewMapNameTextBox.Text;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Назва не може бути пустою.", "Помилка");
                return;
            }

<<<<<<< HEAD
            if (this.isRenaming)
            {
                // TODO: Замінити на виклик BLL
                await MockMapService.RenameMapAsync(this.selectedMap!.Id, newName);

                this.AddMapPopup.Visibility = Visibility.Collapsed;
                await this.LoadMapsAsync();
=======
            if (_isRenaming)
            {
                // TODO: Замінити на виклик BLL
                await MockMapService.RenameMapAsync(_selectedMap.Id, newName);

                AddMapPopup.Visibility = Visibility.Collapsed;
                await LoadMapsAsync(); 
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            else
            {
                // TODO: Замінити на виклик BLL
<<<<<<< HEAD
                var newMap = await MockMapService.CreateMapAsync(newName); // 1 - ID юзера

                this.AddMapPopup.Visibility = Visibility.Collapsed;
                this.RequestNavigation?.Invoke(this, new NavigateEventArgs
                {
                    TargetView = "UserMapView",
                    Data = newMap.Id, // Виправлено SA1413
=======
                var newMap = await MockMapService.CreateMapAsync(newName, 1); // 1 - ID юзера

                AddMapPopup.Visibility = Visibility.Collapsed;
                RequestNavigation?.Invoke(this, new NavigateEventArgs
                {
                    TargetView = "UserMapView",
                    Data = newMap.Id
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                });
            }
        }

        private void CancelMapButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.AddMapPopup.Visibility = Visibility.Collapsed;
        }
    }
=======
            AddMapPopup.Visibility = Visibility.Collapsed;
        }

        #endregion
    }

    #region Імітація BLL (вставте це в C# файл для компіляції)

    // TODO: Створити ці класи у вашому DAL/BLL проекті
    public class Map
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } // Шлях до картинки
    }

    public static class MockMapService
    {
        // Імітація бази даних
        private static List<Map> _maps = new List<Map>
        {
            new Map { Id = 1, Name = "Моя перша мапа", ImageUrl = "/Images/map_placeholder_1.jpg" },
            new Map { Id = 2, Name = "Сад біля будинку", ImageUrl = "/Images/map_placeholder_2.jpg" }
        };
        private static int _nextId = 3;

        public static async Task<List<Map>> GetUserMapsAsync(int userId)
        {
            await Task.Delay(200); 
            return _maps.ToList(); 
        }

        public static async Task DeleteMapAsync(int mapId)
        {
            await Task.Delay(100);
            var map = _maps.FirstOrDefault(m => m.Id == mapId);
            if (map != null) _maps.Remove(map);
        }

        public static async Task<Map> CreateMapAsync(string name, int userId)
        {
            await Task.Delay(100);
            var newMap = new Map
            {
                Id = _nextId++,
                Name = name,
                ImageUrl = "/Images/map_placeholder_1.jpg"
            };
            _maps.Add(newMap);
            return newMap;
        }

        public static async Task RenameMapAsync(int mapId, string newName)
        {
            await Task.Delay(100);
            var map = _maps.FirstOrDefault(m => m.Id == mapId);
            if (map != null) map.Name = newName;
        }
    }

    #endregion
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
}