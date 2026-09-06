using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Core.Session;

namespace SystemEgzaminow.WPF.Views.Questions
{
    /// <summary>
    /// Logika interakcji dla klasy EditQuestionView.xaml
    /// </summary>
    public partial class EditQuestionView : UserControl
    {
        private readonly int _questionId;
        private string? _imagePath;
        private Pytanie? _question;

        public EditQuestionView(int questionId)
        {
            InitializeComponent();
            _questionId = questionId;

            LoadQuestion();
        }

        private void LoadQuestion()
        {
            using var context = DbContextHelper.CreateDbContext();

            _question = context.Pytania
    .FirstOrDefault(p => p.Id == _questionId);


            if (_question == null)
            {
                MessageBox.Show(
                    "Nie znaleziono pytania.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            QuestionTextBox.Text = _question.TrescPytania;
            PointsTextBox.Text = _question.LiczbaPunktow.ToString();

            QuestionTypeTextBlock.Text = _question.TypPytania switch
            {
                TypPytaniaEnum.JednokrotnyWybor => "Jednokrotny wybór",
                TypPytaniaEnum.WielokrotnyWybor => "Wielokrotny wybór",
                TypPytaniaEnum.Otwarte => "Otwarte",
                _ => "Nieznany typ"
            };

            _imagePath = _question.SciezkaZdjecia;
            LoadImagePreview();
        }

        private void LoadImagePreview()
        {
            QuestionImage.Source = null;

            if (string.IsNullOrWhiteSpace(_imagePath) || !File.Exists(_imagePath))
            {
                ImageInfoTextBlock.Text = "Brak zdjęcia";
                ImageInfoTextBlock.Visibility = Visibility.Visible;
                RemoveImageButton.IsEnabled = false;
                return;
            }

            var bitmap = new System.Windows.Media.Imaging.BitmapImage();

            bitmap.BeginInit();
            bitmap.CacheOption =
                System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(_imagePath, UriKind.Absolute);
            bitmap.EndInit();

            QuestionImage.Source = bitmap;
            ImageInfoTextBlock.Visibility = Visibility.Collapsed;
            RemoveImageButton.IsEnabled = true;
        }

        private void EditAnswer_Click(object sender, RoutedEventArgs e)
        {
            string trescPytania = QuestionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(trescPytania))
            {
                MessageBox.Show(
                    "Treść pytania nie może być pusta.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(PointsTextBox.Text, out int liczbaPunktow) ||
                liczbaPunktow <= 0)
            {
                MessageBox.Show(
                    "Liczba punktów musi być liczbą większą od 0.",
                    "Błąd",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var draft = new QuestionDraft
            {
                TrescPytania = trescPytania,
                LiczbaPunktow = liczbaPunktow,
                TypPytania = _question!.TypPytania,
                SciezkaZdjecia = _imagePath
            };

            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            switch (_question.TypPytania)
            {
                case TypPytaniaEnum.JednokrotnyWybor:
                    mainWindow.ShowSingleChoiceAnswerView(
                        draft,
                        FormMode.Edit,
                        _questionId);
                    break;

                case TypPytaniaEnum.WielokrotnyWybor:
                    mainWindow.ShowMultipleChoiceAnswerView(
                        draft,
                        FormMode.Edit,
                        _questionId);
                    break;

                case TypPytaniaEnum.Otwarte:
                    mainWindow.ShowOpenAnswerView(
                        draft,
                        FormMode.Edit,
                        _questionId);
                    break;
            }
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
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

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_imagePath))
                return;

            var result = MessageBox.Show(
                "Czy na pewno chcesz usunąć zdjęcie z pytania?",
                "Usuń zdjęcie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            _imagePath = null;
            LoadImagePreview();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            mainWindow.ShowEditQuestionsView(LoggedUser.RolaId);

            //if (LoggedUser.RolaId == 1)
            //    mainWindow.ShowAdminView();
            //else if (LoggedUser.RolaId == 2)
            //    mainWindow.ShowTeacherView();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string trescPytania = QuestionTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(trescPytania))
                {
                    MessageBox.Show(
                        "Treść pytania nie może być pusta.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                if (!int.TryParse(PointsTextBox.Text, out int liczbaPunktow))
                {
                    MessageBox.Show(
                        "Liczba punktów musi być liczbą.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                if (liczbaPunktow <= 0)
                {
                    MessageBox.Show(
                        "Liczba punktów musi być większa od 0.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                using var context = DbContextHelper.CreateDbContext();

                var question = context.Pytania
                    .FirstOrDefault(p => p.Id == _questionId);

                if (question == null)
                {
                    MessageBox.Show(
                        "Nie znaleziono pytania.",
                        "Błąd",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }

                question.TrescPytania = trescPytania;
                question.LiczbaPunktow = liczbaPunktow;
                question.SciezkaZdjecia = _imagePath;

                context.SaveChanges();

                MessageBox.Show(
                    "Zmiany pytania zapisano poprawnie.",
                    "Sukces",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null)
                {
                    mainWindow.ShowEditQuestionsView(LoggedUser.RolaId);
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
    }
}