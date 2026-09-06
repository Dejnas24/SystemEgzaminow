using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Windows.Tests;

namespace SystemEgzaminow.WPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy StudentView.xaml
    /// </summary>
    public partial class StudentView : UserControl
    {
        private List<StudentAssignedTestDto> _allAssignedTests = new();

        public StudentView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateAssignedTestStatusesAsync();
            await LoadTestTypesAsync();
            LoadStatuses();
            await LoadSchoolYearsAsync();
            await LoadAssignedTestsAsync();
        }

        private async Task UpdateAssignedTestStatusesAsync()
        {
            using var context =
        DbContextHelper.CreateDbContext();

            var service =
                new StudentService(context);

            await service.UpdateAssignedTestStatusesAsync(
                LoggedUser.Id);
        }

        private async Task LoadSchoolYearsAsync()
        {
            using var context =
                DbContextHelper.CreateDbContext();

            var service =
                new StudentService(context);

            var years =
                await service.GetAvailableSchoolYearsAsync(
                    LoggedUser.Id);

            var items = new List<string>
    {
        "Wszystkie"
    };

            items.AddRange(years);

            SchoolYearFilterComboBox.ItemsSource = items;
            SchoolYearFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadTestTypesAsync()
        {
            using var context =
         DbContextHelper.CreateDbContext();

            var service =
                new StudentService(context);

            var typyTestow =
                await service.GetAvailableTestTypesAsync(LoggedUser.Id);

            var items = new List<TestTypeFilterDto>
    {
        new TestTypeFilterDto
        {
            Id = null,
            Nazwa = "Wszystkie"
        }
    };

            items.AddRange(typyTestow);

            TestTypeFilterComboBox.ItemsSource = items;

            TestTypeFilterComboBox.DisplayMemberPath =
                nameof(TestTypeFilterDto.Nazwa);

            TestTypeFilterComboBox.SelectedValuePath =
                nameof(TestTypeFilterDto.Id);

            TestTypeFilterComboBox.SelectedIndex = 0;
        }

        private void LoadStatuses()
        {
            var statuses = new List<string>
    {
        "Wszystkie"
    };

            statuses.AddRange(
                Enum.GetNames<StatusPrzypisanegoTestu>());

            StatusFilterComboBox.ItemsSource = statuses;
            StatusFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadAssignedTestsAsync()
        {
            using var context =
         DbContextHelper.CreateDbContext();

            var service =
                new StudentService(context);

            _allAssignedTests =
                await service.GetAssignedTestsAsync(LoggedUser.Id);

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<StudentAssignedTestDto> query =
         _allAssignedTests;

            // Typ testu
            if (TestTypeFilterComboBox.SelectedItem
                is TestTypeFilterDto selectedType &&
                selectedType.Id.HasValue)
            {
                query = query.Where(t =>
                    t.TypTestu == selectedType.Nazwa);
            }

            // Status
            string? selectedStatus =
                StatusFilterComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(selectedStatus) &&
                selectedStatus != "Wszystkie")
            {
                query = query.Where(t =>
                    t.StatusTekst == selectedStatus);
            }

            // Rok szkolny
            string? selectedSchoolYear =
                SchoolYearFilterComboBox.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(selectedSchoolYear) &&
                selectedSchoolYear != "Wszystkie")
            {
                query = query.Where(t =>
                    t.RokSzkolny == selectedSchoolYear);
            }

            // Wyszukiwanie
            string search =
                SearchTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    t.Tytul.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    || t.TypTestu.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    || t.StatusTekst.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    || t.InformacjaTerminowa.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    || t.DataDostepnosci.ToString("dd.MM.yyyy HH:mm").Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)
                    || t.DataWygasniecia.ToString("dd.MM.yyyy HH:mm").Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase)

                    || t.LiczbaProb.ToString().Contains(search)

                     || t.PozostaleProby.ToString().Contains(search)

                     || (!string.IsNullOrWhiteSpace(t.RokSzkolny) &&
                        t.RokSzkolny.Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase))

                    );
            }

            TestyGrid.ItemsSource =
                query.ToList();
        }

        private async void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not StudentAssignedTestDto test)
                return;

            if (!test.CzyMoznaRozwiazac)
            {
                MessageBox.Show(
                    "Ten test nie jest obecnie dostępny do rozwiązania.");
                return;
            }

            using var context =
                DbContextHelper.CreateDbContext();

            var service =
                new TestSessionService(context);

            var startDto =
                await service.GetStartTestSessionDtoAsync(
                    test.TestId,
                    false,
                    test.PrzypisanyTestId);

            if (startDto == null)
            {
                MessageBox.Show(
                    "Nie udało się pobrać informacji o teście.");
                return;
            }

            // tutaj otwieramy istniejący ekran

            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            var window = new StartTestSessionWindow(
                startDto,
                false)
            {
                Owner = mainWindow
            };

            bool? result = window.ShowDialog();

            if (result != true)
                return;

            TestSessionDto? testSessionDto =
                await service.GetTestSessionDtoAsync(
                    startDto.TestId,
                    startDto.CzyProbny,
                    startDto.PrzypisanyTestId);

            if (testSessionDto == null ||
                testSessionDto.Pytania.Count == 0)
            {
                MessageBox.Show(
                    "Nie udało się pobrać pytań testu.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            testSessionDto.UserId = LoggedUser.Id;
            testSessionDto.DataRozpoczecia = DateTime.Now;

            mainWindow.ShowTestSessionView(testSessionDto);
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowLoginView();
            }
            await mainWindow.EndCurrentSessionAsync();

            mainWindow.ShowLoginView();
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
                await mainWindow.EndCurrentSessionAsync();
            Application.Current.Shutdown();
        }

        private async void ShowResult_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not StudentAssignedTestDto test)
                return;

            using var context =
                DbContextHelper.CreateDbContext();

            var service =
                new StudentService(context);

            var resultDto =
                await service.GetLatestTestResultAsync(
                    test.PrzypisanyTestId);

            if (resultDto == null)
            {
                MessageBox.Show(
                    "Nie znaleziono wyniku dla tego testu.",
                    "Brak wyniku",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            var window =
                new StudentTestResultWindow(resultDto)
                {
                    Owner = Window.GetWindow(this)
                };

            window.ShowDialog();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            TestTypeFilterComboBox.SelectedIndex = 0;
            StatusFilterComboBox.SelectedIndex = 0;
            SchoolYearFilterComboBox.SelectedIndex = 0;

            SearchTextBox.Clear();

            await LoadAssignedTestsAsync();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Filters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}