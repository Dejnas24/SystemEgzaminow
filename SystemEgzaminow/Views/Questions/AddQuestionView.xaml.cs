using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.WPF.Windows.Questions;

namespace SystemEgzaminow.WPF.Views.Questions
{
    /// <summary>
    /// Logika interakcji dla klasy AddQuestionView.xaml
    /// </summary>
    public partial class AddQuestionView : UserControl
    {
        private string? _imagePath;
        private readonly FormMode _formMode;
        private readonly TestDraft _testDraft;
        private readonly int _rolaId;

        public AddQuestionView(FormMode formMode, int rolaId)
        {
            InitializeComponent();
            _formMode = formMode;
            _rolaId = rolaId;
        }

        public AddQuestionView(FormMode formMode, TestDraft testDraft, int rolaId) : this(formMode, rolaId)
        {
            _testDraft = testDraft;
        }

        public AddQuestionView(FormMode formMode, QuestionDraft draft, int rolaId) : this(formMode, rolaId)
        {
            QuestionTextBox.Text = draft.TrescPytania;
            PointsTextBox.Text = draft.LiczbaPunktow.ToString();

            _imagePath = draft.SciezkaZdjecia;
            LoadImagePreview();
        }

        public AddQuestionView(FormMode formMode, TestDraft testDraft, QuestionDraft draft, int rolaId) : this(formMode, testDraft, rolaId)
        {
            QuestionTextBox.Text = draft.TrescPytania;
            PointsTextBox.Text = draft.LiczbaPunktow.ToString();

            _imagePath = draft.SciezkaZdjecia;
            LoadImagePreview();
        }

        private void LoadImagePreview()
        {
            QuestionImage.Source = null;

            if (string.IsNullOrWhiteSpace(_imagePath) || !File.Exists(_imagePath))
            {
                ImageInfoTextBlock.Text = "Brak zdjęcia";
                ImageInfoTextBlock.Visibility = Visibility.Visible;
                return;
            }

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            QuestionImage.Source = bitmap;
            ImageInfoTextBlock.Text = Path.GetFileName(_imagePath);
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = "Wybierz zdjęcie pytania",
                    Filter = "Pliki obrazów|*.jpg;*.jpeg;*.png;*.bmp;*.webp"
                };

                if (dialog.ShowDialog() == true)
                {
                    _imagePath = dialog.FileName;

                    LoadImagePreview();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd podczas wczytywania zdjęcia:\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
                {
                    MessageBox.Show(
                        "Podaj treść pytania!",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                if (!int.TryParse(PointsTextBox.Text, out int points) || points <= 0)
                {
                    MessageBox.Show(
                        "Punkty muszą być liczbą większą od 0!",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                var window = new QuestionTypeWindow
                {
                    Owner = Window.GetWindow(this)
                };

                if (window.ShowDialog() != true ||
                    window.SelectedQuestionType == null)
                {
                    return;
                }

                switch (window.SelectedQuestionType.Value)
                {
                    case TypPytaniaEnum.JednokrotnyWybor:
                        OpenAnswerView(TypPytaniaEnum.JednokrotnyWybor);
                        break;

                    case TypPytaniaEnum.WielokrotnyWybor:
                        OpenAnswerView(TypPytaniaEnum.WielokrotnyWybor);
                        break;

                    case TypPytaniaEnum.Otwarte:
                        OpenAnswerView(TypPytaniaEnum.Otwarte);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Błąd zapisu:\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void OpenAnswerView(TypPytaniaEnum typPytania)
        {
            var draft = CreateQuestionDraft(typPytania);
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            if (_formMode == FormMode.Add)
            {
                switch (typPytania)
                {
                    case TypPytaniaEnum.JednokrotnyWybor:
                        mainWindow.ShowSingleChoiceAnswerView(draft, FormMode.Add);
                        break;

                    case TypPytaniaEnum.WielokrotnyWybor:
                        mainWindow.ShowMultipleChoiceAnswerView(draft, FormMode.Add);
                        break;

                    case TypPytaniaEnum.Otwarte:
                        mainWindow.ShowOpenAnswerView(draft, FormMode.Add);
                        break;
                }
            }
            else if (_formMode == FormMode.AddToTest || _formMode == FormMode.EditToTest)
            {
                switch (typPytania)
                {
                    case TypPytaniaEnum.JednokrotnyWybor:
                        mainWindow.ShowSingleChoiceAnswerView(draft, _testDraft, _formMode);
                        break;

                    case TypPytaniaEnum.WielokrotnyWybor:
                        mainWindow.ShowMultipleChoiceAnswerView(draft, _testDraft, _formMode);
                        break;

                    case TypPytaniaEnum.Otwarte:
                        mainWindow.ShowOpenAnswerView(draft, _testDraft, _formMode);
                        break;
                }
            }
        }

        private void SingleChoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var draft = CreateQuestionDraft(TypPytaniaEnum.JednokrotnyWybor);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    if (_formMode == FormMode.Add)
                    {
                        mainWindow.ShowSingleChoiceAnswerView(draft, FormMode.Add);
                    }
                    else if (_formMode == FormMode.AddToTest)
                    {
                        mainWindow.ShowSingleChoiceAnswerView(draft, _testDraft, FormMode.AddToTest);
                    }
                    else if (_formMode == FormMode.EditToTest)
                    {
                        mainWindow.ShowSingleChoiceAnswerView(draft, _testDraft, FormMode.EditToTest);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MultiChoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var draft = CreateQuestionDraft(TypPytaniaEnum.WielokrotnyWybor);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    if (_formMode == FormMode.Add)
                    {
                        mainWindow.ShowMultipleChoiceAnswerView(draft, FormMode.Add);
                    }
                    else if (_formMode == FormMode.AddToTest)
                    {
                        mainWindow.ShowMultipleChoiceAnswerView(draft, _testDraft, FormMode.AddToTest);
                    }
                    else if (_formMode == FormMode.EditToTest)
                    {
                        mainWindow.ShowMultipleChoiceAnswerView(draft, _testDraft, FormMode.EditToTest);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var draft = CreateQuestionDraft(TypPytaniaEnum.Otwarte);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    if (_formMode == FormMode.Add)
                    {
                        mainWindow.ShowOpenAnswerView(draft, FormMode.Add);
                    }
                    else if (_formMode == FormMode.AddToTest)
                    {
                        mainWindow.ShowOpenAnswerView(draft, _testDraft, FormMode.AddToTest);
                    }
                    else if (_formMode == FormMode.EditToTest)
                    {
                        mainWindow.ShowOpenAnswerView(draft, _testDraft, FormMode.EditToTest);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
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
            else if (_formMode == FormMode.AddToTest)
            {
                mainWindow.ShowAddAddTestView(_testDraft, FormMode.AddToTest, _rolaId);
            }
            else if (_formMode == FormMode.EditToTest)
            {
                mainWindow.ShowAddAddTestView(_testDraft, FormMode.EditToTest, _rolaId);
            }
        }

        private QuestionDraft CreateQuestionDraft(TypPytaniaEnum typPytania)
        {
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
                throw new Exception("Wpisz treść pytania.");

            if (!int.TryParse(PointsTextBox.Text, out int punkty))
                throw new Exception("Liczba punktów musi być liczbą.");

            if (punkty <= 0)
                throw new Exception("Liczba punktów musi być większa od 0.");

            return new QuestionDraft
            {
                TrescPytania = QuestionTextBox.Text.Trim(),
                LiczbaPunktow = punkty,
                TypPytania = typPytania,
                IdAutora = LoggedUser.Id,
                SciezkaZdjecia = _imagePath
            };
        }
    }
}