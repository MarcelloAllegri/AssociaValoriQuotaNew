using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociaValoriQuotaNew.Resources.Classes
{
    public class Campi
    {
        public Campi()
        {

        }

        public Campi(double campoEst, double campoNord, double campoQuota, double diff)
        {
            this.CampoEst = campoEst;
            this.CampoNord = campoNord;
            this.CampoQuota = campoQuota;
            this.DifferenceValue = diff;
            this.Calculate();
        }

        private double m_CampoEst;

        public double CampoEst
        {
            get { return m_CampoEst; }
            set { m_CampoEst = value; }
        }

        private double m_CampoNord;

        public double CampoNord
        {
            get { return m_CampoNord; }
            set { m_CampoNord = value; }
        }

        private double m_CampoQuota;

        public double CampoQuota
        {
            get { return m_CampoQuota; }
            set { m_CampoQuota = value; }
        }

        private double m_DifferenceValue;

        public double DifferenceValue
        {
            get { return m_DifferenceValue; }
            set { m_DifferenceValue = value; }
        }

        private double m_Result;

        public double Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

        public void Calculate()
        {
            if (this != null)
                this.Result = this.CampoQuota - this.DifferenceValue;
        }

        //public string ToString(char separator, Dictionary<char, bool> ViewDictionary)
        //{
        //    return (ViewDictionary['E'] == true ? m_CampoEst.ToString() + separator : string.Empty) +
        //        (ViewDictionary['N'] == true ? m_CampoNord.ToString() + separator : string.Empty) +
        //        (ViewDictionary['Q'] == true ? m_CampoQuota.ToString() + separator : string.Empty) +
        //        (ViewDictionary['D'] == true ? m_DifferenceValue.ToString().Replace(',','.') + separator : string.Empty) +
        //        (ViewDictionary['R'] == true ? m_Result.ToString().Replace(',', '.') : string.Empty);

        //}        
        public string this[string Value]
        {
            get
            {
                switch (Value)
                {
                    case "CampoEst": return this.CampoEst.ToString();
                    case "CampoNord": return this.CampoNord.ToString();
                    case "CampoQuota": return this.CampoQuota.ToString();
                    case "DifferenceValue": return this.DifferenceValue.ToString();
                    case "Result": return this.Result.ToString();
                    default: return string.Empty;
                }
            }

        }
    }
}
