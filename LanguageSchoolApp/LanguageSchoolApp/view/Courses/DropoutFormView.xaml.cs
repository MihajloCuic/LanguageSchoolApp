using LanguageSchoolApp.viewModel.Courses;
using LanguageSchoolApp.viewModel.Users;
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
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Courses
{
    /// <summary>
    /// Interaction logic for DropoutFormView.xaml
    /// </summary>
    public partial class DropoutFormView : Window
    {
        public DropoutFormView(string studentId, int courseId)
        {
            InitializeComponent();
            DataContext = new DropoutFormViewModel(studentId, courseId);
            if (DataContext is DropoutFormViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
