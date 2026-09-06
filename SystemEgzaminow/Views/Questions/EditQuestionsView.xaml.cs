using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Questions
{
    /// <summary>
    /// Logika interakcji dla klasy EditQuestionsView.xaml
    /// </summary>
    public partial class EditQuestionsView : UserControl
    {
        private List<QuestionListItem> _questions = new();
        private List<QuestionListItem> _allQuestions = new();
        private bool _isViewLoaded;
        private readonly int _roleId;

        public EditQuestionsView(int roleId)
        {
            _roleId = roleId;
            InitializeComponent();
            ConfigureViewForRole();
            _isViewLoaded = true;
            LoadQuestions();
            LoadAuthors();
        }

        private void LoadQuestions()
        {
            try
            {
                using var context = DbContextHelper.CreateDbContext();

                var service = new QuestionService(context);

                _allQuestions = service.GetQuestionsForCurrentUser();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd wczytywania pytań",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ConfigureViewForRole()
        {
            if (LoggedUser.RolaId == 1)
            {
                AuthorFilterPanel.Visibility = Visibility.Visible;
                DeleteColumn.Visibility = Visibility.Visible;
            }
            else
            {
                AuthorFilterPanel.Visibility = Visibility.Collapsed;
                DeleteColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadAuthors()
        {
            if (LoggedUser.RolaId != 1)
                return;

            AuthorFilterBox.Items.Clear();

            AuthorFilterBox.Items.Add(new ComboBoxItem
            {
                Content = "Wszyscy",
                Tag = null,
                IsSelected = true
            });

            var authors = _allQuestions
                .GroupBy(q => new
                {
                    q.IdAutora,
                    q.AutorPelnaNazwa
                })
                .Select(g => g.Key)
                .OrderBy(a => a.AutorPelnaNazwa)
                .ToList();

            foreach (var author in authors)
            {
                AuthorFilterBox.Items.Add(new ComboBoxItem
                {
                    Content = author.AutorPelnaNazwa,
                    Tag = author.IdAutora
                });
            }
        }

        private void ApplyFilters()
        {
            if (!_isViewLoaded || _allQuestions == null)
                return;
            IEnumerable<QuestionListItem> filteredQuestions = _allQuestions;

            // Autor — tylko administrator
            if (LoggedUser.RolaId == 1 &&
                AuthorFilterBox.SelectedItem is ComboBoxItem selectedAuthor &&
                selectedAuthor.Tag is int authorId)
            {
                filteredQuestions = filteredQuestions
                    .Where(q => q.IdAutora == authorId);
            }

            // Typ pytania
            int selectedTypeIndex = QuestionTypeFilterBox.SelectedIndex;

            if (selectedTypeIndex > 0)
            {
                var selectedType = selectedTypeIndex switch
                {
                    1 => TypPytaniaEnum.JednokrotnyWybor,
                    2 => TypPytaniaEnum.WielokrotnyWybor,
                    3 => TypPytaniaEnum.Otwarte,
                    _ => throw new InvalidOperationException("Nieznany typ pytania.")
                };

                filteredQuestions = filteredQuestions
                    .Where(q => q.TypPytania == selectedType);
            }

            // Status
            int selectedStatusIndex = StatusFilterBox.SelectedIndex;

            if (selectedStatusIndex == 1)
            {
                filteredQuestions = filteredQuestions
                    .Where(q => !q.CzyZarchiwizowane);
            }
            else if (selectedStatusIndex == 2)
            {
                filteredQuestions = filteredQuestions
                    .Where(q => q.CzyZarchiwizowane);
            }

            // Wyszukiwanie
            string searchText = SearchTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredQuestions = filteredQuestions.Where(q =>
                    q.TrescPytania.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.AutorPelnaNazwa.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.TypPytania.ToString().Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase) ||

                    q.LiczbaPunktow.ToString().Contains(searchText) ||

                    q.StatusPytania.Contains(
                        searchText,
                        StringComparison.CurrentCultureIgnoreCase));
            }

            var result = filteredQuestions.ToList();

            EditQuestionsGrid.ItemsSource = result;
            QuestionsCountTextBlock.Text = $"Liczba pytań: {result.Count}";
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            QuestionTypeFilterBox.SelectedIndex = 0;
            StatusFilterBox.SelectedIndex = 0;
            SearchTextBox.Clear();

            if (LoggedUser.RolaId == 1)
                AuthorFilterBox.SelectedIndex = 0;

            ApplyFilters();
            LoadAuthors();
        }

        private void EditQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
         button.DataContext is QuestionListItem question)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.ShowEditQuestionView(question.Id);
                }
            }
        }

        private void EditAnswers_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
         button.DataContext is QuestionListItem question)
            {
                var draft = new QuestionDraft
                {
                    TrescPytania = question!.TrescPytania,
                    LiczbaPunktow = question.LiczbaPunktow,
                    TypPytania = question.TypPytania,
                    SciezkaZdjecia = question.SciezkaZdjecia
                };

                var mainWindow = Window.GetWindow(this) as MainWindow;
                switch (question.TypPytania)
                {
                    case TypPytaniaEnum.JednokrotnyWybor:
                        mainWindow.ShowSingleChoiceAnswerView(
                            draft,
                            FormMode.Edit,
                            question.Id);
                        break;

                    case TypPytaniaEnum.WielokrotnyWybor:
                        mainWindow.ShowMultipleChoiceAnswerView(
                            draft,
                            FormMode.Edit,
                            question.Id);
                        break;

                    case TypPytaniaEnum.Otwarte:
                        mainWindow.ShowOpenAnswerView(
                            draft,
                            FormMode.Edit,
                            question.Id);
                        break;
                }
            }
        }

        private void ArchiveOrRestore_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
        button.DataContext is not QuestionListItem question)
            {
                return;
            }

            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var service = new QuestionService(context);

                if (question.CzyZarchiwizowane)
                {
                    service.RestoreQuestion(question.Id);

                    MessageBox.Show(
                        "Pytanie zostało przywrócone.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    service.ArchiveQuestion(question.Id);

                    MessageBox.Show(
                        "Pytanie zostało zarchiwizowane.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                LoadQuestions();
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
        button.DataContext is not QuestionListItem question)
            {
                return;
            }

            if (!question.CzyZarchiwizowane)
            {
                MessageBox.Show(
                    "Najpierw zarchiwizuj pytanie, a dopiero później możesz je trwale usunąć.",
                    "Usuwanie pytania",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var result = MessageBox.Show(
                $"Czy na pewno chcesz trwale usunąć pytanie:\n\n{question.TrescPytania}\n\n" +
                "Usunięte zostaną również wszystkie odpowiedzi przypisane do tego pytania.",
                "Trwałe usuwanie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using var context = DbContextHelper.CreateDbContext();
                var service = new QuestionService(context);

                service.DeleteQuestion(question.Id);

                MessageBox.Show(
                    "Pytanie oraz jego odpowiedzi zostały trwale usunięte.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadQuestions();
                LoadAuthors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.InnerException?.Message ?? ex.Message,
                    "Błąd usuwania",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AuthorFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void QuestionTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allQuestions == null || _allQuestions.Count == 0)
                return;
            ApplyFilters();
        }
    }
}