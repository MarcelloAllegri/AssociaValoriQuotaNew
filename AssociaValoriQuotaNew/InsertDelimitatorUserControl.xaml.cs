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
    /// Logica di interazione per InsertDelimitatorUserControl.xaml
    /// </summary>
    public partial class InsertDelimitatorUserControl : UserControl
    {
        private bool m_IsInputDelimiter;

        public bool IsInputDelimiter
        {
            set { m_IsInputDelimiter = value; }
            get { return m_IsInputDelimiter; }
        }

        public InsertDelimitatorUserControl()
        {
            InitializeComponent();
        }

        public string Save()
        {
            return this.CharacterDelimiterTextBox.Text;
        }

    }
}
