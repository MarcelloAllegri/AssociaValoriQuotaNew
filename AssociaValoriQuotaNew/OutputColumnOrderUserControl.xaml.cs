using AssociaValoriQuotaNew.Resources.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Logica di interazione per OutputColumnOrderUserControl.xaml
    /// </summary>
    public partial class OutputColumnOrderUserControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<DictionaryClass> m_LeftQuoteOrder;
        private ObservableCollection<DictionaryClass> m_RightQuoteOrder;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<DictionaryClass> LeftQuoteOrder
        {
            set
            {
                m_LeftQuoteOrder = value;
                OnPropertyChanged(nameof(LeftQuoteOrder));
            }
            get { return m_LeftQuoteOrder; }
        }

        public ObservableCollection<DictionaryClass> RightQuoteOrder
        {
            set
            {
                m_RightQuoteOrder = value;
                OnPropertyChanged(nameof(RightQuoteOrder));
            }
            get { return m_RightQuoteOrder; }
        }

        public OutputColumnOrderUserControl()
        {
            InitializeComponent();
        }

        
        private void RightToLeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (RightListBox.SelectedItem is DictionaryClass selectedItem)
            {                
                LeftQuoteOrder.Add(selectedItem);
                RightQuoteOrder.Remove(selectedItem);                    
            }

            LeftListBox.Items.Refresh();
            RightListBox.Items.Refresh();
        }

        private void LeftToRightButton_Click(object sender, RoutedEventArgs e)
        {
            if (LeftListBox.SelectedItem is DictionaryClass selectedItem)
            {
                RightQuoteOrder.Add(selectedItem);
                LeftQuoteOrder.Remove(selectedItem);
            }

            LeftListBox.Items.Refresh();
            RightListBox.Items.Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LeftQuoteOrder = new ObservableCollection<DictionaryClass>() {
               new DictionaryClass('E' , "Est" ),
               new DictionaryClass( 'N' , "Nord" ),
               new DictionaryClass( 'Q' , "Quota" ),
               new DictionaryClass( 'D' , "Diff. Value" ),
               new DictionaryClass( 'R' , "Result" )
            };           

            RightQuoteOrder = new ObservableCollection<DictionaryClass>();
        }

       public bool CheckList()
       {
            if (RightQuoteOrder.Count == 0) return false;
            else return true;
       }        

        public Dictionary<char,bool> ReturnVisibilityOfColumn()
        {
            Dictionary<char, bool> ViewDictionary = new Dictionary<char, bool>();

            if (RightQuoteOrder.Any(x=> x.Key == 'E' && x.Value == "Est"))
                ViewDictionary.Add('E', true);
            else
                ViewDictionary.Add('E', false);
            if (RightQuoteOrder.Any(x => x.Key == 'N' && x.Value == "Nord"))
                ViewDictionary.Add('N', true);
            else
                ViewDictionary.Add('N', false);
            if (RightQuoteOrder.Any(x => x.Key == 'Q' && x.Value == "Quota")) 
                ViewDictionary.Add('Q', true);
            else
                ViewDictionary.Add('Q', false);
            if (RightQuoteOrder.Any(x => x.Key == 'D' && x.Value == "Diff. Value")) 
                ViewDictionary.Add('D', true);
            else
                ViewDictionary.Add('D', false);
            if (RightQuoteOrder.Any(x => x.Key == 'R' && x.Value == "Result")) 
                ViewDictionary.Add('R', true);
            else
                ViewDictionary.Add('R', false);

            return ViewDictionary;
        }
    }
}
