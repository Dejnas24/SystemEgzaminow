using System.Windows;
using System.Windows.Input;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Windows.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy AssignQuestionToTestWindow.xaml
    /// </summary>
    public partial class AssignQuestionToTestWindow : Window
    {
        private readonly QuestionService _questionService;

        private List<QuestionListItem> _allQuestions = new();

        public QuestionListItem? SelectedQuestion { get; private set; }

        public AssignQuestionToTestWindow()
        {
            InitializeComponent();
            var context = DbContextHelper.CreateDbContext();

            _questionService = new QuestionService(context);

            Loaded += AssignQuestionToTestWindow_Loaded;
        }

        private void AssignQuestionToTestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            try
            {
                _allQuestions = _questionService
                    .GetQuestionsForCurrentUser()
                    .Where(q => !q.CzyZarchiwizowane)
                    .ToList();

                QuestionsDataGrid.ItemsSource = _allQuestions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się załadować pytań.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Clear();

            LoadQuestions();
        }

        private void QuestionsDataGrid_MouseDoubleClick(
            object sender,
            MouseButtonEventArgs e)
        {
            AssignSelectedQuestion();
        }

        private void AssignQuestionButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AssignSelectedQuestion();
        }

        private void AssignSelectedQuestion()
        {
            if (QuestionsDataGrid.SelectedItem
                is not QuestionListItem selectedQuestion)
            {
                MessageBox.Show(
                    "Wybierz pytanie, które chcesz przypisać do testu.",
                    "Brak wybranego pytania",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            SelectedQuestion = selectedQuestion;

            DialogResult = true;
        }

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                QuestionsDataGrid.ItemsSource = _allQuestions;
                return;
            }

            var filteredQuestions = _allQuestions
                .Where(q =>
                    q.TrescPytania.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    q.AutorPelnaNazwa.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    q.LiczbaPunktow.ToString().Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase)
                    ||
                    q.TypPytania.ToString().Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            QuestionsDataGrid.ItemsSource = filteredQuestions;
        }
    }
}