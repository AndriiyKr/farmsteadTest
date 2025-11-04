using System.Diagnostics;
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
            }
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
    }
}