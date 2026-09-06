using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string haslo = txtHaslo.Password;

            // MessageBox.Show($"Login: {login}\nHasło: {haslo}");

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(haslo))
            {
                MessageBox.Show("Wprowadź login i hasło.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var loginLogService = new LoginLogService(context);
                var user = context.Uzytkownicy
    .Include(u => u.Rola)
    .FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    await loginLogService.AddLoginLogAsync(null, false);
                    MessageBox.Show(
                        "Nieprawidłowy login lub hasło.",
                        "Błąd logowania",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                var passwordService = new PasswordService();

                bool poprawneHaslo;

                try
                {
                    if (user.Haslo.StartsWith("$2"))
                    {
                        poprawneHaslo =
                            passwordService.VerifyPassword(haslo, user.Haslo);
                    }
                    else
                    {
                        poprawneHaslo = user.Haslo == haslo;

                        if (poprawneHaslo)
                        {
                            user.Haslo =
                                passwordService.HashPassword(haslo);

                            context.SaveChanges();
                        }
                    }
                }
                catch
                {
                    poprawneHaslo = false;
                }

                if (!poprawneHaslo)
                {
                    await loginLogService.AddLoginLogAsync(user.Id, false);
                    MessageBox.Show(
                        "Nieprawidłowy login lub hasło.",
                        "Błąd logowania",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow == null)
                {
                    MessageBox.Show("Nie znaleziono głównego okna aplikacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                LoggedUser.Id = user.Id;
                LoggedUser.Login = user.Login;
                LoggedUser.Imie = user.Imie;
                LoggedUser.Nazwisko = user.Nazwisko;
                LoggedUser.RolaId = user.RolaId;

                var loginLog = await loginLogService.AddLoginLogAsync(user.Id, true);

                LoggedUser.LoginLogId = loginLog.Id;

                switch (user.RolaId)
                {
                    case 1:
                        mainWindow.ShowAdminView(user);
                        break;

                    case 2:
                        mainWindow.ShowTeacherView(user);
                        break;

                    case 3:
                        mainWindow.ShowStudentView(user);
                        break;

                    default:
                        MessageBox.Show("Nieznana rola użytkownika.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}