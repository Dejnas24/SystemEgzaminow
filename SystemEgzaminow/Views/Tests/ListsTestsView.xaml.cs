using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Windows.Tests;

namespace SystemEgzaminow.WPF.Views.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy ListsTestsView.xaml
    /// </summary>
    public partial class ListsTestsView : UserControl
    {
        private List<TestListItem> _allTests = new();
        private readonly TestTypeService _testTypeService;
        private bool _isViewLoaded;
        private StartTestSessionDto _startSessionDto;
        private readonly int _roleId;

        public ListsTestsView(int roleId)
        {
            InitializeComponent();
            _roleId = roleId;
            ConfigureViewForRole();
            _isViewLoaded = true;
            LoadTests();
            var context = DbContextHelper.CreateDbContext();
            _testTypeService = new TestTypeService(context);
            _startSessionDto = new StartTestSessionDto();
        }

        private void ConfigureViewForRole()
        {
            if (_roleId == 2)
            {
                AuthorFilterPanel.Visibility = Visibility.Collapsed;
                AuthorColumn.Visibility = Visibility.Collapsed;
                DeleteColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadTests()
        {
            try
            {
                using var context = DbContextHelper.CreateDbContext();

                var service = new TestService(context);

                _allTests = service.GetTestsForCurrentUser();

                LoadAuthors();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd wczytywania testów",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void ListTest_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTestTypesAsync();
        }

        private async Task LoadTestTypesAsync()
        {
            try
            {
                var testTypes = await _testTypeService.GetAllAsync();

                testTypes.Insert(0, new TestTypeDto
                {
                    Id = 0,
                    NazwaTypu = "Wszyskie"
                });

                TestTypeFilterBox.ItemsSource = testTypes;
                TestTypeFilterBox.DisplayMemberPath = nameof(TestTypeDto.NazwaTypu);
                TestTypeFilterBox.SelectedValuePath = nameof(TestTypeDto.Id);
                TestTypeFilterBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się załadować typów testów.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadAuthors()
        {
            if (LoggedUser.RolaId != 1)
                return;

            AuthorFilterBox.Items.Clear();

            AuthorFilterBox.Items.Add(new ComboBoxItem
            {
                Content = "Wszyscy",
                Tag = null,
                IsSelected = true
            });

            var authors = _allTests
                .GroupBy(q => new
                {
                    q.IdAutora,
                    q.Autor
                })
                .Select(g => g.Key)
                .OrderBy(a => a.Autor)
                .ToList();

            foreach (var author in authors)
            {
                AuthorFilterBox.Items.Add(new ComboBoxItem
                {
                    Content = author.Autor,
                    Tag = author.IdAutora
                });
            }
        }

        private void ApplyFilters()
        {
            if (!_isViewLoaded || _allTests == null)
                return;
            IEnumerable<TestListItem> filteredTests = _allTests;

            // Autor — tylko administrator
            if (LoggedUser.RolaId == 1 &&
                AuthorFilterBox.SelectedItem is ComboBoxItem selectedAuthor &&
                selectedAuthor.Tag is int authorId)
            {
                filteredTests = filteredTests
                    .Where(q => q.IdAutora == authorId);
            }

            // Typ Testu
            if (TestTypeFilterBox.SelectedValue is int selectedTypeId &&
      selectedTypeId > 0)
            {
                filteredTests = filteredTests
                    .Where(t => t.IdTyp == selectedTypeId);
            }

            // Status
            int selectedStatusIndex = StatusFilterBox.SelectedIndex;

            if (selectedStatusIndex == 1)
            {
                filteredTests = filteredTests
                    .Where(q => !q.CzyZarchiwizowany);
            }
            else if (selectedStatusIndex == 2)
            {
                filteredTests = filteredTests
                    .Where(q => q.CzyZarchiwizowany);
            }

            //Data Utworzenia
            DateTime? dataUtworzenia = CreationDateFilterPicker.SelectedDate;

            if (dataUtworzenia.HasValue)
            {
                filteredTests = filteredTests.Where(d => d.DataUtworzenia.Date == dataUtworzenia.Value.Date);
            }

            // Wyszukiwanie
            string searchText = SearchTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredTests = filteredTests.Where(q =>
                    q.Tytul.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.Typ.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.LiczbaPytan.ToString().Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.CzasTrwaniaMinuty.ToString().Contains(searchText) ||
                    q.Prog.ToString().Contains(searchText) ||
                    q.Autor.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.DataUtworzenia.ToString().Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase));
            }
            var result = filteredTests.ToList();
            TestsGrid.ItemsSource = result;
            TestsCountTextBlock.Text = $"Liczba testów: {result.Count}";
        }

        //private void Filter_Click(object sender, RoutedEventArgs e)
        //{
        //    ApplyFilters();
        //}

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            TestTypeFilterBox.SelectedIndex = 0;
            StatusFilterBox.SelectedIndex = 0;
            SearchTextBox.Clear();
            CreationDateFilterPicker.SelectedDate = null;

            if (LoggedUser.RolaId == 1)
                AuthorFilterBox.SelectedIndex = 0;

            // LoadAuthors();
            // await LoadTestTypesAsync();
            ApplyFilters();
        }

        private async void EditTest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
        button.DataContext is not TestListItem test)
            {
                return;
            }
            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var service = new TestService(context);
                var testDraft = await service.GetTestDraftByIdAsync(test.Id);

                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow == null)
                    return;
                mainWindow.ShowAddAddTestView(testDraft, FormMode.EditToTest, LoggedUser.RolaId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                       ex.InnerException?.Message ?? ex.Message,
                       "Błąd",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
            }
        }

        private async void SolveTestTrial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
        button.DataContext is not TestListItem selectedTest)
            {
                return;
            }

            using var context = DbContextHelper.CreateDbContext();
            var testSesionService = new TestSessionService(context);

            StartTestSessionDto? startSessionDto =
                await testSesionService.GetStartTestSessionDtoAsync(
                    selectedTest.Id,
                    true);

            if (startSessionDto == null)
            {
                MessageBox.Show(
                    "Nie udało się pobrać danych testu.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            var window = new StartTestSessionWindow(startSessionDto, true)
            {
                Owner = mainWindow
            };

            bool? result = window.ShowDialog();

            if (result != true)
                return;

            //  mainWindow.ShowTestSessionView();

            // przejście do TestSessionView

            TestSessionDto? testSessionDto =
      await testSesionService.GetTestSessionDtoAsync(
          startSessionDto.TestId, startSessionDto.CzyProbny, startSessionDto.PrzypisanyTestId);

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

        private void ArchiveOrRestore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
        button.DataContext is not TestListItem test)
            {
                return;
            }

            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var service = new TestService(context);

                if (test.CzyZarchiwizowany)
                {
                    service.RestoreTest(test.Id);

                    MessageBox.Show(
                        "Test zostało przywrócony.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    service.ArchiveTest(test.Id);

                    MessageBox.Show(
                        "Test zostało zarchiwizowane.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                LoadTests();
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DeleteTest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
      button.DataContext is not TestListItem Test)
            {
                return;
            }

            if (!Test.CzyZarchiwizowany)
            {
                MessageBox.Show(
                    "Najpierw zarchiwizuj test, a dopiero później możesz go trwale usunąć.",
                    "Usuwanie testu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var result = MessageBox.Show(
                $"Czy na pewno chcesz trwale usunąć test:\n\n{Test.Tytul}\n\n",
                "Trwałe usuwanie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var service = new TestService(context);

                service.DeleteTest(Test.Id);

                MessageBox.Show(
                    "Test oraz jego powiązania relacyjne (TestPytania) zostały trwale usunięte.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadTests();
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd usuwania",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null)
                return;

            if (LoggedUser.RolaId == 1)
                mainWindow.ShowAdminView();
            else if (LoggedUser.RolaId == 2)
                mainWindow.ShowTeacherView();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allTests == null || _allTests.Count == 0)
                return;

            ApplyFilters();
        }

        private void AuthorFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void TestTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CreationDateFilterPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }
}