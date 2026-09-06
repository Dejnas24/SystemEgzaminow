using Microsoft.Win32;
using System.Windows;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.WPF.Services;

namespace SystemEgzaminow.WPF.Windows.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy StudentTestResultWindow.xaml
    /// </summary>
    public partial class StudentTestResultWindow : Window
    {
        private readonly StudentTestResultDto _result;

        public StudentTestResultWindow(StudentTestResultDto result)
        {
            InitializeComponent();
            _result = result;

            LoadResult();
        }

        private void LoadResult()
        {
            TestTitleTextBlock.Text =
        _result.TytulTestu;

            TestTypeTextBlock.Text =
                _result.TypTestu;

            StudentNameTextBlock.Text =
                _result.ImieNazwiskoUcznia;

            PassedTextBlock.Text =
                _result.CzyZaliczony
                    ? "Tak"
                    : "Nie";

            PercentageTextBlock.Text =
                $"{_result.Procent:0.##}%";

            PointsTextBlock.Text =
      $"{_result.ZdobytePunkty} / {_result.MaksymalnePunkty}";

            GradeTextBlock.Text =
                _result.Ocena > 0
                    ? _result.Ocena.ToString("0.##")
                    : "-";

            AttemptTextBlock.Text =
    $"{_result.NumerProby} z {_result.LiczbaProb}";

            ClassTextBlock.Text =
                string.IsNullOrWhiteSpace(_result.Klasa)
                    ? "-"
                    : _result.Klasa;

            SchoolYearTextBlock.Text =
                string.IsNullOrWhiteSpace(_result.RokSzkolny)
                    ? "-"
                    : _result.RokSzkolny;

            FinishDateTextBlock.Text =
                _result.DataZakonczenia.ToString(
                    "dd.MM.yyyy HH:mm");

            MaxPointsTextBlock.Text =
                _result.MaksymalnePunkty.ToString();

            MinimumPercentageTextBlock.Text =
                $"{_result.MinimalnyProgProcentowy}%";

            MinimumPointsTextBlock.Text =
     $"{_result.MinimalnaLiczbaPunktow} / {_result.MaksymalnePunkty}";
            UstawWidocznoscWynikuWedlugSposobuOceniania();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SavePdfButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Plik PDF (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                AddExtension = true,
                FileName = $"Wynik_{_result.TytulTestu}.pdf"
            };
            if (saveFileDialog.ShowDialog() != true)
                return;

            try
            {
                var pdfService = new PdfService();

                pdfService.GenerateStudentTestResultPdf(
                    _result,
                    saveFileDialog.FileName);

                MessageBox.Show(
                    "Wynik testu został zapisany do pliku PDF.",
                    "Zapisano PDF",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się zapisać pliku PDF.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UstawWidocznoscWynikuWedlugSposobuOceniania()
        {
            PassedResultPanel.Visibility = Visibility.Collapsed;
            PercentageResultPanel.Visibility = Visibility.Collapsed;
            PointsResultPanel.Visibility = Visibility.Collapsed;
            GradeResultPanel.Visibility = Visibility.Collapsed;

            switch (_result.SposobOceniania)
            {
                case SposobOcenianiaEnum.Punkty:
                    PointsResultPanel.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.Procent:
                    PercentageResultPanel.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.PunktyIProcent:
                    PointsResultPanel.Visibility = Visibility.Visible;
                    PercentageResultPanel.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.Ocena:
                    GradeResultPanel.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.TylkoZaliczenie:
                    PassedResultPanel.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}