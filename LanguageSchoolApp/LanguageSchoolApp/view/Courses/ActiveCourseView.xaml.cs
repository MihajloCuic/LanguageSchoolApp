using LanguageSchoolApp.viewModel.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Courses
{
    /// <summary>
    /// Interaction logic for ActiveCourseView.xaml
    /// </summary>
    public partial class ActiveCourseView : UserControl
    {
        public ActiveCourseView()
        {
            InitializeComponent();
            Loaded += ActiveCourseView_Loaded;
        }

        private void ActiveCourseView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ActiveCourseViewModel activeCourseViewModel)
            {
                activeCourseViewModel.DisableTeacherGradingAction = new Action(DisableTeacherGrading);
            }
        }

        private void DisableTeacherGrading()
        {
            TextBox gradeTeacherTextBox = (TextBox)FindName("TeachersGradeTextBox");
            Button gradeTeacherButton = (Button)FindName("GradeTeacherButton");

            gradeTeacherButton.IsEnabled = false;
            gradeTeacherTextBox.IsEnabled = false;
        }
    }
}
