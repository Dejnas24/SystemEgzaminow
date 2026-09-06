using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF.Windows.Tests;

namespace SystemEgzaminow.WPF.Views.Tests
{
    /// <summary>
    /// Logika interakcji dla klasy AddTestView.xaml
    /// </summary>
    public partial class AddTestView : UserControl
    {
        private readonly TestTypeService _testTypeService;
        private readonly TestService _testService;
        private readonly ObservableCollection<AssignedQuestionDto> _assignedQuestions = new();
        private readonly TestDraft _testDraft;
        private readonly FormMode _formMode = FormMode.AddToTest;
        private readonly int? _newQuestionId = null;
        private readonly int _rolaId;

        public AddTestView(int rolaId)
        {
            _rolaId = rolaId;
            InitializeComponent();
            AssignedQuestionsItemsControl.ItemsSource = _assignedQuestions;
            var context = DbContextHelper.CreateDbContext();
            _testTypeService = new TestTypeService(context);
            _testService = new TestService(context);
            AssignedQuestionsItemsControl.ItemsSource = _assignedQuestions;

            _assignedQuestions.CollectionChanged += (_, _) =>
            {
                UpdateQuestionsView();
            };
            Loaded += AddTestView_Loaded;

            UpdateQuestionsView();
        }

        public AddTestView(TestDraft testDraft, FormMode formMode, int roleId) : this(roleId)
        {
            _testDraft = testDraft;
            _formMode = formMode;
        }

        public AddTestView(TestDraft testDraft, FormMode formMode, int? newQuestionId, int roleId) : this(testDraft, formMode, roleId)
        {
            _newQuestionId = newQuestionId;
        }

        private void UpdateQuestionsView()
        {
            int totalQuestions = _assignedQuestions.Count;

            for (int i = 0; i < totalQuestions; i++)
            {
                var question = _assignedQuestions[i];

                question.NumerPytania = i + 1;
                question.LiczbaWszystkichPytan = totalQuestions;

                question.CzyMoznaPrzesunacWGore = i > 0;
                question.CzyMoznaPrzesunacWDol = i < totalQuestions - 1;
            }

            QuestionsCountTextBlock.Text = totalQuestions.ToString();

            AssignedQuestionsItemsControl.Items.Refresh();
        }

        private async void AddTestView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_formMode == FormMode.EditToTest)
            {
                ViewTitleTextBlock.Text = "Edytuj test";
                SaveTestButton.Content = "Zapisz zmiany";
            }
            else
            {
                ViewTitleTextBlock.Text = "Dodaj test";
                SaveTestButton.Content = "Zapisz test";
            }

            await LoadTestTypesAsync();

            if (_testDraft != null)
            {
                RestoreFormFromTestDraft(_testDraft);
                await RestoreAssignedQuestionsAsync(_testDraft);
            }

            if (_newQuestionId.HasValue)
            {
                var newQuestion =
                    await _testService.GetAssignedQuestionByIdAsync(
                        _newQuestionId.Value);

                if (!_assignedQuestions.Any(
                        p => p.Id == newQuestion.Id))
                {
                    _assignedQuestions.Add(newQuestion);
                }
            }
        }

        private async Task LoadTestTypesAsync()
        {
            try
            {
                var testTypes = await _testTypeService.GetAllAsync();

                testTypes.Insert(0, new TestTypeDto
                {
                    Id = 0,
                    NazwaTypu = "Wybierz typ testu"
                });

                TestTypeComboBox.ItemsSource = testTypes;

                TestTypeComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się załadować typów testów.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private TestDraft CreateTestDraftFromForm()
        {
            return new TestDraft
            {
                TestId = _formMode == FormMode.EditToTest ? _testDraft?.TestId : null,
                Tytul = TitleTextBox.Text,
                IdTypuTestu = TestTypeComboBox.SelectedValue is int id ? id : 0,
                Opis = DescriptionTextBox.Text,
                ProgZdania = int.TryParse(PassThresholdTextBox.Text.Trim(), out var prog) ? prog : 0,
                CzasTrwaniaMinuty = int.TryParse(DurationTextBox.Text.Trim(), out var czas) ? czas : 0,
                LosujKolejnoscPytan = RandomizeQuestionsCheckBox.IsChecked == true,
                LosujKolejnoscOdpowiedzi = RandomizeAnswersCheckBox.IsChecked == true,

                PrzypisanePytania = _assignedQuestions.Select((p, index) => new PrzypisanePytaniaDraft
                {
                    PytanieId = p.Id,
                    TrescPytania = p.TrescPytania,
                    TypPytania = p.TypPytania,
                    LiczbaPunktow = p.LiczbaPunktow,
                    SciezkaZdjecia = p.SciezkaZdjecia,
                    Kolejnosc = index + 1
                }).ToList()
            };
        }

        private void RestoreFormFromTestDraft(TestDraft testDraft)
        {
            TitleTextBox.Text = testDraft.Tytul;
            TestTypeComboBox.SelectedValue = testDraft.IdTypuTestu;
            DescriptionTextBox.Text = testDraft.Opis;
            PassThresholdTextBox.Text = testDraft.ProgZdania.ToString();
            DurationTextBox.Text = testDraft.CzasTrwaniaMinuty.ToString();
            RandomizeQuestionsCheckBox.IsChecked = testDraft.LosujKolejnoscPytan;
            RandomizeAnswersCheckBox.IsChecked = testDraft.LosujKolejnoscOdpowiedzi;
        }

        private async Task RestoreAssignedQuestionsAsync(TestDraft testDraft)
        {
            _assignedQuestions.Clear();

            foreach (var draftQuestion in testDraft.PrzypisanePytania
                         .OrderBy(p => p.Kolejnosc))
            {
                var question = await _testService
                    .GetAssignedQuestionByIdAsync(draftQuestion.PytanieId);

                _assignedQuestions.Add(question);
            }
        }

        private async void SaveTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_formMode == FormMode.AddToTest)
                {
                    CreateTestDto dto = ReadTestFromView();

                    int testId = await _testService.SaveTestAsync(dto);

                    MessageBox.Show(
                        $"Test został zapisany poprawnie.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    ClearForm();
                }
                if (_formMode == FormMode.EditToTest)
                {
                    if (_testDraft?.TestId is not int testId)
                    {
                        throw new InvalidOperationException(
                            "Nie można zaktualizować testu, ponieważ brakuje jego identyfikatora.");
                    }
                    CreateTestDto dto = ReadTestFromView();
                    await _testService.UpdateTestAsync(testId, dto);
                    MessageBox.Show(
                        $"Test został zaktualizowany poprawnie.",
                        "Sukces",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    var mainWindow = Window.GetWindow(this) as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.ShowListsTestsView(LoggedUser.RolaId);
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

        private CreateTestDto ReadTestFromView()
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Tytuł testu nie może być pusty.");

            if (TestTypeComboBox.SelectedItem is not TestTypeDto selectedTestType ||
                selectedTestType.Id == 0)
            {
                throw new Exception("Wybierz typ testu.");
            }

            if (!int.TryParse(
                    PassThresholdTextBox.Text.Trim(),
                    out int passingThreshold))
            {
                throw new Exception("Próg zdania musi być liczbą całkowitą.");
            }

            if (passingThreshold < 0 || passingThreshold > 100)
            {
                throw new Exception("Próg zdania musi mieścić się w zakresie od 0 do 100%.");
            }

            if (!int.TryParse(
                    DurationTextBox.Text.Trim(),
                    out int durationMinutes))
            {
                throw new Exception("Czas trwania musi być liczbą całkowitą.");
            }

            if (durationMinutes <= 0)
            {
                throw new Exception("Czas trwania testu musi być większy od 0 minut.");
            }

            if (_assignedQuestions.Count == 0)
            {
                throw new Exception("Test musi zawierać co najmniej jedno pytanie.");
            }

            return new CreateTestDto
            {
                Tytul = title,
                Opis = description,
                TypTestuId = selectedTestType.Id,
                ProgZdania = passingThreshold,
                CzasTrwaniaMinuty = durationMinutes,
                LosujKolejnoscPytan =
                    RandomizeQuestionsCheckBox.IsChecked == true,
                LosujKolejnoscOdpowiedzi =
                    RandomizeAnswersCheckBox.IsChecked == true,

                Pytania = _assignedQuestions
                    .Select((question, index) => new CreateTestQuestionDto
                    {
                        PytanieId = question.Id,
                        Kolejnosc = index + 1
                    })
                    .ToList()
            };
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();

            TestTypeComboBox.SelectedIndex = 0;

            PassThresholdTextBox.Clear();
            DurationTextBox.Clear();

            RandomizeQuestionsCheckBox.IsChecked = false;
            RandomizeAnswersCheckBox.IsChecked = false;

            _assignedQuestions.Clear();

            UpdateQuestionsView();

            TitleTextBox.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null)
                return;

            if (_formMode == FormMode.EditToTest)
            {
                mainWindow.ShowListsTestsView(LoggedUser.RolaId);
            }
            else if (_formMode == FormMode.AddToTest)
            {
                if (LoggedUser.RolaId == 1)
                    mainWindow.ShowAdminView();
                else if (LoggedUser.RolaId == 2)
                    mainWindow.ShowTeacherView();
            }
        }

        private async void AssignExistingQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AssignQuestionToTestWindow
            {
                Owner = Window.GetWindow(this)
            };

            bool? result = window.ShowDialog();

            if (result != true || window.SelectedQuestion == null)
                return;

            int questionId = window.SelectedQuestion.Id;

            bool isAlreadyAssigned = _assignedQuestions
                .Any(q => q.Id == questionId);

            if (isAlreadyAssigned)
            {
                MessageBox.Show(
                    "To pytanie jest już przypisane do tworzonego testu.",
                    "Pytanie już przypisane",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            try
            {
                AssignedQuestionDto assignedQuestion =
                    await _testService
                        .GetAssignedQuestionByIdAsync(questionId);

                _assignedQuestions.Add(assignedQuestion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie udało się przypisać pytania.\n\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddNewQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                var testDraft = CreateTestDraftFromForm();

                mainWindow.ShowAddQuestionView(_formMode, testDraft, LoggedUser.RolaId);
            }
        }

        private void MoveQuestionUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
       button.DataContext is not AssignedQuestionDto selectedQuestion)
            {
                return;
            }

            int currentIndex = _assignedQuestions.IndexOf(selectedQuestion);

            if (currentIndex <= 0)
                return;

            _assignedQuestions.Move(currentIndex, currentIndex - 1);

            UpdateQuestionsView();
        }

        private void MoveQuestionDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
       button.DataContext is not AssignedQuestionDto selectedQuestion)
            {
                return;
            }

            int currentIndex = _assignedQuestions.IndexOf(selectedQuestion);

            if (currentIndex < 0 ||
                currentIndex >= _assignedQuestions.Count - 1)
            {
                return;
            }

            _assignedQuestions.Move(currentIndex, currentIndex + 1);

            UpdateQuestionsView();
        }

        private void RemoveQuestionFromTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button ||
       button.DataContext is not AssignedQuestionDto selectedQuestion)
            {
                return;
            }

            _assignedQuestions.Remove(selectedQuestion);
        }
    }
}