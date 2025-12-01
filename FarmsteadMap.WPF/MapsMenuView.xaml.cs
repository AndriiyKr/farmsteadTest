<<<<<<< HEAD
﻿// <copyright file="MapsMenuView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Забезпечує логіку взаємодії для MapsMenuView.xaml.
    /// </summary>
    public partial class MapsMenuView : UserControl
    {
        private readonly UserControl myMapsView;
        private readonly SettingsView settingsView;
        private long currentUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapsMenuView"/> class.
        /// </summary>
        /// <param name="settingsView">View налаштувань.</param>
        public MapsMenuView(SettingsView settingsView)
        {
            this.InitializeComponent();
            var myMapsViewInstance = new MyMapsView();
            myMapsViewInstance.RequestNavigation += (s, e) => this.RequestNavigation?.Invoke(this, e);
            this.myMapsView = myMapsViewInstance;

            this.settingsView = settingsView;
            this.settingsView.AccountDeleted += this.OnAccountDeleted;

            this.NavigateTo(this.MyMapsButton, this.myMapsView);
        }

        /// <summary>
        /// Подія, що запитує вихід з облікового запису.
        /// </summary>
        public event EventHandler? LogoutRequest;

        /// <summary>
        /// Подія, що запитує навігацію до іншого View.
        /// </summary>
        public event EventHandler<NavigateEventArgs>? RequestNavigation;

        /// <summary>
        /// Встановлює дані поточного користувача.
        /// </summary>
        /// <param name="userId">ID користувача.</param>
        /// <param name="username">Ім'я користувача.</param>
        public void SetCurrentUser(long userId, string username)
        {
            this.WelcomeTextBlock.Text = $"Вітаємо, {username}!";
            this.currentUserId = userId;
=======
﻿using System;
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
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void MyMapsButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.NavigateTo(sender as Button, this.myMapsView);
=======
            NavigateTo(sender as Button, _myMapsView);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.settingsView.LoadUserData(this.currentUserId);
            this.NavigateTo(sender as Button, this.settingsView);
=======
            NavigateTo(sender as Button, _settingsView);
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            this.LogoutRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnAccountDeleted(object? sender, EventArgs e)
        {
            this.LogoutRequest?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateTo(Button? activeButton, UserControl view)
        {
            this.MainContentArea.Content = view;

            this.MyMapsButton.Tag = null;
            this.SettingsButton.Tag = null;
=======
            LogoutRequest?.Invoke(this, EventArgs.Empty);
        }

        private void NavigateTo(Button activeButton, UserControl view)
        {
            MainContentArea.Content = view;

            MyMapsButton.Tag = null;
            SettingsButton.Tag = null;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3

            if (activeButton != null)
            {
                activeButton.Tag = "Active";
            }

<<<<<<< HEAD
            this.MyMapsButton.Style = (Style)this.FindResource("HeaderNavButton");
            this.SettingsButton.Style = (Style)this.FindResource("HeaderNavButton");
=======
            MyMapsButton.Style = (Style)FindResource("HeaderNavButton");
            SettingsButton.Style = (Style)FindResource("HeaderNavButton");
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
        }
    }
}