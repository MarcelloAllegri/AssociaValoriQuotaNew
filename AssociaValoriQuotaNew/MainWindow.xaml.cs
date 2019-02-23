using AssociaValoriQuotaNew.Resources.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssociaValoriQuotaNew
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FileInformation> m_fileInformation;
        Dictionary<string, char> ListOfSeparator;
        private ConcurrentBag<string> m_FileContentList;
        private ConcurrentBag<string> m_OutputFileContentList;
        private char m_ImputFileDelimiter;
        private char m_OutputFileDelimiter;
        private int m_ItemPorRow = -1;

        public List<FileInformation> fileInformation
        {
            get { return m_fileInformation; }
            set { m_fileInformation = value; }
        }

        public char InputFileDelimiter
        {
            set { m_ImputFileDelimiter = value; }
        }

        public char OutputFileDelimiter
        {
            set { m_ImputFileDelimiter = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListOfSeparator = new Dictionary<string, char>();

            ListOfSeparator.Add(".", '.');
            ListOfSeparator.Add(",", ',');
            ListOfSeparator.Add(";", ';');
            ListOfSeparator.Add("Tab", Convert.ToChar(11));
            ListOfSeparator.Add("Space", ' ');
            ListOfSeparator.Add("Other", 'o');

            OutputFileSeparatorChooseComboBox.ItemsSource = InputFileSeparatorChooseComboBox.ItemsSource = ListOfSeparator;
        }

        private void AddFileToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }

        private void SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Csv file (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            if (openFileDialog.ShowDialog() == true)
            {
                fileInformation = new List<FileInformation>();
                fileInformation.Add(new FileInformation(Path.GetFileName(openFileDialog.FileName), Path.GetFullPath(openFileDialog.FileName)));
                this.FileListView.ItemsSource = fileInformation;
            }
        }

        private void SaveResult()
        {
            bool retry = true;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Salva File Risultato";
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv";

            while (retry)
            {
                if (saveFileDialog1.ShowDialog() == true)
                {
                    if (File.Exists(saveFileDialog1.FileName) == false)
                    {

                        File.AppendAllLines(saveFileDialog1.FileName, m_OutputFileContentList.ToList());
                        retry = false;

                        MessageBox.Show("Procedura di sotituzione completata.");
                    }
                    else
                    {
                        MessageBox.Show("Il file e' correntemente in uso, si prega di chiuderlo.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                        retry = true;
                    }
                }
                else
                {
                    retry = true;
                }
            }
        }

        private int GetFile()
        {
            try
            {
                m_FileContentList = new ConcurrentBag<string>(File.ReadAllLines(m_fileInformation[0].Path));
            }
            catch (Exception)
            {
                MessageBox.Show("Error during importing file. Check it!");
                return -1;
            }

            return 0;
        }

        private void InputFileSeparatorChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = InputFileSeparatorChooseComboBox.SelectedValue;

            if (selectedValue != null)
                switch (((KeyValuePair<string, char>)selectedValue).Key)
                {
                    case ".":
                    case ",":
                    case ";":
                    case "Tab":
                    case "Space": this.m_ImputFileDelimiter = ListOfSeparator[((KeyValuePair<string, char>)selectedValue).Key]; break;
                    case "Other": InsertCustomDelimiter(true); break;
                    default:
                        break;
                }
        }

        private void OutputFileSeparatorChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = OutputFileSeparatorChooseComboBox.SelectedValue;

            if (selectedValue != null)
                switch (((KeyValuePair<string, char>)selectedValue).Key)
                {
                    case ".":
                    case ",":
                    case ";":
                    case "Tab":
                    case "Space": this.m_OutputFileDelimiter = ListOfSeparator[((KeyValuePair<string, char>)selectedValue).Key]; break;
                    case "Other": InsertCustomDelimiter(false); break;
                    default:
                        break;
                }
        }

        private void InsertCustomDelimiter(bool isInputDelimiter)
        {
            NextButton.IsEnabled = false;
            customCharacterUserControl.Visibility = Visibility.Visible;
            customCharacterUserControl.IsSelected = true;
            insertDelimitatorUserControl.IsInputDelimiter = isInputDelimiter;
            MainTabItem.Visibility = Visibility.Collapsed;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string text = insertDelimitatorUserControl.Save();


            if (text.Count() == 1)
            {
                switch (insertDelimitatorUserControl.IsInputDelimiter)
                {
                    case true: m_ImputFileDelimiter = text[0]; break;
                    case false: m_OutputFileDelimiter = text[0]; break;
                }

                MessageBox.Show("Saved!");
            }
            else
            {
                if (text.Count() == 0)
                    MessageBox.Show("Non è stato inserito alcun carattere!");
                else
                    MessageBox.Show("Hai inserito più di 1 carattere!");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainTabItem.Visibility = Visibility.Visible;
            MainTabItem.IsSelected = true;
            string text = insertDelimitatorUserControl.Save();
            if (!(text.Count() == 1))
                switch (insertDelimitatorUserControl.IsInputDelimiter)
                {
                    case true: InputFileSeparatorChooseComboBox.SelectedItem = null; break;
                    case false: OutputFileSeparatorChooseComboBox.SelectedItem = null; break;
                }
            customCharacterUserControl.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = true;
        }
    }
}
