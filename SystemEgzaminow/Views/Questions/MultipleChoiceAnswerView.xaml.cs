using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Questions
{
    /// <summary>
    /// Logika interakcji dla klasy MultipleChoiceAnswerView.xaml
    /// </summary>
    public partial class MultipleChoiceAnswerView : UserControl
    {
        private int _answersCount = 1;

        private readonly QuestionDraft _draft;

        private readonly FormMode _formMode;
        private readonly int? _questionId;
        private readonly TestDraft _testDraft;
        private readonly List<(TextBox AnswerTextBox, CheckBox CheckBox, TextBox PointsTextBox)> _answerInputs = new();

        public MultipleChoiceAnswerView(QuestionDraft draft, FormMode formMode, int? questionId = null)
        {
            InitializeComponent();
            _draft = draft;
            _formMode = formMode;
            _questionId = questionId;

            ConfigureView();
            _answerInputs.Add((AnswerTextBox1, CorrectAnswerCheckBox1, PointsAnswerTextBox1));
        }

        public MultipleChoiceAnswerView(QuestionDraft draft, FormMode formMode, int? questionId, TestDraft testDraft) : this(draft, formMode, questionId)
        {
            _testDraft = testDraft;
        }

        private void ConfigureView()
        {
            QuestionTextBlock.Text = _draft.TrescPytania;
            PointsTextBlock.Text = $"Liczba punktów: {_draft.LiczbaPunktow}";

            if (_formMode == FormMode.Add || _formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
            {
                HeaderTextBlock.Text = "Dodaj odpowiedzi wielokrotnego wyboru";
                SaveButton.Content = "Zapisz";
            }
            else
            {
                HeaderTextBlock.Text = "Edytuj odpowiedzi wielokrotnego wyboru";
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

        private void AddAnswerRow(string answerText, bool isCorrect, int? points)
        {
            _answersCount++;

            var border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var grid = new Grid();

            grid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = new GridLength(35) });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = new GridLength(120) });

            var checkBox = new CheckBox
            {
                IsChecked = isCorrect,
                VerticalAlignment = VerticalAlignment.Center
            };

            checkBox.Checked += CorrectAnswerCheckBox_Checked;
            checkBox.Unchecked += CorrectAnswerCheckBox_Unchecked;

            Grid.SetColumn(checkBox, 0);

            var answerTextBox = new TextBox
            {
                Height = 32,
                Margin = new Thickness(10, 0, 10, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = answerText
            };

            Grid.SetColumn(answerTextBox, 1);

            var pointsTextBox = new TextBox
            {
                Height = 32,
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = points.ToString(),
                IsEnabled = isCorrect
            };

            Grid.SetColumn(pointsTextBox, 2);

            grid.Children.Add(checkBox);
            grid.Children.Add(answerTextBox);
            grid.Children.Add(pointsTextBox);

            border.Child = grid;
            AnswersPanel.Children.Add(border);

            _answerInputs.Add(
                (answerTextBox, checkBox, pointsTextBox));
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

        private void CorrectAnswerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
                checkBox.Parent is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is TextBox textBox && Grid.GetColumn(textBox) == 2)
                    {
                        textBox.IsEnabled = true;
                        if (textBox.Text == "0")
                            textBox.Text = "1";
                    }
                }
            }
        }

        private void CorrectAnswerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
                checkBox.Parent is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is TextBox textBox && Grid.GetColumn(textBox) == 2)
                    {
                        textBox.Text = "0";
                        textBox.IsEnabled = false;
                    }
                }
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
                    newQuestionId = service.SaveMultipleChoiceQuestion(_draft);
                }
                else
                {
                    if (_questionId == null)
                        throw new Exception("Brak identyfikatora edytowanego pytania.");

                    service.UpdateMultipleChoiceAnswers(
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

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    if (_formMode == FormMode.Add)
                        mainWindow.ShowAddQuestionView(FormMode.Add, LoggedUser.RolaId);
                    else if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
                    {
                        mainWindow.ShowAddAddTestView(_testDraft, _formMode, newQuestionId, LoggedUser.RolaId);
                    }
                    else
                        mainWindow.ShowEditQuestionView(_questionId!.Value);
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

        private void ReadAnswersFromView()
        {
            _draft.Odpowiedzi.Clear();

            foreach (var input in _answerInputs)
            {
                string answerText = input.AnswerTextBox.Text.Trim();
                bool isCorrect = input.CheckBox.IsChecked == true;

                if (string.IsNullOrWhiteSpace(answerText))
                    throw new Exception("Treść odpowiedzi nie może być pusta.");

                int points = 0;

                if (isCorrect)
                {
                    if (!int.TryParse(
                            input.PointsTextBox.Text,
                            out points))
                    {
                        throw new Exception(
                            "Liczba punktów odpowiedzi musi być liczbą.");
                    }

                    if (points <= 0)
                    {
                        throw new Exception(
                            "Poprawna odpowiedź musi mieć liczbę punktów większą od 0.");
                    }
                }

                _draft.Odpowiedzi.Add(new OdpowiedzDraft
                {
                    TrescOdpowiedzi = answerText,
                    CzyPoprawna = isCorrect,
                    LiczbaPunktow = points
                });
            }

            if (_draft.Odpowiedzi.Count < 2)
            {
                throw new Exception(
                    "Pytanie wielokrotnego wyboru musi mieć co najmniej dwie odpowiedzi.");
            }

            if (!_draft.Odpowiedzi.Any(o => o.CzyPoprawna))
            {
                throw new Exception(
                    "Co najmniej jedna odpowiedź musi być poprawna.");
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