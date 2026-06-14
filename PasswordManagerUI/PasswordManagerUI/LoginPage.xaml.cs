using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using PasswordManager.Services;

namespace PasswordManagerUI
{
    public sealed partial class LoginPage : Page
    {
        private readonly IAuthService _authService;

        public LoginPage()
        {
            this.InitializeComponent();
            _authService = App.Container.AuthService;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            HandleLogin();
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            HandleRegister();
        }

        private void OnPasswordBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                HandleLogin();
        }

        private void HandleLogin()
        {
            var password = MasterPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter your master password.");
                return;
            }

            SetLoading(true);

            var result = _authService.Login(password);

            SetLoading(false);

            if (result.IsSuccess)
            {
                MasterPasswordBox.Password = string.Empty;
                Frame.Navigate(typeof(VaultPage));
            }
            else
            {
                ShowError(result.Message ?? "Login failed.");
            }
        }

        private void HandleRegister()
        {
            var password = MasterPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter a master password.");
                return;
            }

            SetLoading(true);

            var result = _authService.Register(password);

            SetLoading(false);

            if (result.IsSuccess)
            {
                MasterPasswordBox.Password = string.Empty;
                Frame.Navigate(typeof(VaultPage));
            }
            else
            {
                ShowError(result.Message ?? "Registration failed.");
            }
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.Visibility = Visibility.Visible;
        }

        private void SetLoading(bool isLoading)
        {
            LoginButton.IsEnabled    = !isLoading;
            RegisterButton.IsEnabled = !isLoading;
            MasterPasswordBox.IsEnabled = !isLoading;
            ErrorText.Visibility = Visibility.Collapsed;
        }
    }
}
