using System.Windows;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Windows.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy StartTestSessionWindow.xaml
    /// </summary>
    public partial class StartTestSessionWindow : Window
    {
        private readonly StartTestSessionDto _dto;
        private readonly bool _czyProbny;

        public StartTestSessionWindow(StartTestSessionDto dto, bool czyProbny)
        {
            InitializeComponent();
            _dto = dto;
            _czyProbny = czyProbny;

            LoadTestData();
        }

        private void LoadTestData()
        {
            TestTitleTextBlock.Text = _dto.Tytul;

            DurationTextBlock.Text =
                $"{_dto.CzasTrwaniaMinuty} minut";

            MaximumPointsTextBlock.Text =
                $"{_dto.MaksymalnaLiczbaPunktow} punktów";

            PassingThresholdTextBlock.Text =
                $"{_dto.ProgZaliczenia}%";

            bool hasTeacherInformation =
                !string.IsNullOrWhiteSpace(_dto.InformacjaOdNauczyciela);

            TeacherInformationBorder.Visibility =
                hasTeacherInformation
                    ? Visibility.Visible
                    : Visibility.Collapsed;

            TeacherInformationTextBlock.Text =
                _dto.InformacjaOdNauczyciela ?? string.Empty;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_czyProbny && _dto.PrzypisanyTestId.HasValue)
            {
                using var context =
                    DbContextHelper.CreateDbContext();

                var service =
                    new TestSessionService(context);

                await service.MarkAssignedTestAsStartedAsync(
                    _dto.PrzypisanyTestId.Value);
            }

            DialogResult = true;
        }
    }
}