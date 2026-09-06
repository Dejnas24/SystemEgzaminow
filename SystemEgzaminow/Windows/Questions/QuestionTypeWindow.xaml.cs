using System.Windows;
using SystemEgzaminow.Core.Enums;

namespace SystemEgzaminow.WPF.Windows.Questions
{
    /// <summary>
    /// Interaction logic for QuestionTypeWindow.xaml
    /// </summary>
    public partial class QuestionTypeWindow : Window
    {
        public TypPytaniaEnum? SelectedQuestionType { get; private set; }

        public QuestionTypeWindow()
        {
            InitializeComponent();
            LoadQuestionTypes();
        }

        private void LoadQuestionTypes()
        {
            QuestionTypeComboBox.Items.Add("Jednokrotny wybór");
            QuestionTypeComboBox.Items.Add("Wielokrotny wybór");
            QuestionTypeComboBox.Items.Add("Otwarte");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (QuestionTypeComboBox.SelectedItem is not string selectedItem)
            {
                MessageBox.Show("Wybierz typ pytania.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            switch (selectedItem)
            {
                case "Jednokrotny wybór":
                    SelectedQuestionType = TypPytaniaEnum.JednokrotnyWybor;
                    break;

                case "Wielokrotny wybór":
                    SelectedQuestionType = TypPytaniaEnum.WielokrotnyWybor;
                    break;

                case "Otwarte":
                    SelectedQuestionType = TypPytaniaEnum.Otwarte;
                    break;

                default:
                    return;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}