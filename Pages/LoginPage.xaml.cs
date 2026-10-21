using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(
            object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            string email = EmailTextBox.Text.Trim();
            string password = PasswordInput.Password;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter your email and password.");
                return;
            }

            // TODO: Authenticate against your backend.
            // Do not accept arbitrary credentials in production.

            ShowError("Connect your authentication service to sign in.");
        }
        private void RegisterButton_Click(
            object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(
                new RegisterPage());
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
