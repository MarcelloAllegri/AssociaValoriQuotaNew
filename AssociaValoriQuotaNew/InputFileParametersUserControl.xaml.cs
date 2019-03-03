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
    /// Logica di interazione per InputFileParametersUserControl.xaml
    /// </summary>
    public partial class InputFileParametersUserControl : UserControl
    {
        private int m_EstPosition;
        public int EstPosition
        {
            set { m_EstPosition = value; }
            get { return m_EstPosition; }
        }

        private int m_NorthPosition;
        public int NorthPosition
        {
            set { m_NorthPosition = value; }
            get { return m_NorthPosition; }
        }

        private int m_QuotePosition;
        public int QuotePosition
        {
            set { m_QuotePosition = value; }
            get { return m_QuotePosition; }
        }

        public InputFileParametersUserControl()
        {
            InitializeComponent();
        }

        private void ItemFoundedReadOnlyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ItemFoundedReadOnlyTextBox.Text != null)
                EstPositionNumericUpAndDown.Maximum = NorthPositionNumericUpAndDown.Maximum = quotePositionNumericUpAndDown.Maximum = Convert.ToInt32(ItemFoundedReadOnlyTextBox.Text);
            else
                EstPositionNumericUpAndDown.IsEnabled = NorthPositionNumericUpAndDown.IsEnabled = quotePositionNumericUpAndDown.IsEnabled = false;
        }

        public bool CheckFields()
        {
            if (m_EstPosition != m_NorthPosition && m_NorthPosition != m_QuotePosition && m_EstPosition != m_QuotePosition)
                return true;
            else return false;
        }
    }
}
