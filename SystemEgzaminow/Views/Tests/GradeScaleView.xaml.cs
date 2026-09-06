using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy GradeScaleView.xaml
    /// </summary>
    public partial class GradeScaleView : UserControl
    {
        private GradeScaleDto? _currentScale;
        private ObservableCollection<GradeThresholdDto> _thresholds = new();
        private readonly int _rolaId;

        public GradeScaleView(int roleId)
        {
            InitializeComponent();
            _rolaId = roleId;
        }

        private async void GradeScaleView_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = DbContextHelper.CreateDbContext();

            var service = new GradeScaleService(context);

            var testTypes = await service.GetTestTypesAsync();

            TestTypeBox.ItemsSource = testTypes;
            TestTypeBox.DisplayMemberPath = "NazwaTypu";
            TestTypeBox.SelectedValuePath = "Id";
        }

        private void RecalculateThresholdStarts()
        {
            if (_thresholds.Count == 0)
                return;

            // Pierwszy przedział zawsze zaczyna się od 0
            _thresholds[0].ProgOd = 0.00m;

            // Każdy następny zaczyna się 0.01 po poprzednim
            for (int i = 1; i < _thresholds.Count; i++)
            {
                _thresholds[i].ProgOd =
                    _thresholds[i - 1].ProgDo + 0.01m;
            }
        }

        private bool ValidateThresholds()
        {
            if (_thresholds.Count == 0)
            {
                MessageBox.Show(
                    "Skala musi zawierać co najmniej jeden próg oceny.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            // Pierwszy próg zawsze od 0
            _thresholds[0].ProgOd = 0.00m;

            // Ostatni próg zawsze do 100
            _thresholds[^1].ProgDo = 100.00m;

            RecalculateThresholdStarts();

            foreach (var threshold in _thresholds)
            {
                if (threshold.ProgOd < 0 ||
                    threshold.ProgOd > 100 ||
                    threshold.ProgDo < 0 ||
                    threshold.ProgDo > 100)
                {
                    MessageBox.Show(
                        "Progi procentowe muszą mieścić się w zakresie od 0 do 100.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return false;
                }

                if (threshold.ProgDo < threshold.ProgOd)
                {
                    MessageBox.Show(
                        $"Nieprawidłowy zakres dla oceny {threshold.Ocena}.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return false;
                }
            }

            return true;
        }

        private void DeleteThreshold_Click(object sender, RoutedEventArgs e)
        {
            if (_currentScale == null)
                return;
            if (_currentScale?.CzyDomyslna == true)
            {
                MessageBox.Show(
                    "Nie można modyfikować skali domyślnej.",
                    "Informacja",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }
            if (sender is Button button && button.DataContext is GradeThresholdDto threshold)
            {
                _thresholds.Remove(threshold);
                RecalculateThresholdStarts();
            }
        }

        private void AddThreshold_Click(object sender, RoutedEventArgs e)
        {
            if (_currentScale == null || _currentScale.CzyDomyslna)
                return;

            _thresholds.Add(new GradeThresholdDto
            {
                Ocena = 0,
                ProgOd = 0,
                ProgDo = 0
            });
            RecalculateThresholdStarts();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_currentScale == null)
                return;

            if (_currentScale.CzyDomyslna)
            {
                MessageBox.Show(
                    "Skala domyślna nie może być modyfikowana.",
                    "Informacja",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            string nazwa = ScaleNameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(nazwa))
            {
                MessageBox.Show(
                    "Podaj nazwę skali ocen.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (_thresholds.Count == 0)
            {
                MessageBox.Show(
                    "Skala musi zawierać co najmniej jeden próg oceny.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }
            if (!ValidateThresholds())
                return;

            _currentScale.Nazwa = nazwa;
            _currentScale.Progi = _thresholds.ToList();

            using var context = DbContextHelper.CreateDbContext();

            var service = new GradeScaleService(context);

            bool result = await service.UpdateIndividualScaleAsync(
                _currentScale,
                LoggedUser.Id);

            if (!result)
            {
                MessageBox.Show(
                    "Nie udało się zapisać skali ocen.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            MessageBox.Show(
                "Skala ocen została zapisana.",
                "Sukces",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private async void RestoreDefault_Click(object sender, RoutedEventArgs e)
        {
            if (_currentScale == null ||
      _currentScale.CzyDomyslna)
                return;

            if (TestTypeBox.SelectedValue is not int idTypuTestu)
                return;

            var result = MessageBox.Show(
                "Czy na pewno chcesz przywrócić skalę domyślną dla tego typu testu?",
                "Przywróć skalę domyślną",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var service = new GradeScaleService(context);

            bool restored = await service.RestoreDefaultScaleAsync(
                idTypuTestu,
                LoggedUser.Id);

            if (!restored)
                return;

            // Ponownie pobieramy aktywną skalę.
            // Ponieważ własna została dezaktywowana,
            // serwis zwróci globalną domyślną.
            var scale = await service.GetActiveScaleAsync(
                idTypuTestu,
                LoggedUser.Id);

            _currentScale = scale;

            if (scale == null)
                return;

            ScaleNameBox.Text = scale.Nazwa;

            _thresholds =
                new ObservableCollection<GradeThresholdDto>(
                    scale.Progi);

            ThresholdsItemsControl.ItemsSource = _thresholds;

            SetEditMode(scale.CzyDomyslna);

            MessageBox.Show(
                "Przywrócono domyślną skalę ocen.",
                "Sukces",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
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

        private async void TestTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestTypeBox.SelectedValue is not int idTypuTestu)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var service = new GradeScaleService(context);

            var scale = await service.GetActiveScaleAsync(idTypuTestu, LoggedUser.Id);
            _currentScale = scale;

            if (scale == null)
            {
                ScaleNameBox.Clear();
                ThresholdsItemsControl.ItemsSource = null;
                return;
            }

            ScaleNameBox.Text = scale.Nazwa;

            _thresholds = new ObservableCollection<GradeThresholdDto>(scale.Progi);
            ThresholdsItemsControl.ItemsSource = _thresholds;
            SetEditMode(scale.CzyDomyslna);
        }

        private void SetEditMode(bool czyDomyslna)
        {
            ScaleNameBox.IsReadOnly = czyDomyslna;

            AddThresholdButton.IsEnabled = !czyDomyslna;
            SaveButton.IsEnabled = !czyDomyslna;

            RestoreDefaultButton.IsEnabled = !czyDomyslna;

            CreateIndividualButton.IsEnabled = czyDomyslna;
        }

        private async void CreateIndividual_Click(object sender, RoutedEventArgs e)
        {
            if (TestTypeBox.SelectedValue is not int idTypuTestu)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var service = new GradeScaleService(context);

            var scale = await service.CreateIndividualScaleAsync(
                idTypuTestu,
                LoggedUser.Id);

            if (scale == null)
            {
                MessageBox.Show(
                    "Nie znaleziono domyślnej skali ocen.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            _currentScale = scale;

            ScaleNameBox.Text = scale.Nazwa;
            _thresholds = new ObservableCollection<GradeThresholdDto>(scale.Progi);
            ThresholdsItemsControl.ItemsSource = _thresholds;

            SetEditMode(scale.CzyDomyslna);
        }

        private void ProgDo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentScale == null)
                return;

            if (_currentScale.CzyDomyslna)
                return;

            RecalculateThresholdStarts();
        }
    }
}