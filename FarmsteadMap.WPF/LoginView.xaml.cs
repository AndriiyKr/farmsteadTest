<<<<<<< HEAD
﻿// <copyright file="LoginView.xaml.cs" company="PlaceholderCompany">
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
    /// Забезпечує логіку взаємодії для LoginView.xaml.
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginView"/> class.
        /// </summary>
        /// <param name="authService">Сервіс автентифікації.</param>
        public LoginView(IAuthService authService)
        {
            this.InitializeComponent();
            this.authService = authService;
        }

        /// <summary>
        /// Подія, що спрацьовує при успішному вході.
        /// </summary>
        public event EventHandler<LoginSuccessEventArgs>? LoginSuccess;

        /// <summary>
        /// Обробник для навігації за гіперпосиланням.
        /// </summary>
=======
﻿using FarmsteadMap.BLL.Data.DTO;
using FarmsteadMap.BLL.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;

namespace FarmsteadMap.WPF
{
    public class LoginSuccessEventArgs : EventArgs
    {
        public string Username { get; set; }
    }


    public partial class LoginView : UserControl
    {
        public event EventHandler<LoginSuccessEventArgs> LoginSuccess;
        private readonly IAuthService _authService;

        public LoginView(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
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

            e.Handled = true;
        }

<<<<<<< HEAD
        /// <summary>
        /// Показує панель реєстрації та ховає панель логіну.
        /// </summary>
        private void ShowRegisterPanel_Click(object sender, RoutedEventArgs e)
        {
            this.LoginPanel.Visibility = Visibility.Collapsed;
            this.RegistrationPanel.Visibility = Visibility.Visible;
            this.LoginErrorTextBlock.Visibility = Visibility.Collapsed;
            this.RegisterErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Показує панель логіну та ховає панель реєстрації.
        /// </summary>
        private void ShowLoginPanel_Click(object sender, RoutedEventArgs e)
        {
            this.RegistrationPanel.Visibility = Visibility.Collapsed;
            this.LoginPanel.Visibility = Visibility.Visible;
            this.LoginErrorTextBlock.Visibility = Visibility.Collapsed;
            this.RegisterErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Обробник натискання кнопки "Увійти".
        /// </summary>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoginErrorTextBlock.Visibility = Visibility.Collapsed;

            var loginRequest = new LoginRequestDTO
            {
                Username = this.UsernameTextBox.Text,
                Password = this.PasswordBox.Password,
            };

            var response = await this.authService.LoginAsync(loginRequest);

            if (response.User != null)
            {
                this.LoginSuccess?.Invoke(this, new LoginSuccessEventArgs { UserId = response.User.Id, Username = response.User.Username });
            }
            else
            {
                this.LoginErrorTextBlock.Text = response.Error ?? "Невідома помилка логіну";
                this.LoginErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Обробник натискання кнопки "Зареєструватися".
        /// </summary>
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.RegisterErrorTextBlock.Visibility = Visibility.Collapsed;

            if (this.RegPasswordBox.Password != this.RegConfirmPasswordBox.Password)
            {
                this.RegisterErrorTextBlock.Text = "Паролі не співпадають";
                this.RegisterErrorTextBlock.Visibility = Visibility.Visible;
=======
        #region Перемикачі видимості

        private void ShowRegisterPanel_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegistrationPanel.Visibility = Visibility.Visible;
            LoginErrorTextBlock.Visibility = Visibility.Collapsed;
            RegisterErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowLoginPanel_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
            LoginErrorTextBlock.Visibility = Visibility.Collapsed;
            RegisterErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Логіка Логіну
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginErrorTextBlock.Visibility = Visibility.Collapsed;

            var loginRequest = new LoginRequestDTO
            {
                Username = UsernameTextBox.Text,
                Password = PasswordBox.Password
            };

            var response = await _authService.LoginAsync(loginRequest);

            if (response.User != null) 
            {
                LoginSuccess?.Invoke(this, new LoginSuccessEventArgs { Username = response.User.Username });
            }
            else 
            {
                LoginErrorTextBlock.Text = response.Error ?? "Невідома помилка логіну";
                LoginErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Логіка Реєстрації (ОНОВЛЕНО)

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterErrorTextBlock.Visibility = Visibility.Collapsed;

            if (RegPasswordBox.Password != RegConfirmPasswordBox.Password)
            {
                RegisterErrorTextBlock.Text = "Паролі не співпадають";
                RegisterErrorTextBlock.Visibility = Visibility.Visible;
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
                return;
            }

            var registerRequest = new RegisterRequestDTO
            {
<<<<<<< HEAD
                Lastname = this.RegLastNameTextBox.Text,
                Firstname = this.RegFirstNameTextBox.Text,
                Username = this.RegUsernameTextBox.Text,
                Email = this.RegEmailTextBox.Text,
                Password = this.RegPasswordBox.Password,
                ConfirmPassword = this.RegConfirmPasswordBox.Password,
                TermsAccepted = this.RegTermsCheckBox.IsChecked == true,
                PersonalDataAccepted = this.RegPrivacyCheckBox.IsChecked == true,
            };

            var response = await this.authService.RegisterAsync(registerRequest);

            if (response.Success)
            {
                this.ShowLoginPanel_Click(this, null!);

                this.RegLastNameTextBox.Text = string.Empty;
                this.RegFirstNameTextBox.Text = string.Empty;
            }
            else
            {
                this.RegisterErrorTextBlock.Text = response.Error ?? "Невідома помилка реєстрації";
                this.RegisterErrorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
=======
                Lastname = RegLastNameTextBox.Text,
                Firstname = RegFirstNameTextBox.Text,
                Username = RegUsernameTextBox.Text,
                Email = RegEmailTextBox.Text,
                Password = RegPasswordBox.Password,
                ConfirmPassword = RegConfirmPasswordBox.Password,
                TermsAccepted = RegTermsCheckBox.IsChecked == true,
                PersonalDataAccepted = RegPrivacyCheckBox.IsChecked == true
            };

            var response = await _authService.RegisterAsync(registerRequest);

            if (response.Success) 
            {
                ShowLoginPanel_Click(null, null); 

                RegLastNameTextBox.Text = "";
                RegFirstNameTextBox.Text = "";
            }
            else 
            {
                RegisterErrorTextBlock.Text = response.Error ?? "Невідома помилка реєстрації";
                RegisterErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }


>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
}