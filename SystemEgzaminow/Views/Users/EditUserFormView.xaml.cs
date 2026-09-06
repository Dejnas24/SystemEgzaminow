using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Views.Users
{
    /// <summary>
    /// Logika interakcji dla klasy EditUserFormView.xaml
    /// </summary>
    public partial class EditUserFormView : UserControl
    {
        private readonly int _userId;
        private bool _isLoading;

        public EditUserFormView(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUser();
        }

        private void LoadUser()
        {
            try
            {
                _isLoading = true;

                using var context = DbContextHelper.CreateDbContext();

                var user = context.Uzytkownicy
                    .FirstOrDefault(u => u.Id == _userId);

                if (user == null)
                {
                    MessageBox.Show(
                        "Nie znaleziono użytkownika.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                LoginBox.Text = user.Login;
                PasswordBox.Clear();
                FirstNameBox.Text = user.Imie;
                LastNameBox.Text = user.Nazwisko;

                switch (user.RolaId)
                {
                    case 1:
                        RoleBox.SelectedIndex = 0;
                        break;

                    case 2:
                        RoleBox.SelectedIndex = 1;
                        break;

                    case 3:
                        RoleBox.SelectedIndex = 2;
                        break;

                    default:
                        RoleBox.SelectedIndex = -1;
                        break;
                }

                if (user.RolaId == 3)
                {
                    ShowClassFields();

                    var years = context.Klasy
                        .Select(k => k.RokSzkolny)
                        .Distinct()
                        .OrderByDescending(r => r)
                        .ToList();

                    SchoolYearBox.ItemsSource = years;

                    if (user.KlasaId.HasValue)
                    {
                        var userClass = context.Klasy
                            .FirstOrDefault(k => k.Id == user.KlasaId.Value);

                        if (userClass != null)
                        {
                            SchoolYearBox.SelectedItem =
                                userClass.RokSzkolny;

                            var classes = context.Klasy
                                .Where(k =>
                                    k.RokSzkolny == userClass.RokSzkolny)
                                .OrderBy(k => k.NazwaKlasy)
                                .ToList();

                            ClassBox.ItemsSource = classes;
                            ClassBox.DisplayMemberPath = "NazwaKlasy";
                            ClassBox.SelectedValuePath = "Id";

                            ClassBox.SelectedValue =
                                userClass.Id;
                        }
                    }
                }
                else
                {
                    HideClassFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd podczas wczytywania użytkownika: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ShowClassFields()
        {
            SchoolYearLabel.Visibility = Visibility.Visible;
            SchoolYearBox.Visibility = Visibility.Visible;
            ClassLabel.Visibility = Visibility.Visible;
            ClassBox.Visibility = Visibility.Visible;
        }

        private void HideClassFields()
        {
            SchoolYearLabel.Visibility = Visibility.Collapsed;
            SchoolYearBox.Visibility = Visibility.Collapsed;
            ClassLabel.Visibility = Visibility.Collapsed;
            ClassBox.Visibility = Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string haslo = PasswordBox.Password.Trim();
            string imie = FirstNameBox.Text.Trim();
            string nazwisko = LastNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(imie) ||
                string.IsNullOrWhiteSpace(nazwisko) ||
                RoleBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Uzupełnij wszystkie wymagane pola.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!string.IsNullOrWhiteSpace(haslo) && haslo.Length < 6)
            {
                MessageBox.Show("Hasło musi mieć co najmniej 6 znaków.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
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

                var user = context.Uzytkownicy.FirstOrDefault(u => u.Id == _userId);

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (_userId <= 3)
                {
                    MessageBox.Show("Nie można edytować użytkowników systemowych.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                user.Imie = imie;
                user.Nazwisko = nazwisko;
                user.RolaId = rolaId;
                user.KlasaId = klasaId;

                if (!string.IsNullOrWhiteSpace(haslo))
                {
                    var passwordService = new PasswordService();

                    user.Haslo =
                        passwordService.HashPassword(haslo);
                }

                context.SaveChanges();

                MessageBox.Show("Zmiany zostały zapisane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.ShowEditUserView();
                }
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

            mainWindow.ShowEditUserView();
        }

        private void RoleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoading)
                return;

            var selectedRole =
                (RoleBox.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString();

            if (selectedRole == "Uczeń")
            {
                ShowClassFields();

                using var context =
                    DbContextHelper.CreateDbContext();

                var years = context.Klasy
                    .Select(k => k.RokSzkolny)
                    .Distinct()
                    .OrderByDescending(r => r)
                    .ToList();

                SchoolYearBox.ItemsSource = years;
            }
            else
            {
                HideClassFields();

                SchoolYearBox.SelectedItem = null;
                ClassBox.SelectedItem = null;

                SchoolYearBox.ItemsSource = null;
                ClassBox.ItemsSource = null;
            }
        }

        private void SchoolYearBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoading)
                return;

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
    }
}