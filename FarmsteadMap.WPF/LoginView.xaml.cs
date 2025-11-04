using FarmsteadMap.BLL.Data.DTO;
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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Не вдалося відкрити посилання: " + ex.Message);
            }

            e.Handled = true;
        }

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
                return;
            }

            var registerRequest = new RegisterRequestDTO
            {
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


}