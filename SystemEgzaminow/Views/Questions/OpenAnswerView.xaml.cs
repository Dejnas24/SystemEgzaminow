using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Questions
{
    /// <summary>
    /// Logika interakcji dla klasy OpenAnswerView.xaml
    /// </summary>
    public partial class OpenAnswerView : UserControl
    {
        private readonly QuestionDraft _draft;
        private readonly FormMode _formMode;
        private readonly int? _questionId;
        private readonly TestDraft _testDraft;

        public OpenAnswerView(QuestionDraft draft, FormMode formMode,
    int? questionId = null)
        {
            InitializeComponent();
            _draft = draft;
            _formMode = formMode;
            _questionId = questionId;

            ConfigureView();
        }

        public OpenAnswerView(QuestionDraft draft, FormMode formMode, int? questionId, TestDraft testDraft) : this(draft, formMode, questionId)
        {
            _testDraft = testDraft;
        }

        private void ConfigureView()
        {
            QuestionTextBlock.Text = _draft.TrescPytania;
            PointsTextBlock.Text = $"Liczba punktów: {_draft.LiczbaPunktow}";

            if (_formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
            {
                HeaderTextBlock.Text = "Dodaj odpowiedzi jednego wyboru";
                SaveButton.Content = "Zapisz";
            }
            else
            {
                HeaderTextBlock.Text = "Edytuj odpowiedzi jednego wyboru";
                SaveButton.Content = "Zapisz zmiany";

                LoadExistingAnswers();
            }

            LoadQuestionImage();
        }

        private void LoadExistingAnswers()
        {
            if (_questionId == null)
                return;

            using var context = DbContextHelper.CreateDbContext();

            var answer = context.Odpowiedzi
                .FirstOrDefault(o => o.PytanieId == _questionId.Value);

            if (answer == null)
            {
                MessageBox.Show(
                    "Nie znaleziono odpowiedzi wzorcowej.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            OpenAnswerTextBox.Text = answer.TrescOdpowiedzi;
        }

        private void LoadQuestionImage()
        {
            QuestionImage.Source = null;

            if (string.IsNullOrWhiteSpace(_draft.SciezkaZdjecia) ||
                !File.Exists(_draft.SciezkaZdjecia))
            {
                NoImageTextBlock.Text = "Brak zdjęcia";
                NoImageTextBlock.Visibility = Visibility.Visible;
                return;
            }

            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(
                _draft.SciezkaZdjecia,
                UriKind.Absolute);
            bitmap.EndInit();

            QuestionImage.Source = bitmap;
            NoImageTextBlock.Visibility = Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string answerText = OpenAnswerTextBox.Text.Trim();
                int? newQuestionId = null;

                if (string.IsNullOrWhiteSpace(answerText))
                {
                    throw new Exception(
                        "Podaj odpowiedź wzorcową.");
                }

                using var context = DbContextHelper.CreateDbContext();
                var service = new QuestionService(context);

                if (_formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
                {
                    _draft.Odpowiedzi.Clear();

                    _draft.Odpowiedzi.Add(new OdpowiedzDraft
                    {
                        TrescOdpowiedzi = answerText,
                        CzyPoprawna = true,
                        LiczbaPunktow = _draft.LiczbaPunktow
                    });

                    newQuestionId = service.SaveOpenQuestion(_draft);
                }
                else
                {
                    if (_questionId == null)
                    {
                        throw new Exception(
                            "Brak identyfikatora edytowanego pytania.");
                    }

                    service.UpdateOpenAnswer(
                        _questionId.Value,
                        answerText,
                        _draft.LiczbaPunktow);
                }

                MessageBox.Show(
                    _formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest
                        ? "Pytanie zapisano poprawnie."
                        : "Odpowiedź wzorcową zaktualizowano poprawnie.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    if (_formMode == FormMode.Add)
                    {
                        mainWindow.ShowAddQuestionView(FormMode.Add, LoggedUser.RolaId);
                    }
                    else if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
                    {
                        mainWindow.ShowAddAddTestView(_testDraft, _formMode, newQuestionId, LoggedUser.RolaId);
                    }
                    else
                    {
                        mainWindow.ShowEditQuestionView(
                            _questionId!.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd zapisu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BackToQuestion_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            if (_formMode == FormMode.Add)
            {
                mainWindow.ShowAddQuestionView(FormMode.Add, _draft, LoggedUser.RolaId);
            }
            else if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
            {
                mainWindow.ShowAddQuestionView(_formMode, _testDraft, _draft, LoggedUser.RolaId);
            }
            else
            {
                if (_questionId == null)
                {
                    MessageBox.Show(
                        "Brak identyfikatora pytania.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                mainWindow.ShowEditQuestionView(_questionId.Value);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Czy na pewno chcesz anulować?",
                "Potwierdzenie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (_formMode == FormMode.Add)
                {
                    if (LoggedUser.RolaId == 1)
                        mainWindow.ShowAdminView();
                    else if (LoggedUser.RolaId == 2)
                        mainWindow.ShowTeacherView();
                }
                if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
                {
                    mainWindow.ShowAddAddTestView(_testDraft, _formMode, LoggedUser.RolaId);
                }
                else
                {
                    mainWindow.ShowEditQuestionsView(LoggedUser.RolaId);
                }
            }
        }
    }
}