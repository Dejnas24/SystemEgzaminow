using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Views.Logs
{
    /// <summary>
    /// Logika interakcji dla klasy LoginLogsView.xaml
    /// </summary>
    public partial class LoginLogsView : UserControl
    {
        private List<LoginLogDto> _allLogs = new();

        public LoginLogsView()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowAdminView();
            }
        }

        private async void LoginLogsView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            try
            {
                using var context =
                    DbContextHelper.CreateDbContext();

                var service =
                    new LoginLogService(context);

                _allLogs =
                    await service.GetLoginLogsAsync();

                LoginLogsGrid.ItemsSource = _allLogs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd podczas pobierania logów: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var logs = LoginLogsGrid.Items.Cast<LoginLogDto>().ToList();

            if (logs.Count == 0)
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
                FileName = "Logi_logowania.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                var excelService = new ExcelService();

                excelService.ExportLoginLogs(logs, saveFileDialog.FileName);

                MessageBox.Show(
                    $"Wyeksportowano {logs.Count} rekordów.",
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

        private void ApplyFilters()
        {
            if (_allLogs == null || LoginLogsGrid == null)
            {
                return;
            }
            IEnumerable<LoginLogDto> query = _allLogs;

            // Rola
            var selectedRole =
                (RoleFilterBox.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString();

            if (!string.IsNullOrWhiteSpace(selectedRole) &&
                selectedRole != "Wszystkie")
            {
                query = query.Where(l =>
                    l.Rola == selectedRole);
            }

            // Sesja
            var selectedSession =
                (SessionFilterBox.SelectedItem as ComboBoxItem)?
                .Content?
                .ToString();

            if (!string.IsNullOrWhiteSpace(selectedSession) &&
                selectedSession != "Wszystkie")
            {
                query = query.Where(l =>
                    l.SesjaTekst == selectedSession);
            }

            // Data od
            if (DateFromPicker.SelectedDate.HasValue)
            {
                var dateFrom =
                    DateFromPicker.SelectedDate.Value.Date;

                query = query.Where(l =>
                    l.DataLogowania >= dateFrom);
            }

            // Data do
            if (DateToPicker.SelectedDate.HasValue)
            {
                var dateTo =
                    DateToPicker.SelectedDate.Value.Date.AddDays(1);

                query = query.Where(l =>
                    l.DataLogowania < dateTo);
            }

            // Wyszukiwanie
            string search =
                SearchBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(l =>
                    l.Id.ToString().Contains(search) ||

                    l.Login.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    l.ImieNazwisko.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    l.Rola.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    l.DataLogowania
                        .ToString("dd.MM.yyyy HH:mm:ss")
                        .Contains(search, StringComparison.OrdinalIgnoreCase) ||

                    (l.DataWylogowania.HasValue &&
                     l.DataWylogowania.Value
                        .ToString("dd.MM.yyyy HH:mm:ss")
                        .Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    l.CzySukcesTekst.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    l.SesjaTekst.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    (l.AdresIp != null &&
                     l.AdresIp.Contains(
                         search,
                         StringComparison.OrdinalIgnoreCase))
                );
            }

            LoginLogsGrid.ItemsSource = query.ToList();
        }

        private void RoleFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SessionFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RoleFilterBox.SelectedIndex = 0;
            SessionFilterBox.SelectedIndex = 0;

            DateFromPicker.SelectedDate = null;
            DateToPicker.SelectedDate = null;

            SearchBox.Clear();

            ApplyFilters();
        }
    }
}