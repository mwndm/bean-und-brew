using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace beanbrew.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void CreateAccountButton_Click(
            object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            string email = EmailTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string password = PasswordInput.Password;
            string confirmPassword = ConfirmPasswordInput.Password;

            // Check required fields
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("Please fill in all fields.");
                return;
            }

            // Validate email
            if (!Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            // Validate phone number
            if (!Regex.IsMatch(
                phone,
                @"^\+?[0-9\s()\-]{7,20}$"))
            {
                ShowError("Please enter a valid phone number.");
                return;
            }

            // Check password length
            if (password.Length < 8)
            {
                ShowError(
                    "Password must be at least 8 characters long.");
                return;
            }

            // Check password confirmation
            if (password != confirmPassword)
            {
                ShowError("Your passwords do not match.");
                return;
            }

            // No account is saved yet.
            MessageBox.Show(
                "Your details are valid!\n\n" +
                "Account creation will be connected later.",
                "BeanBrew",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BackToLoginButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                NavigationService?.Navigate(new LoginPage());
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
