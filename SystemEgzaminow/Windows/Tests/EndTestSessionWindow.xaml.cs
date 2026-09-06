using System.Windows;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.WPF.Windows.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy EndTestSessionWindow.xaml
    /// </summary>
    public partial class EndTestSessionWindow : Window
    {
        private readonly EndTestSessionDto _result;
        private readonly SposobWyswietlaniaWynikuEnum _sposobWyswietlania;

        public EndTestSessionWindow(EndTestSessionDto result, SposobWyswietlaniaWynikuEnum sposobWyswietlania)
        {
            InitializeComponent();

            _result = result;
            _sposobWyswietlania = sposobWyswietlania;

            WyswietlWynik();
        }

        private void WyswietlWynik()
        {
            PointsResultTextBlock.Text =
        $"{_result.ZdobytePunkty} / {_result.MaksymalnePunkty}";

            PercentageResultTextBlock.Text =
                $"{_result.Procent:0.##}%";

            GradeResultTextBlock.Text = $"{_result.Ocena:0.##}";

            PassedResultTextBlock.Text =
                _result.CzyZaliczony
                    ? "Zaliczony"
                    : "Niezaliczony";

            if (_result.CzyZakonczonyAutomatycznie)
            {
                CompletionMessageTextBlock.Text =
                    "Czas minął. Test został zakończony automatycznie, a odpowiedzi zapisane.";
            }
            else
            {
                CompletionMessageTextBlock.Text =
                    "Test został zakończony, a odpowiedzi zapisane.";
            }
            if (!string.IsNullOrWhiteSpace(_result.InformacjaKoncowa))
            {
                CompletionMessageTextBlock.Text +=
                    $"\n\n{_result.InformacjaKoncowa}";
            }
            bool czyPokazacWynik = _result.CzyProbny || _result.CzyPokazacWynikPoZakonczeniu;
            if (!czyPokazacWynik)
            {
                ResultsBorder.Visibility = Visibility.Collapsed;
                HiddenResultsBorder.Visibility = Visibility.Visible;
                return;
            }
            switch (_sposobWyswietlania)
            {
                case SposobWyswietlaniaWynikuEnum.TylkoPotwierdzenie:
                    {
                        ResultsBorder.Visibility = Visibility.Collapsed;
                        HiddenResultsBorder.Visibility = Visibility.Visible;
                        AnswersBorder.Visibility = Visibility.Collapsed;
                        break;
                    }

                case SposobWyswietlaniaWynikuEnum.PunktyIProcent:
                    {
                        ResultsBorder.Visibility = Visibility.Visible;
                        HiddenResultsBorder.Visibility = Visibility.Collapsed;
                        AnswersBorder.Visibility = Visibility.Collapsed;

                        PointsResultRow.Visibility = Visibility.Visible;
                        PercentageResultRow.Visibility = Visibility.Visible;
                        GradeResultRow.Visibility = Visibility.Collapsed;
                        PassedResultRow.Visibility = Visibility.Collapsed;
                        break;
                    }

                case SposobWyswietlaniaWynikuEnum.PelnyWynik:
                    {
                        ResultsBorder.Visibility = Visibility.Visible;
                        HiddenResultsBorder.Visibility = Visibility.Collapsed;

                        UstawWidocznoscWynikuWedlugSposobuOceniania();
                        break;
                    }

                case SposobWyswietlaniaWynikuEnum.PelnyWynikZOdpowiedziami:
                    {
                        ResultsBorder.Visibility = Visibility.Visible;
                        HiddenResultsBorder.Visibility = Visibility.Collapsed;
                        AnswersBorder.Visibility = Visibility.Visible;

                        AnswersItemsControl.ItemsSource = _result.Pytania;

                        UstawWidocznoscWynikuWedlugSposobuOceniania();
                        break;
                    }

                default:
                    {
                        ResultsBorder.Visibility = Visibility.Collapsed;
                        HiddenResultsBorder.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        private void UstawWidocznoscWynikuWedlugSposobuOceniania()
        {
            PointsResultRow.Visibility = Visibility.Collapsed;
            PercentageResultRow.Visibility = Visibility.Collapsed;
            GradeResultRow.Visibility = Visibility.Collapsed;
            PassedResultRow.Visibility = Visibility.Collapsed;

            switch (_result.SposobOceniania)
            {
                case SposobOcenianiaEnum.Punkty:
                    PointsResultRow.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.Procent:
                    PercentageResultRow.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.PunktyIProcent:
                    PointsResultRow.Visibility = Visibility.Visible;
                    PercentageResultRow.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.Ocena:
                    GradeResultRow.Visibility = Visibility.Visible;
                    break;

                case SposobOcenianiaEnum.TylkoZaliczenie:
                    PassedResultRow.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}