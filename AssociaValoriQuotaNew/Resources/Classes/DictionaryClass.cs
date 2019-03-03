using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociaValoriQuotaNew.Resources.Classes
{
    public class DictionaryClass : INotifyPropertyChanged
    {
        public DictionaryClass()
        {

        }

        public DictionaryClass(char key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        private char m_key;
        private string m_Value;

        public event PropertyChangedEventHandler PropertyChanged;

        public char Key
        {
            set
            {
                m_key = value;
                OnPropertyChanged(nameof(Key));
            }
            get { return m_key; }
        }

        public string Value
        {
            set
            {
                m_Value = value;
                OnPropertyChanged(nameof(Value));
            }
            get { return m_Value; }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
