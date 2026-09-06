using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Models;

namespace SystemEgzaminow.WPF.Views.Users
{
    /// <summary>
    /// Logika interakcji dla klasy EditUserView.xaml
    /// </summary>
    public partial class EditUserView : UserControl
    {
        private bool _isViewLoaded;

        public EditUserView()
        {
            InitializeComponent();
            LoadSchoolYears();
            _isViewLoaded = true;
            LoadClassesForSelectedYear();
            LoadUsers();
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

        private void LoadUsers(int? rolaId = null)
        {
            if (!_isViewLoaded || UsersGrid == null)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var query = context.Uzytkownicy
                .Include(u => u.Rola)
                .Include(u => u.Klasa)
                .AsQueryable();

            // Rola
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

            // Rok szkolny
            if (SchoolYearFilterBox.SelectedItem is string schoolYear &&
                schoolYear != "Wszystkie")
            {
                query = query.Where(u =>
                    u.Klasa != null &&
                    u.Klasa.RokSzkolny == schoolYear);
            }

            // Klasa
            if (ClassFilterBox.SelectedItem is string className &&
    className != "Wszystkie")
            {
                query = query.Where(u =>
                    u.Klasa != null &&
                    u.Klasa.NazwaKlasy == className);
            }

            // Wyszukiwanie
            string search = SearchBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
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

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var user = button.DataContext as Uzytkownik;
            if (user == null)
                return;

            if (user.Id <= 3)
            {
                MessageBox.Show("Nie można edytować użytkowników systemowych.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.ShowEditUserFormView(user.Id);
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

        private void SchoolYearFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadClassesForSelectedYear();
            LoadUsers();
        }

        private void ClassFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadUsers();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isViewLoaded)
                return;

            LoadUsers();
        }
    }
}