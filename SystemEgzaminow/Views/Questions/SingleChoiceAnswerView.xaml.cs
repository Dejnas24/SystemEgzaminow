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
    /// Logika interakcji dla klasy SingleChoiceAnswerView.xaml
    /// </summary>
    public partial class SingleChoiceAnswerView : UserControl
    {
        private int _answersCount = 1;
        private readonly QuestionDraft _draft;
        private readonly FormMode _formMode;
        private readonly int? _questionId;
        private readonly TestDraft _testDraft;
        private readonly List<(TextBox TextBox, RadioButton RadioButton)> _answerInputs = new();

        public SingleChoiceAnswerView(QuestionDraft draft, FormMode formMode,
    int? questionId = null)
        {
            InitializeComponent();
            _draft = draft;
            _formMode = formMode;
            _questionId = questionId;

            _answerInputs.Add((AnswerTextBox1, CorrectAnswerRadio1));
            ConfigureView();
        }

        public SingleChoiceAnswerView(QuestionDraft draft, FormMode formMode, int? questionId, TestDraft testDraft) : this(draft, formMode, questionId)
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

            var answers = context.Odpowiedzi
                .Where(o => o.PytanieId == _questionId.Value)
                .OrderBy(o => o.Id)
                .ToList();

            AnswersPanel.Children.Clear();
            _answerInputs.Clear();
            _answersCount = 0;

            foreach (var answer in answers)
            {
                AddAnswerRow(
                    answer.TrescOdpowiedzi,
                    answer.CzyPoprawna,
                    answer.LiczbaPunktow);
            }
        }

        private void AddAnswerRow(string answerText,
    bool isCorrect,
    int? points)
        {
            _answersCount++;

            var border = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            var radioButton = new RadioButton
            {
                GroupName = "CorrectAnswerGroup",
                IsChecked = isCorrect,
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            var textBox = new TextBox
            {
                Width = 450,
                Height = 32,
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = answerText
            };

            stackPanel.Children.Add(radioButton);
            stackPanel.Children.Add(textBox);

            border.Child = stackPanel;
            AnswersPanel.Children.Add(border);

            _answerInputs.Add((textBox, radioButton));
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

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            AddAnswerRow(
        $"Odpowiedź {_answersCount + 1}",
        false,
        0);
        }

        private void ReadAnswersFromView()
        {
            _draft.Odpowiedzi.Clear();

            foreach (var input in _answerInputs)
            {
                string answerText = input.TextBox.Text.Trim();
                bool isCorrect = input.RadioButton.IsChecked == true;

                if (string.IsNullOrWhiteSpace(answerText))
                {
                    throw new Exception(
                        "Treść odpowiedzi nie może być pusta.");
                }

                _draft.Odpowiedzi.Add(new OdpowiedzDraft
                {
                    TrescOdpowiedzi = answerText,
                    CzyPoprawna = isCorrect,

                    // Punkty dostaje tylko odpowiedź poprawna.
                    LiczbaPunktow = isCorrect
                        ? _draft.LiczbaPunktow
                        : 0
                });
            }

            if (_draft.Odpowiedzi.Count < 2)
            {
                throw new Exception(
                    "Pytanie jednokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");
            }

            if (_draft.Odpowiedzi.Count(o => o.CzyPoprawna) != 1)
            {
                throw new Exception(
                    "Pytanie jednokrotnego wyboru musi mieć dokładnie jedną poprawną odpowiedź.");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadAnswersFromView();

                using var context = DbContextHelper.CreateDbContext();

                var service = new QuestionService(context);
                int? newQuestionId = null;

                if (_formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
                {
                    newQuestionId = service.SaveSingleChoiceQuestion(_draft);
                }
                else
                {
                    if (_questionId == null)
                        throw new Exception("Brak identyfikatora pytania.");

                    service.UpdateSingleChoiceAnswers(
                        _questionId.Value,
                        _draft.Odpowiedzi);
                }

                MessageBox.Show(
                    _formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest
                        ? "Pytanie zapisano poprawnie."
                        : "Odpowiedzi zaktualizowano poprawnie.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var mainWindow = (MainWindow)Application.Current.MainWindow;

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
                        mainWindow.ShowEditQuestionView(_questionId!.Value);
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
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow == null)
                    return;
                if (_formMode == FormMode.Add)
                {
                    if (LoggedUser.RolaId == 1)
                        mainWindow.ShowAdminView();
                    else if (LoggedUser.RolaId == 2)
                        mainWindow.ShowTeacherView();
                }
                else if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
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