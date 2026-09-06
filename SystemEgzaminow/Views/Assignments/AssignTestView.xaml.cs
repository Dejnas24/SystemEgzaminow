using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Assignments
{
    /// <summary>
    /// Logika interakcji dla klasy AssignTestView.xaml
    /// </summary>
    public partial class AssignTestView : UserControl
    {
        private readonly int _rolaId;

        public AssignTestView(int roleId)
        {
            InitializeComponent();
            _rolaId = roleId;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;
            if (LoggedUser.RolaId == 1)
                mainWindow.ShowAdminView();
            else if (LoggedUser.RolaId == 2)
                mainWindow.ShowTeacherView();
        }

        private async void AssignTestView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAssignmentSettings();
            // Prawy ComboBox
            RecipientTypeComboBox.ItemsSource =
                new List<string>
                {
                    "Uczeń",
                    "Klasa"
                };

            RecipientTypeComboBox.SelectedIndex = 0;

            // Lewa strona
            await LoadTestTypesAsync();

            // LoadTestTypesAsync ustawi "Wszystkie",
            // co uruchomi SelectionChanged i załaduje testy.
        }

        private void LoadAssignmentSettings()
        {
            // Status
            StatusComboBox.ItemsSource =
                Enum.GetValues<StatusPrzypisanegoTestu>();

            // Sposób oceniania
            ScoringModeComboBox.ItemsSource =
                Enum.GetValues<SposobOcenianiaEnum>();

            // Sposób wyświetlania wyniku
            ResultDisplayModeComboBox.ItemsSource =
                Enum.GetValues<SposobWyswietlaniaWynikuEnum>();

            // Godziny 00-23
            var godziny = Enumerable.Range(0, 24)
                .Select(h => h.ToString("00"))
                .ToList();

            AvailableFromHourComboBox.ItemsSource = godziny;
            AvailableToHourComboBox.ItemsSource = godziny;

            // Minuty co 1 minut
            var minuty = Enumerable.Range(0, 60)
    .Select(m => m.ToString("00"))
    .ToList();

            AvailableFromMinuteComboBox.ItemsSource = minuty;
            AvailableToMinuteComboBox.ItemsSource = minuty;

            // wartości domyślne
            AvailableFromHourComboBox.SelectedItem = "00";
            AvailableFromMinuteComboBox.SelectedItem = "00";

            AvailableToHourComboBox.SelectedItem = "00";
            AvailableToMinuteComboBox.SelectedItem = "00";

            StatusComboBox.SelectedIndex = 0;
            ScoringModeComboBox.SelectedIndex = 0;
            ResultDisplayModeComboBox.SelectedIndex = 0;
        }

        private async Task LoadTestTypesAsync()
        {
            using var context =
                   DbContextHelper.CreateDbContext();

            var service =
                new TestTypeService(context);

            var typyTestow =
                await service.GetAllAsync();

            var items = new List<TestTypeFilterDto>
            {
                new TestTypeFilterDto
                {
                    Id = null,
                    Nazwa = "Wszystkie"
                }
            };

            items.AddRange(
                typyTestow.Select(t => new TestTypeFilterDto
                {
                    Id = t.Id,
                    Nazwa = t.NazwaTypu
                }));

            TestTypeComboBox.ItemsSource = items;

            TestTypeComboBox.DisplayMemberPath =
                nameof(TestTypeFilterDto.Nazwa);

            TestTypeComboBox.SelectedValuePath =
                nameof(TestTypeFilterDto.Id);

            TestTypeComboBox.SelectedIndex = 0;
        }

        private async Task LoadTestsAsync(int? typTestuId = null)
        {
            using var context =
               DbContextHelper.CreateDbContext();

            var service =
                new AssignmentService(context);

            TestsDataGrid.ItemsSource =
                await service.GetTestsAsync(typTestuId);
        }

        private async Task LoadStudentsAsync()
        {
            using var context =
                DbContextHelper.CreateDbContext();

            var service =
                new AssignmentService(context);

            RecipientsDataGrid.ItemsSource =
                await service.GetStudentsAsync();
        }

        private async Task LoadClassesAsync()
        {
            using var context =
                DbContextHelper.CreateDbContext();

            var service =
                new AssignmentService(context);

            RecipientsDataGrid.ItemsSource =
                await service.GetClassesAsync();
        }

        private async void RecipientTypeComboBox_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
        {
            if (RecipientTypeComboBox.SelectedItem == null)
                return;

            string selectedType =
                RecipientTypeComboBox.SelectedItem.ToString()!;

            if (selectedType == "Uczeń")
            {
                await LoadStudentsAsync();
            }
            else if (selectedType == "Klasa")
            {
                await LoadClassesAsync();
            }
        }

        private async void TestTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestTypeComboBox.SelectedItem
               is not TestTypeFilterDto selected)
            {
                return;
            }

            await LoadTestsAsync(selected.Id);
        }

        private void ClearForm()
        {
            // Informacje
            StartInfoTextBox.Clear();
            EndInfoTextBox.Clear();

            // Daty
            AvailableFromDatePicker.SelectedDate = null;
            AvailableToDatePicker.SelectedDate = null;

            // Godzina
            AvailableFromHourComboBox.SelectedItem = "00";
            AvailableFromMinuteComboBox.SelectedItem = "00";

            AvailableToHourComboBox.SelectedItem = "00";
            AvailableToMinuteComboBox.SelectedItem = "00";

            // Liczba prób
            AttemptsTextBox.Text = "1";

            // Ustawienia
            StatusComboBox.SelectedIndex = 0;
            ScoringModeComboBox.SelectedIndex = 0;
            ResultDisplayModeComboBox.SelectedIndex = 0;

            // Checkbox
            ShowResultAfterFinishCheckBox.IsChecked = false;

            // Zaznaczenia
            TestsDataGrid.SelectedItem = null;
            RecipientsDataGrid.SelectedItems.Clear();
        }

        private async void AssignTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Wybierz test.");
                return;
            }

            if (RecipientsDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wybierz co najmniej jednego odbiorcę.");
                return;
            }

            if (AvailableFromDatePicker.SelectedDate == null ||
                AvailableToDatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Wybierz datę dostępności i datę wygaśnięcia.");
                return;
            }

            if (StatusComboBox.SelectedItem == null ||
                ScoringModeComboBox.SelectedItem == null ||
                ResultDisplayModeComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Uzupełnij ustawienia przypisania.");
                return;
            }

            if (!int.TryParse(AttemptsTextBox.Text, out int liczbaProb) ||
                liczbaProb < 1)
            {
                MessageBox.Show(
                    "Liczba prób musi być liczbą większą od zera.");
                return;
            }

            var selectedTest =
                (AssignTestListItemDto)TestsDataGrid.SelectedItem;

            int godzinaOd =
                int.Parse(
                    AvailableFromHourComboBox.SelectedItem!
                        .ToString()!);

            int minutaOd =
                int.Parse(
                    AvailableFromMinuteComboBox.SelectedItem!
                        .ToString()!);

            int godzinaDo =
                int.Parse(
                    AvailableToHourComboBox.SelectedItem!
                        .ToString()!);

            int minutaDo =
                int.Parse(
                    AvailableToMinuteComboBox.SelectedItem!
                        .ToString()!);

            DateTime dataDostepnosci =
                AvailableFromDatePicker.SelectedDate.Value.Date
                    .AddHours(godzinaOd)
                    .AddMinutes(minutaOd);

            DateTime dataWygasniecia =
                AvailableToDatePicker.SelectedDate.Value.Date
                    .AddHours(godzinaDo)
                    .AddMinutes(minutaDo);

            if (dataWygasniecia <= dataDostepnosci)
            {
                MessageBox.Show(
                    "Data wygaśnięcia musi być późniejsza niż data dostępności.");
                return;
            }

            var uczenIds = new List<int>();

            string recipientType =
                RecipientTypeComboBox.SelectedItem?.ToString()
                ?? string.Empty;

            if (recipientType == "Uczeń")
            {
                uczenIds = RecipientsDataGrid.SelectedItems
                    .Cast<AssignmentRecipientDto>()
                    .Select(x => x.Id)
                    .Distinct()
                    .ToList();
            }
            else if (recipientType == "Klasa")
            {
                var klasaIds = RecipientsDataGrid.SelectedItems
         .Cast<AssignmentRecipientDto>()
         .Select(x => x.Id)
         .Distinct()
         .ToList();

                using var context =
                    DbContextHelper.CreateDbContext();

                var service =
                    new AssignmentService(context);

                uczenIds =
                    await service.GetStudentIdsForClassesAsync(klasaIds);
            }

            if (uczenIds.Count == 0)
            {
                MessageBox.Show(
                    "Nie znaleziono uczniów do przypisania.");
                return;
            }
            var dto = new AssignTestDto
            {
                TestId = selectedTest.TestId,

                UczenIds = uczenIds,

                DataDostepnosci = dataDostepnosci,
                DataWygasniecia = dataWygasniecia,

                LiczbaProb = liczbaProb,

                Status =
            (StatusPrzypisanegoTestu)
            StatusComboBox.SelectedItem,

                SposobOceniania =
            (SposobOcenianiaEnum)
            ScoringModeComboBox.SelectedItem,

                SposobWyswietlaniaWyniku =
            (SposobWyswietlaniaWynikuEnum)
            ResultDisplayModeComboBox.SelectedItem,

                CzyPokazacWynikPoZakonczeniu =
            ShowResultAfterFinishCheckBox.IsChecked == true,

                InformacjaStartowa =
            string.IsNullOrWhiteSpace(StartInfoTextBox.Text)
                ? null
                : StartInfoTextBox.Text.Trim(),

                InformacjaKoncowa =
            string.IsNullOrWhiteSpace(EndInfoTextBox.Text)
                ? null
                : EndInfoTextBox.Text.Trim()
            };

            try
            {
                using var context =
                    DbContextHelper.CreateDbContext();

                var service =
                    new AssignmentService(context);

                await service.AssignTestAsync(
                    dto,
                    LoggedUser.Id);

                MessageBox.Show(
                    $"Test został przypisany dla {uczenIds.Count} ucznia/uczniów.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się przypisać testu.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}