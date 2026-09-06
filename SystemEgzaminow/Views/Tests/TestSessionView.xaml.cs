using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Windows.Tests;

namespace SystemEgzaminow.WPF.Views.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy TestSessionView.xaml
    /// </summary>
    public partial class TestSessionView : UserControl
    {
        private readonly TestSessionDto _testSession;
        private int _currentQuestionIndex;
        private TimeSpan _pozostalyCzas;
        private TimeSpan _progOstrzezenia;
        private TimeSpan _progAlarmu;
        private bool _czyTimerMiga;
        private bool _isFinishing;

        private readonly DispatcherTimer _timer;

        public TestSessionView(TestSessionDto testSession)
        {
            InitializeComponent();
            _testSession = testSession;
            _currentQuestionIndex = 0;

            int czasTrwaniaMinuty =
                Math.Max(1, testSession.CzasTrwaniaMinuty);

            _pozostalyCzas =
                TimeSpan.FromMinutes(czasTrwaniaMinuty);

            // Ostrzeżenie, gdy pozostaje 10% czasu.
            _progOstrzezenia =
                TimeSpan.FromTicks(
                    (long)(_pozostalyCzas.Ticks * 0.10));

            // Krótszy test: alarm od ostatniej minuty.
            // Dłuższy test: alarm od ostatnich 2%.
            _progAlarmu = czasTrwaniaMinuty <= 30
                ? TimeSpan.FromMinutes(1)
                : TimeSpan.FromTicks(
                    (long)(_pozostalyCzas.Ticks * 0.02));

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += Timer_Tick;

            AktualizujWidokTimera();
            _timer.Start();

            ShowCurrentQuestion();
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if (_isFinishing)
                return;

            _pozostalyCzas -= TimeSpan.FromSeconds(1);

            if (_pozostalyCzas <= TimeSpan.Zero)
            {
                _pozostalyCzas = TimeSpan.Zero;

                _timer.Stop();
                AktualizujWidokTimera();

                await FinishTestAsync(true);
                return;
            }

            AktualizujWidokTimera();
        }

        private void AktualizujWidokTimera()
        {
            RemainingTimeTextBlock.Text =
       _pozostalyCzas.ToString(@"hh\:mm\:ss");

            bool czyAlarm =
                _pozostalyCzas <= _progAlarmu;

            bool czyOstrzezenie =
                _pozostalyCzas <= _progOstrzezenia;

            if (czyAlarm)
            {
                RemainingTimeTextBlock.Foreground =
                    Brushes.Red;

                RemainingTimeTextBlock.FontWeight =
                    FontWeights.Bold;

                _czyTimerMiga = !_czyTimerMiga;

                RemainingTimeTextBlock.Opacity =
                    _czyTimerMiga ? 1.0 : 0.35;

                return;
            }

            _czyTimerMiga = false;
            RemainingTimeTextBlock.Opacity = 1.0;

            if (czyOstrzezenie)
            {
                RemainingTimeTextBlock.Foreground =
                    Brushes.Red;

                RemainingTimeTextBlock.FontWeight =
                    FontWeights.Normal;

                return;
            }

            RemainingTimeTextBlock.Foreground =
                Brushes.Green;

            RemainingTimeTextBlock.FontWeight =
                FontWeights.Normal;
        }

        private void ShowCurrentQuestion()
        {
            if (_testSession.Pytania.Count == 0)
                return;

            TestSessionQuestionDto question =
                _testSession.Pytania[_currentQuestionIndex];

            CurrentQuestionNumberTextBlock.Text =
                (_currentQuestionIndex + 1).ToString();

            QuestionsCountTextBlock.Text =
                _testSession.Pytania.Count.ToString();

            QuestionPointsTextBlock.Text =
                $"Liczba punktów: {question.LiczbaPunktow}";

            QuestionContentTextBlock.Text =
                question.TrescPytania;

            ShowQuestionType(question);
            UpdateNavigationButtons();
        }

        private void ShowQuestionType(TestSessionQuestionDto question)
        {
            SingleChoicePanel.Visibility = Visibility.Collapsed;
            MultipleChoicePanel.Visibility = Visibility.Collapsed;
            OpenAnswerPanel.Visibility = Visibility.Collapsed;

            OpenAnswerTextBox.DataContext = null;

            switch (question.TypPytania)
            {
                case TypPytaniaEnum.JednokrotnyWybor:
                    SingleChoicePanel.Visibility = Visibility.Visible;
                    SingleChoiceAnswersItemsControl.ItemsSource =
                        question.Odpowiedzi;
                    break;

                case TypPytaniaEnum.WielokrotnyWybor:
                    MultipleChoicePanel.Visibility = Visibility.Visible;
                    MultipleChoiceAnswersItemsControl.ItemsSource =
                        question.Odpowiedzi;
                    break;

                case TypPytaniaEnum.Otwarte:
                    OpenAnswerPanel.Visibility = Visibility.Visible;
                    OpenAnswerTextBox.DataContext = question;
                    break;
            }
        }

        private async Task FinishTestAsync(bool czyZakonczonyAutomatycznie)
        {
            if (_isFinishing)
                return;

            _isFinishing = true;
            _timer.Stop();
            FinishTestButton.IsEnabled = false;

            try
            {
                using var context =
                    DbContextHelper.CreateDbContext();

                var testSessionService =
                    new TestSessionService(context);

                EndTestSessionDto result =
                    await testSessionService.FinishTestSessionAsync(
                        _testSession,
                        czyProbny: _testSession.CzyProbny,
                        czyZakonczonyAutomatycznie);

                // później przekażemy result do EndTestSessionWindow

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow == null)
                    return;

                SposobWyswietlaniaWynikuEnum sposobWyswietlania;

                if (_testSession.CzyProbny)
                {
                    sposobWyswietlania =
                        SposobWyswietlaniaWynikuEnum.PelnyWynikZOdpowiedziami;
                }
                else if (result.CzyPokazacWynikPoZakonczeniu)
                {
                    sposobWyswietlania = result.SposobWyswietlaniaWyniku;
                }
                else
                {
                    sposobWyswietlania =
                        SposobWyswietlaniaWynikuEnum.TylkoPotwierdzenie;
                }

                var window = new EndTestSessionWindow(
                    result,
                    sposobWyswietlania)
                {
                    Owner = mainWindow
                };

                bool? dialogresult = window.ShowDialog();

                if (dialogresult != true)
                    return;

                if (_testSession.CzyProbny)
                {
                    mainWindow.ShowListsTestsView(1);
                }
                else
                {
                    mainWindow.ShowStudentView();
                }

                // przejście do do lista testów nauczyciel lub panel ucznia
            }
            catch (Exception ex)
            {
                _isFinishing = false;
                FinishTestButton.IsEnabled = true;

                string details = ex.InnerException?.Message ?? ex.Message;

                if (_pozostalyCzas > TimeSpan.Zero)
                {
                    _timer.Start();
                }

                Exception deepestException = ex;

                while (deepestException.InnerException != null)
                {
                    deepestException =
                        deepestException.InnerException;
                }
                MessageBox.Show(
                    $"Nie udało się zakończyć testu.\n\n{details}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void FinishTestButton_Click(object sender, RoutedEventArgs e)
        {
            await FinishTestAsync(false);
        }

        private void FirstQuestionButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            _currentQuestionIndex = 0;
            ShowCurrentQuestion();
        }

        private void PreviousQuestionButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_currentQuestionIndex <= 0)
                return;

            _currentQuestionIndex--;
            ShowCurrentQuestion();
        }

        private void NextQuestionButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_currentQuestionIndex >= _testSession.Pytania.Count - 1)
                return;

            _currentQuestionIndex++;
            ShowCurrentQuestion();
        }

        private void LastQuestionButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _currentQuestionIndex =
                _testSession.Pytania.Count - 1;

            ShowCurrentQuestion();
        }

        private void UpdateNavigationButtons()
        {
            bool isFirstQuestion =
                _currentQuestionIndex == 0;

            bool isLastQuestion =
                _currentQuestionIndex ==
                _testSession.Pytania.Count - 1;

            FirstQuestionButton.IsEnabled = !isFirstQuestion;
            PreviousQuestionButton.IsEnabled = !isFirstQuestion;

            NextQuestionButton.IsEnabled = !isLastQuestion;
            LastQuestionButton.IsEnabled = !isLastQuestion;

            FinishTestButton.Visibility =
                isLastQuestion
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }

        private void SingleChoiceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton radioButton ||
                radioButton.DataContext is not TestSessionAnswerDto selectedAnswer)
            {
                return;
            }

            TestSessionQuestionDto currentQuestion =
                _testSession.Pytania[_currentQuestionIndex];

            foreach (TestSessionAnswerDto answer in currentQuestion.Odpowiedzi)
            {
                answer.CzyWybrana =
                    answer.OdpowiedzId == selectedAnswer.OdpowiedzId;
            }
        }

        private void MultipleChoiceCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox checkBox ||
                checkBox.DataContext is not TestSessionAnswerDto answer)
            {
                return;
            }

            answer.CzyWybrana = checkBox.IsChecked == true;
        }
    }
}