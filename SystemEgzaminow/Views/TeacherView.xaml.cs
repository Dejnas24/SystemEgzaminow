using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Session;

namespace SystemEgzaminow.WPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy TeacherView.xaml
    /// </summary>
    public partial class TeacherView : UserControl
    {
        public TeacherView()
        {
            InitializeComponent();
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowAddAddTestView(LoggedUser.RolaId);
            }
        }

        private void GradeScale_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowGradeScaleView(LoggedUser.RolaId);
            }
        }

        private void TestList_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.ShowListsTestsView(LoggedUser.RolaId);
            }
        }

        private void AssignTests_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowAssignTestView(LoggedUser.RolaId);
            }
        }

        private void TestResults_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowTestResultsView(LoggedUser.RolaId);
            }
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
                return;

            await mainWindow.EndCurrentSessionAsync();
            mainWindow.ShowLoginView();
        }

        private async void Close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
                await mainWindow.EndCurrentSessionAsync();
            Application.Current.Shutdown();
        }

        private void AddQuestions_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.AddQuestionView(FormMode.Add, LoggedUser.RolaId);
            }
        }

        private void EditQuestions_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ShowEditQuestionsView(LoggedUser.RolaId);
            }
        }

        private void SolveReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Raport rozwiązywania testów będzie dostępny w wersji 1.0.", "Planowana funkcjonalność", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}