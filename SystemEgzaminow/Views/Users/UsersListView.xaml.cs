using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Views.Users
{
    /// <summary>
    /// Logika interakcji dla klasy UsersListView.xaml
    /// </summary>
    public partial class UsersListView : UserControl
    {
        private bool _isViewLoaded;

        public UsersListView()
        {
            InitializeComponent();
            LoadSchoolYears();
            _isViewLoaded = true;
            LoadClassesForSelectedYear();
            LoadUsers();
        }

        private void LoadClassesForSelectedYear()
        {
            using var context = DbContextHelper.CreateDbContext();

            ClassFilterBox.Items.Clear();
            ClassFilterBox.Items.Add("Wszystkie");

            string? schoolYear =
                SchoolYearFilterBox.SelectedItem as string;

            var query = context.Klasy
                .Where(k => context.Uzytkownicy
                    .Any(u => u.KlasaId == k.Id))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(schoolYear) &&
                schoolYear != "Wszystkie")
            {
                query = query.Where(k =>
                    k.RokSzkolny == schoolYear);
            }

            var classNames = query
                .Select(k => k.NazwaKlasy)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            foreach (var className in classNames)
                ClassFilterBox.Items.Add(className);

            ClassFilterBox.SelectedIndex = 0;
        }

        private void LoadSchoolYears()
        {
            using var context = DbContextHelper.CreateDbContext();

            var years = context.Klasy
                .Where(k => context.Uzytkownicy
                    .Any(u => u.KlasaId == k.Id))
                .Select(k => k.RokSzkolny)
                .Distinct()
                .OrderByDescending(r => r)
                .ToList();

            SchoolYearFilterBox.Items.Clear();
            SchoolYearFilterBox.Items.Add("Wszystkie");

            foreach (var year in years)
                SchoolYearFilterBox.Items.Add(year);

            SchoolYearFilterBox.SelectedIndex = 0;
        }

        private void LoadUsers(int? rolaId = null)
        {
            if (!_isViewLoaded || UsersGrid == null)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var query = context.Uzytkownicy
                .Include(u => u.Rola)
                .Include(u => u.Klasa)
                .AsQueryable();

            var selectedRole =
                (RoleFilterBox.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString();

            switch (selectedRole)
            {
                case "Administrator":
                    query = query.Where(u => u.RolaId == 1);
                    break;

                case "Nauczyciel":
                    query = query.Where(u => u.RolaId == 2);
                    break;

                case "Uczeń":
                    query = query.Where(u => u.RolaId == 3);
                    break;
            }

            if (SchoolYearFilterBox.SelectedItem is string schoolYear &&
                schoolYear != "Wszystkie")
            {
                query = query.Where(u =>
                    u.Klasa != null &&
                    u.Klasa.RokSzkolny == schoolYear);
            }

            if (ClassFilterBox.SelectedItem is string className &&
                className != "Wszystkie")
            {
                query = query.Where(u =>
                    u.Klasa != null &&
                    u.Klasa.NazwaKlasy == className);
            }

            string search = SearchBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                bool isId = int.TryParse(search, out int searchedId);
                query = query.Where(u =>
                (isId && u.Id == searchedId) ||
                    u.Login.Contains(search) ||
                    u.Imie.Contains(search) ||
                    u.Nazwisko.Contains(search) ||
                    (u.Klasa != null &&
                     u.Klasa.NazwaKlasy.Contains(search)) ||
                    (u.Klasa != null &&
                     u.Klasa.RokSzkolny.Contains(search)));
            }

            UsersGrid.ItemsSource = query
                .OrderBy(u => u.Nazwisko)
                .ThenBy(u => u.Imie)
                .ToList();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _isViewLoaded = false;

            RoleFilterBox.SelectedIndex = 0;
            SchoolYearFilterBox.SelectedIndex = 0;
            SearchBox.Clear();

            _isViewLoaded = true;

            LoadClassesForSelectedYear();
            LoadUsers();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowAdminView();
            }
        }

        private void RoleFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadUsers();
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var users = UsersGrid.Items
       .Cast<Uzytkownik>()
       .ToList();

            if (users.Count == 0)
            {
                MessageBox.Show(
                    "Brak danych do eksportu.",
                    "Informacja",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Plik Excel (*.xlsx)|*.xlsx",
                DefaultExt = ".xlsx",
                AddExtension = true,
                FileName = "Lista_uzytkownikow.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                var excelService =
                    new ExcelService();

                excelService.ExportUsers(
                    users,
                    saveFileDialog.FileName);

                MessageBox.Show(
                    $"Wyeksportowano {users.Count} użytkowników.",
                    "Eksport zakończony",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się wyeksportować danych.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SchoolYearFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadClassesForSelectedYear();
            LoadUsers();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadUsers();
        }

        private void ClassFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadUsers();
        }
    }
}