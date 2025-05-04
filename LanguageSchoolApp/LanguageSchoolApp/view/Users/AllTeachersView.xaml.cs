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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanguageSchoolApp.view.Users
{
    /// <summary>
    /// Interaction logic for AllTeachersView.xaml
    /// </summary>
    public partial class AllTeachersView : UserControl
    {
        public AllTeachersView()
        {
            InitializeComponent();
        }

        private async void FlipButtonClick(object sender, RoutedEventArgs e)
        { 
            Button btn = sender as Button;
            if (btn != null) 
            { 
                Grid cardRoot = FindParentWithTag(btn, "FlipCardRoot") as Grid;
                if (cardRoot != null)
                {
                    Border cardBackground = VisualTreeHelper.GetParent(cardRoot) as Border;

                    if (cardBackground != null) 
                    {
                        Grid frontSide = FindChild<Grid>(cardRoot, "FrontSide");
                        Grid backSide = FindChild<Grid>(cardRoot, "BackSide");
                        if (frontSide != null && backSide != null) 
                        {
                            FlipCard(cardBackground, frontSide, backSide);

                            await Task.Delay(3000);
                            FlipCard(cardBackground, backSide, frontSide);
                        }
                    }
                }
            }
        }

        private void FlipCard(Border cardBackground, Grid sideToHide, Grid sideToShow)
        {
            Storyboard flipStoryboard = (Storyboard)FindResource("FlipStoryboard");
            Storyboard flipBackStoryboard = (Storyboard)FindResource("UnflipStoryboard");

            Storyboard firstHalfFlip = flipStoryboard.Clone();

            foreach (Timeline timeline in firstHalfFlip.Children)
            { 
                Storyboard.SetTarget(timeline, cardBackground);
            }

            firstHalfFlip.Completed += (s, e) =>
            {
                sideToHide.Visibility = Visibility.Collapsed;
                sideToShow.Visibility = Visibility.Visible;

                Storyboard secondHalfFlip = flipBackStoryboard.Clone();
                foreach (Timeline timeline in secondHalfFlip.Children)
                {
                    Storyboard.SetTarget(timeline, cardBackground);
                }

                secondHalfFlip.Begin();
            };

            firstHalfFlip.Begin();
        }

        private DependencyObject FindParentWithTag(DependencyObject child, string tag)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is FrameworkElement element && element.Tag != null && element.Tag.ToString() == tag)
                {
                    return parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject 
        {
            if (parent == null) return null;
            T foundChild = null;
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++) 
            { 
                var child = VisualTreeHelper.GetChild(parent, i);

                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else 
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }
    }
}
