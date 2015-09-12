using System;
using System.ComponentModel;

namespace HCS
{
    public partial class GenerateReportInfo : INotifyPropertyChanged
    {
        #region Private Members 
        private DateTime m_fromDate;
        private DateTime m_toDate;
        #endregion


        #region Public Properties
        public DateTime FROMDATE
        {
            get
            {
                return m_fromDate;
            }
            set
            {
                m_fromDate = value;
                NotifyPropertyChanged("FROMDATE");
            }
        }

        public DateTime TODATE
        {
            get
            {
                return m_toDate;
            }
            set
            {
                m_toDate = value;
                NotifyPropertyChanged("TODATE");
            }
        }

        public bool isenglishvisible
        {
            get;
            set;

        }

        public bool isurduvisible
        {
            get;
            set;

        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
