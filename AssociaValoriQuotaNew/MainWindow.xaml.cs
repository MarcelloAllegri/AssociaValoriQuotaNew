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

namespace AssociaValoriQuotaNew
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddFileToolbarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InputFileSeparatorChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OutputFileSeparatorChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            bool bypass = false;

            if(MainTabItem.Visibility == Visibility.Visible)
            {
                InputFileParameterOrder.Visibility = Visibility.Visible;
                MainTabItem.Visibility = Visibility.Collapsed;
                OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                InputFileParameterOrder.IsSelected = true;
                bypass = true;
            }

            if(InputFileParameterOrder.Visibility == Visibility.Visible && bypass == false)
            {
                InputFileParameterOrder.Visibility = Visibility.Collapsed;
                MainTabItem.Visibility = Visibility.Collapsed;
                OutputFileParameterOrder.Visibility = Visibility.Visible;
                OutputFileParameterOrder.IsSelected = true;
                bypass = true;
            }

            if(OutputFileParameterOrder.Visibility == Visibility.Visible && bypass == false)
            {
                InputFileParameterOrder.Visibility = Visibility.Collapsed;
                MainTabItem.Visibility = Visibility.Visible;
                OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                MainTabItem.IsSelected = true;
            }

            MainTabControl.UpdateLayout();
        }
    }
}
