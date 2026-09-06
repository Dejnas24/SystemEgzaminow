using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Views.Users
{
    /// <summary>
    /// Logika interakcji dla klasy AddUserView.xaml
    /// </summary>
    public partial class AddUserView : UserControl
    {
        public AddUserView()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim().ToLower();
            string haslo = PasswordBox.Password.Trim();
            string imie = FirstNameBox.Text.Trim();
            string nazwisko = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(haslo) ||
                string.IsNullOrWhiteSpace(imie) ||
                string.IsNullOrWhiteSpace(nazwisko) ||
                RoleBox.SelectedItem == null)
            {
                MessageBox.Show("Uzupełnij wszystkie pola.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int rolaId = 0;
            int? klasaId = null;

            var selectedRole = (RoleBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            switch (selectedRole)
            {
                case "Administrator":
                    rolaId = 1;
                    break;

                case "Nauczyciel":
                    rolaId = 2;
                    break;

                case "Uczeń":
                    rolaId = 3;
                    break;

                default:
                    MessageBox.Show("Wybierz poprawną rolę.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
            }

            if (rolaId == 3)
            {
                if (SchoolYearBox.SelectedItem == null ||
                    ClassBox.SelectedItem == null)
                {
                    MessageBox.Show(
                        "Dla ucznia wybierz rok szkolny i klasę.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                if (ClassBox.SelectedValue is int selectedClassId)
                {
                    klasaId = selectedClassId;
                }
            }
            try
            {
                using var context = DbContextHelper.CreateDbContext();

                bool loginExists = context.Uzytkownicy.Any(u => u.Login == login);

                if (loginExists)
                {
                    MessageBox.Show("Taki login już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Clear();
                    return;
                }
                if (haslo.Length < 6)
                {
                    MessageBox.Show("Hasło musi mieć co najmniej 6 znaków.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Clear();
                    return;
                }

                var passwordService = new PasswordService();

                string passwordHash =
                    passwordService.HashPassword(haslo);

                var nowyUzytkownik = new Uzytkownik
                {
                    Login = login,
                    Haslo = passwordHash,
                    Imie = imie,
                    Nazwisko = nazwisko,
                    RolaId = rolaId,
                    KlasaId = klasaId
                };

                context.Uzytkownicy.Add(nowyUzytkownik);
                context.SaveChanges();

                MessageBox.Show("Użytkownik został dodany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            mainWindow.ShowAdminView();
        }

        private void Clear()
        {
            LoginBox.Clear();
            PasswordBox.Clear();
            FirstNameBox.Clear();
            LastNameBox.Clear();
            RoleBox.SelectedIndex = -1;
        }

        private void SchoolYearBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SchoolYearBox.SelectedItem is not string rokSzkolny)
            {
                ClassBox.ItemsSource = null;
                return;
            }

            using var context =
                DbContextHelper.CreateDbContext();

            var classes = context.Klasy
                .Where(k => k.RokSzkolny == rokSzkolny)
                .OrderBy(k => k.NazwaKlasy)
                .ToList();

            ClassBox.ItemsSource = classes;
            ClassBox.DisplayMemberPath = "NazwaKlasy";
            ClassBox.SelectedValuePath = "Id";
        }

        private void RoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRole =
       (RoleBox.SelectedItem as ComboBoxItem)?
       .Content?
       .ToString();

            bool isStudent = selectedRole == "Uczeń";

            SchoolYearLabel.Visibility =
                isStudent
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            SchoolYearBox.Visibility =
                isStudent
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            ClassLabel.Visibility =
                isStudent
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            ClassBox.Visibility =
                isStudent
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            if (isStudent)
            {
                LoadSchoolYears();
            }
            else
            {
                SchoolYearBox.ItemsSource = null;
                ClassBox.ItemsSource = null;
                SchoolYearBox.SelectedItem = null;
                ClassBox.SelectedItem = null;
            }
        }

        private void LoadSchoolYears()
        {
            using var context =
                DbContextHelper.CreateDbContext();

            var years = context.Klasy
                .Select(k => k.RokSzkolny)
                .Distinct()
                .OrderByDescending(r => r)
                .ToList();

            SchoolYearBox.ItemsSource = years;
        }
    }
}