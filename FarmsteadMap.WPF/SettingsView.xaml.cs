<<<<<<< HEAD
﻿// <copyright file="SettingsView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using FarmsteadMap.BLL.Data.DTO;
    using FarmsteadMap.BLL.Services;

    /// <summary>
    /// Забезпечує логіку взаємодії для SettingsView.xaml (Налаштування профілю).
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private readonly IUserService userService;
        private readonly IUserAccountService accountService;
        private long currentUserId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsView"/> class.
        /// </summary>
        /// <param name="userService">Сервіс для роботи з даними користувача.</param>
        /// <param name="userAccountService">Сервіс для роботи з акаунтом (пароль, видалення).</param>
        public SettingsView(IUserService userService, IUserAccountService userAccountService)
        {
            this.InitializeComponent();
            this.userService = userService;
            this.accountService = userAccountService;
        }

        /// <summary>
        /// Подія, що спрацьовує, коли акаунт було успішно видалено.
        /// </summary>
        public event EventHandler? AccountDeleted;

        /// <summary>
        /// Асинхронно завантажує дані користувача у форму.
        /// </summary>
        /// <param name="userId">ID користувача для завантаження.</param>
        public async void LoadUserData(long userId)
        {
            this.currentUserId = userId;
            var response = await this.userService.GetUserDataAsync(userId);

            if (response.Success && response.Data != null)
            {
                var user = response.Data;
                this.LastNameTextBox.Text = user.Lastname;
                this.FirstNameTextBox.Text = user.Firstname;
                this.UsernameTextBox.Text = user.Username;
                this.EmailTextBox.Text = user.Email;
            }
            else
            {
                MessageBox.Show(response.Error ?? "Не вдалося завантажити дані профілю.", "Помилка завантаження", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.SettingsNav.SelectedIndex = 0;
        }

        private void SettingsNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ContentTabControl != null)
            {
                this.ContentTabControl.SelectedIndex = this.SettingsNav.SelectedIndex;
            }
        }

        private async void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете оновити дані профілю?",
                "Підтвердження оновлення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            var dto = new UpdateProfileRequestDTO
            {
                Firstname = this.FirstNameTextBox.Text,
                Lastname = this.LastNameTextBox.Text,
                Username = this.UsernameTextBox.Text,
                Email = this.EmailTextBox.Text,
            };

            var response = await this.accountService.UpdateProfileAsync(this.currentUserId, dto);

            if (response.Success)
            {
                MessageBox.Show("Дані профілю успішно збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(response.Error, "Помилка збереження", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Ви впевнені, що хочете змінити пароль?",
                "Підтвердження зміни пароля",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            var dto = new ChangePasswordRequestDTO
            {
                OldPassword = this.OldPasswordBox.Password,
                NewPassword = this.NewPasswordBox.Password,
                ConfirmNewPassword = this.ConfirmNewPasswordBox.Password,
            };

            var response = await this.accountService.ChangePasswordAsync(this.currentUserId, dto);

            if (response.Success)
            {
                MessageBox.Show("Пароль успішно змінено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                this.OldPasswordBox.Password = string.Empty;
                this.NewPasswordBox.Password = string.Empty;
                this.ConfirmNewPasswordBox.Password = string.Empty;
            }
            else
            {
                MessageBox.Show(response.Error, "Помилка зміни пароля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Ви АБСОЛЮТНО впевнені? Ця дія є незворотною і видалить всі ваші дані назавжди.",
                "ПІДТВЕРДЖЕННЯ ВИДАЛЕННЯ",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            string password = this.OldPasswordBox.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Будь ласка, введіть ваш поточний пароль у поле 'Старий пароль' (в секції 'Безпека'), щоб підтвердити видалення акаунта.",
                    "Потрібне підтвердження",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                this.SettingsNav.SelectedIndex = 1;
                return;
            }

            var response = await this.accountService.DeleteAccountAsync(this.currentUserId, password);

            if (response.Success)
            {
                MessageBox.Show("Ваш акаунт було успішно видалено.", "Акаунт видалено", MessageBoxButton.OK, MessageBoxImage.Information);
                this.AccountDeleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show(response.Error, "Помилка видалення", MessageBoxButton.OK, MessageBoxImage.Error);
                this.SettingsNav.SelectedIndex = 1;
=======
﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FarmsteadMap.WPF
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            LoadCurrentUserData();
        }

        private void LoadCurrentUserData()
        {
            // TODO: Замінити на реальний BLL-виклик
            LastNameTextBox.Text = "lastname_from_db";
            FirstNameTextBox.Text = "firstname_from_db";
            UsernameTextBox.Text = "current_username";
            EmailTextBox.Text = "current@email.com";
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Виклик BLL для збереження:
            // - LastNameTextBox.Text
            // - FirstNameTextBox.Text
            // - UsernameTextBox.Text
            // - EmailTextBox.Text

            // BLL має повернути, чи було збереження успішним 
            // (напр. чи не зайнятий новий username/email)

            MessageBox.Show("Дані профілю збережено!");
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password != ConfirmNewPasswordBox.Password)
            {
                MessageBox.Show("Нові паролі не співпадають!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // TODO: Виклик BLL (OldPasswordBox.Password, NewPasswordBox.Password)
            MessageBox.Show("Пароль успішно змінено!");

            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ConfirmNewPasswordBox.Password = "";
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Ви АБСОЛЮТНО впевнені? Це видалить всі ваші дані НАЗАВЖДИ.",
                "Підтвердження видалення акаунта",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                // TODO: Виклик BLL для видалення акаунта
                MessageBox.Show("Акаунт видалено.");

                // TODO: Реалізувати логіку "LogoutRequest", 
                // щоб повернутися на екран входу.
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
<<<<<<< HEAD
                    UseShellExecute = true,
=======
                    UseShellExecute = true
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Не вдалося відкрити посилання: " + ex.Message);
            }
<<<<<<< HEAD

=======
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            e.Handled = true;
        }
    }
}