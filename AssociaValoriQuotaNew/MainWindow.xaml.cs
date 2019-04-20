using AssociaValoriQuotaNew.Resources.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AssociaValoriQuotaNew
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private ObservableCollection<FileInformation> m_fileInformation;
        Dictionary<string, char> ListOfSeparator;       
        private char m_InputFileDelimiter;
        private char m_OutputFileDelimiter;
        private double? m_DifferenceQuoteValue;
        private bool m_CustomDiffQuoteValue;
        private bool m_SelectValueFromFilename = true;

        public bool SelectValueFromFilename
        {
            set { m_SelectValueFromFilename = value; }
            get { return m_SelectValueFromFilename; }
        }

        public bool CustomDiffQuoteValue
        {
            set { m_CustomDiffQuoteValue = value; }
            get { return m_CustomDiffQuoteValue; }
        }

        public ObservableCollection<FileInformation> fileInformation
        {
            get { return m_fileInformation; }
            set { m_fileInformation = value; OnPropertyChanged(nameof(fileInformation)); }
        }

        public char InputFileDelimiter
        {
            set { m_InputFileDelimiter = value; }
        }

        public char OutputFileDelimiter
        {
            set { m_InputFileDelimiter = value; }
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
            ListOfSeparator.Add("Tab", Convert.ToChar(9));
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
            openFileDialog.Multiselect = true;
            if (fileInformation == null) fileInformation = new ObservableCollection<FileInformation>();

            if (openFileDialog.ShowDialog() == true)
            {                
                foreach (var item in openFileDialog.FileNames)
                {
                    fileInformation.Add(new FileInformation(Path.GetFileName(item), Path.GetFullPath(item)));
                    OnPropertyChanged(nameof(fileInformation));
                }
            }
        }

        //private void SaveResult()
        //{
        //    bool retry = true;
        //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //    saveFileDialog1.Title = "Salva File Risultato";
        //    saveFileDialog1.Filter = "csv files (*.csv)|*.csv";

        //    while (retry)
        //    {
        //        if (saveFileDialog1.ShowDialog() == true)
        //        {
        //            if (File.Exists(saveFileDialog1.FileName) == false)
        //            {

        //                File.AppendAllLines(saveFileDialog1.FileName, m_OutputFileContentList.ToList());
        //                retry = false;

        //                MessageBox.Show("Procedura di sotituzione completata.");
        //            }
        //            else
        //            {
        //                MessageBox.Show("Il file e' correntemente in uso, si prega di chiuderlo.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        //                retry = true;
        //            }
        //        }
        //        else
        //        {
        //            retry = true;
        //        }
        //    }
        //}        

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
                    case "Space": this.m_InputFileDelimiter = ListOfSeparator[((KeyValuePair<string, char>)selectedValue).Key]; break;
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
                if (CheckMainTabFields())
                {
                    InputFileParameterOrder.Visibility = Visibility.Visible;
                    MainTabItem.Visibility = Visibility.Collapsed;
                    OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                    InputFileParameterOrder.IsSelected = true;
                    bypass = true;

                    RowItemCount();
                    BackButton.Visibility = Visibility.Visible;
                }
            }

            if(InputFileParameterOrder.Visibility == Visibility.Visible && bypass == false)
            {
                if (inputFileParametersUserControl.CheckFields())
                {
                    InputFileParameterOrder.Visibility = Visibility.Collapsed;
                    MainTabItem.Visibility = Visibility.Collapsed;
                    OutputFileParameterOrder.Visibility = Visibility.Visible;
                    OutputFileParameterOrder.IsSelected = true;
                    bypass = true;
                }
                else
                {
                    MessageBox.Show("Tutti i valori devono essere diversi!");
                }
            }

            if (OutputFileParameterOrder.Visibility == Visibility.Visible && bypass == false)
            {
                if (outputColumnOrderUserControl.CheckList() == true)
                {
                    InputFileParameterOrder.Visibility = Visibility.Collapsed;
                    MainTabItem.Visibility = Visibility.Visible;
                    OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                    MainTabItem.IsSelected = true;
                    BackButton.Visibility = Visibility.Collapsed;
                    MainTabItem.IsEnabled = true;
                    MainTabControl.UpdateLayout();                    

                    using (new WpfWaitCursor())
                    {
                        foreach (var item in fileInformation)
                        {
                            double valore_diff = 0;

                            if (SelectValueFromFilename)
                                valore_diff = getDiffValueFromFilename(item.FileName);
                            else
                                valore_diff = m_DifferenceQuoteValue.Value;

                            RunCalculation(item.Path, valore_diff);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("La lista di destra è vuota!");
                }


            }

            MainTabControl.UpdateLayout();
        }

        private double getDiffValueFromFilename(string filename)
        {
            string app = filename.Substring(filename.Count()- 9,5);
            app = app.Replace('_', '.');

            return double.Parse(app);
        }

        private void RowItemCount()
        {
            string firstItem = File.ReadLines(m_fileInformation[0].Path).First();
            int ColumnCnt = firstItem.Split(m_InputFileDelimiter).Count();
            inputFileParametersUserControl.ItemFoundedReadOnlyTextBox.Text = ColumnCnt.ToString();
        }

        private bool CheckMainTabFields()
        {
            string itemsNotCompiled = string.Empty;

            if (m_fileInformation == null || string.IsNullOrEmpty(this.m_fileInformation[0].Path))
                itemsNotCompiled = string.Concat(itemsNotCompiled, "File not selected!");

            if (InputFileSeparatorChooseComboBox.SelectedItem == null)
                itemsNotCompiled = string.Concat(itemsNotCompiled, "\nInput file character separator not selected!");

            if (OutputFileSeparatorChooseComboBox.SelectedItem == null)
                itemsNotCompiled = string.Concat(itemsNotCompiled, "\nOutput file character separator not selected!");


            m_DifferenceQuoteValue = DifferenceQuoteDoubleUpDown.Value;
           
            //if(m_DifferenceQuoteValue == null)
            //    itemsNotCompiled = string.Concat(itemsNotCompiled, "\nDifference value not Valid!");

            if (!string.IsNullOrEmpty(itemsNotCompiled))
            {
                MessageBox.Show("Errors:\n" + itemsNotCompiled);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string text = insertDelimitatorUserControl.Save();


            if (text.Count() == 1)
            {
                switch (insertDelimitatorUserControl.IsInputDelimiter)
                {
                    case true: m_InputFileDelimiter = text[0]; break;
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

        private void RunCalculation(string path,double diffValue)
        {
            Dictionary<char, bool> ViewDictionary = outputColumnOrderUserControl.ReturnVisibilityOfColumn();
            

            StreamReader file = new StreamReader(path);
            if (file != null)
            {
                string line = file.ReadLine();
                if (!line.Contains(m_InputFileDelimiter))
                    MessageBox.Show("Input Delimiter not correct!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    string filePath = Path.GetFullPath(path);
                    filePath = filePath.Remove(filePath.IndexOf('.'), 4);
                    using (System.IO.StreamWriter fileout = new System.IO.StreamWriter(filePath + "_result.csv"))
                    {
                        string[] Columns = line.Split(m_InputFileDelimiter);

                        Campi campo = new Campi(Convert.ToDouble(Columns[inputFileParametersUserControl.EstPosition - 1]),
                            Convert.ToDouble(Columns[inputFileParametersUserControl.NorthPosition - 1]),
                            Convert.ToDouble(Columns[inputFileParametersUserControl.QuotePosition - 1]),
                            diffValue);

                        string app = campo.ToString(m_OutputFileDelimiter, ViewDictionary);
                        fileout.WriteLine(app);


                        while ((line = file.ReadLine()) != null)
                        {
                            Columns = line.Split(m_InputFileDelimiter);

                            campo = new Campi(Convert.ToDouble(Columns[inputFileParametersUserControl.EstPosition - 1]),
                                Convert.ToDouble(Columns[inputFileParametersUserControl.NorthPosition - 1]),
                                Convert.ToDouble(Columns[inputFileParametersUserControl.QuotePosition - 1]),
                                diffValue);

                            app = campo.ToString(m_OutputFileDelimiter, ViewDictionary);
                            fileout.WriteLine(app);
                        }
                        file.Close();
                        fileout.Close();
                    }
                }                
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.FileListView.SelectedItem != null)
            {
                fileInformation.Remove(this.FileListView.SelectedItem as FileInformation);
                OnPropertyChanged(nameof(fileInformation));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(InputFileParameterOrder.Visibility == Visibility.Visible)
            {
                InputFileParameterOrder.Visibility = Visibility.Collapsed;
                MainTabItem.Visibility = Visibility.Visible;
                OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                MainTabItem.IsSelected = true;
                BackButton.Visibility = Visibility.Collapsed;
            }

            if (OutputFileParameterOrder.Visibility == Visibility.Visible)
            {
                InputFileParameterOrder.Visibility = Visibility.Visible;
                MainTabItem.Visibility = Visibility.Collapsed;
                OutputFileParameterOrder.Visibility = Visibility.Collapsed;
                InputFileParameterOrder.IsSelected = true;
            }
        }
    }
}
