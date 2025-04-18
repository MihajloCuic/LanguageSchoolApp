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
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Courses
{
    /// <summary>
    /// Interaction logic for HandleDropoutRequestView.xaml
    /// </summary>
    public partial class HandleDropoutRequestView : Window
    {
        public HandleDropoutRequestView()
        {
            InitializeComponent();
        }

        public void SetAction()
        {
            if (DataContext is HandleDropoutRequestViewModel viewModel)
            {
                viewModel.CloseAction = new Action(this.Close);
            }
        }
    }
}
