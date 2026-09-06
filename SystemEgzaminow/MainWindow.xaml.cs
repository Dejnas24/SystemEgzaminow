using System.ComponentModel;
using System.Windows;
using SystemEgzaminow.Core.Drafts;
using SystemEgzaminow.Core.DTO;
using SystemEgzaminow.Core.Enums;
using SystemEgzaminow.Core.Models;
using SystemEgzaminow.Core.Session;
using SystemEgzaminow.Data.Services;
using SystemEgzaminow.WPF;
using SystemEgzaminow.WPF.Views;
using SystemEgzaminow.WPF.Views.Assignments;
using SystemEgzaminow.WPF.Views.Logs;
using SystemEgzaminow.WPF.Views.Questions;
using SystemEgzaminow.WPF.Views.Tests;
using SystemEgzaminow.WPF.Views.Users;

namespace SystemEgzaminow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowLoginView();
            // MainContent.Content = new LoginView();
        }

        public void ShowLoginView()
        {
            MainContent.Content = new LoginView();
        }

        public void ShowAdminView(Uzytkownik user)
        {
            MainContent.Content = new AdminView();
        }

        public void ShowAdminView()
        {
            MainContent.Content = new AdminView();
        }

        public void ShowTeacherView(Uzytkownik user)
        {
            MainContent.Content = new TeacherView();
        }

        public void ShowTeacherView()
        {
            MainContent.Content = new TeacherView();
        }

        public void ShowStudentView(Uzytkownik user)
        {
            MainContent.Content = new StudentView();
        }

        public void ShowStudentView()
        {
            MainContent.Content = new StudentView();
        }

        public void ShowAddUserView()
        {
            MainContent.Content = new AddUserView();
        }

        public void ShowUsersListView()
        {
            MainContent.Content = new UsersListView();
        }

        public void ShowEditUserView()
        {
            MainContent.Content = new EditUserView();
        }

        public void ShowEditUserFormView(int userId)
        {
            MainContent.Content = new EditUserFormView(userId);
        }

        internal void AddQuestionView(FormMode formMode, int roleId)
        {
            MainContent.Content = new AddQuestionView(formMode, roleId);
        }

        public void ShowAddQuestionView(FormMode formMode, int roleId)
        {
            MainContent.Content = new AddQuestionView(formMode, roleId);
        }

        public void ShowSingleChoiceAnswerView(QuestionDraft draft, FormMode formMode,
    int? questionId = null)
        {
            MainContent.Content = new SingleChoiceAnswerView(draft, formMode, questionId);
        }

        public void ShowMultipleChoiceAnswerView(QuestionDraft draft, FormMode formMode, int? questionId = null)
        {
            MainContent.Content = new MultipleChoiceAnswerView(draft, formMode, questionId);
        }

        public void ShowOpenAnswerView(QuestionDraft draft, FormMode formMode, int? questionId = null)
        {
            MainContent.Content = new OpenAnswerView(draft, formMode, questionId);
        }

        public void ShowAddQuestionView(FormMode formMode, TestDraft testDraft, int roleId)
        {
            MainContent.Content = new AddQuestionView(formMode, testDraft, roleId);
        }

        public void ShowAddQuestionView(FormMode formMode, QuestionDraft draft, int rolaId)
        {
            MainContent.Content = new AddQuestionView(formMode, draft, rolaId);
        }

        public void ShowAddQuestionView(FormMode formMode, TestDraft testDraft, QuestionDraft draft, int rolaId)
        {
            MainContent.Content = new AddQuestionView(formMode, testDraft, draft, rolaId);
        }

        public void ShowSingleChoiceAnswerView(QuestionDraft draft, TestDraft testDraft, FormMode formMode, int? questionId = null)
        {
            MainContent.Content = new SingleChoiceAnswerView(draft, formMode, questionId, testDraft);
        }

        public void ShowMultipleChoiceAnswerView(QuestionDraft draft, TestDraft testDraft, FormMode formMode, int? questionId = null)
        {
            MainContent.Content = new MultipleChoiceAnswerView(draft, formMode, questionId, testDraft);
        }

        public void ShowOpenAnswerView(QuestionDraft draft, TestDraft testDraft, FormMode formMode, int? questionId = null)
        {
            MainContent.Content = new OpenAnswerView(draft, formMode, questionId, testDraft);
        }

        public void ShowEditQuestionsView(int roleId)
        {
            MainContent.Content = new EditQuestionsView(roleId);
        }

        public void ShowEditQuestionView(int questionId)
        {
            MainContent.Content = new EditQuestionView(questionId);
        }

        public void ShowAddAddTestView(int roleId)
        {
            MainContent.Content = new AddTestView(roleId);
        }

        public void ShowAddAddTestView(TestDraft testDraft, FormMode formMode, int roleId)
        {
            MainContent.Content = new AddTestView(testDraft, formMode, roleId);
        }

        public void ShowAddAddTestView(TestDraft testDraft, FormMode formMode, int? newQuestionId, int roleId)
        {
            MainContent.Content = new AddTestView(testDraft, formMode, newQuestionId, roleId); ;
        }

        public void ShowListsTestsView(int roleId)
        {
            MainContent.Content = new ListsTestsView(roleId);
        }

        public void ShowTestSessionView(TestSessionDto testSessionDto)
        {
            MainContent.Content = new TestSessionView(testSessionDto);
        }

        public void ShowLoginLogsView()
        {
            MainContent.Content = new LoginLogsView();
        }

        public void ShowAssignTestView(int roleId)
        {
            MainContent.Content = new AssignTestView(roleId);
        }

        public void ShowGradeScaleView(int roleId)
        {
            MainContent.Content = new GradeScaleView(roleId);
        }

        public async Task EndCurrentSessionAsync()
        {
            if (LoggedUser.LoginLogId.HasValue)
            {
                using var context = DbContextHelper.CreateDbContext();

                var service = new LoginLogService(context);

                await service.EndLoginSessionAsync(
                    LoggedUser.LoginLogId.Value);
            }

            LoggedUser.LoginLogId = null;
            LoggedUser.Id = 0;
            LoggedUser.Login = string.Empty;
            LoggedUser.Imie = string.Empty;
            LoggedUser.Nazwisko = string.Empty;
            LoggedUser.RolaId = 0;
        }

        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (LoggedUser.LoginLogId.HasValue)
            {
                try
                {
                    using var context = DbContextHelper.CreateDbContext();

                    var service = new LoginLogService(context);

                    await service.EndLoginSessionAsync(
                        LoggedUser.LoginLogId.Value);

                    LoggedUser.LoginLogId = null;
                }
                catch
                {
                    // Przy zamykaniu aplikacji nie blokujemy zamknięcia
                    // tylko dlatego, że nie udało się zapisać logu.
                }
            }

            //public void ShowAssignQuestionToTestWindow()
            //{
            //    MainContent.Content = new AssignQuestionToTestWindow();
            //}
        }
    }
}