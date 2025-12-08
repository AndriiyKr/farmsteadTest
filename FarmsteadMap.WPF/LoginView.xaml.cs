// <copyright file="LoginView.xaml.cs" company="PlaceholderCompany">
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
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true,
                });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Не вдалося відкрити посилання: " + ex.Message);
            }

            e.Handled = true;
        }

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
                return;
            }

            var registerRequest = new RegisterRequestDTO
            {
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
}