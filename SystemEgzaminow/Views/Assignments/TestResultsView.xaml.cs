using System.Windows;
using System.Windows.Controls;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;

namespace SystemEgzaminow.WPF.Views.Assignments
{
    /// <summary>
    /// Interaction logic for TestResultsView.xaml
    /// </summary>
    public partial class TestResultsView : UserControl
    {
        private readonly int _rolaId;
        public TestResultsView(int rolaId)
        {
            InitializeComponent();
            _rolaId = rolaId;
        }
        private async void TestResult_Loaded(object sender, RoutedEventArgs e)
        {
            TestTypeFilterBox.IsEnabled = false;
            TestNameFilterBox.IsEnabled = false;
            await LoadClassesAsync();

        }
        private async Task LoadClassesAsync()
        {
            using var cotext= DbContextHelper.CreateDbContext();

            var sevice= new AssignmentService(cotext);
            ClassFilterBox.ItemsSource = await sevice.GetClassesAsync();

           
        }
      
            private async Task LoadTestsByClassAndTypeAsync(int classId, int testTypeId)
        {
            using var context = DbContextHelper.CreateDbContext();
            var service = new AssignmentService(context);
            TestNameFilterBox.ItemsSource = await service.GetByClassAndTypeAsync(classId, testTypeId);

        }
        private async Task LoadTypesClassAsync(int classId)
        {
            using var context = DbContextHelper.CreateDbContext();
            var service = new TestTypeService(context);
            TestTypeFilterBox.ItemsSource = await service.GetByClassAsync(classId);
        }

        private async void ClassFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestTypeFilterBox.SelectedItem = null;
            TestTypeFilterBox.ItemsSource = null;

            TestNameFilterBox.SelectedItem = null;
            TestNameFilterBox.ItemsSource = null;
            TestNameFilterBox.IsEnabled = false;


            if (ClassFilterBox.SelectedItem is AssignmentRecipientDto selectedClass)
            {
                await LoadTypesClassAsync(selectedClass.Id);
                TestTypeFilterBox.IsEnabled = true;
            }
        }

        private async void TestTypeFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TestNameFilterBox.SelectedItem = null;
            TestNameFilterBox.ItemsSource = null;
            TestNameFilterBox.IsEnabled = false;

            if (ClassFilterBox.SelectedItem is AssignmentRecipientDto selectedClass && TestTypeFilterBox.SelectedItem is TestTypeDto selectedType)
            {
                await LoadTestsByClassAndTypeAsync(selectedClass.Id, selectedType.Id);
                TestNameFilterBox.IsEnabled = true;
            }
        }

        private void TestNameFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EditClassAssignment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PublishTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportTestExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportTestPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseTestStudent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StudentFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditAnswers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PublishResults_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResultStudents_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ArchiveAnswers_Click(object sender, RoutedEventArgs e)
        {

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

        private void DateFromPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DateToPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       
    }
}
