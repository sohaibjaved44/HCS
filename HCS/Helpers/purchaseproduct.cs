using System.ComponentModel;

namespace HCS
{
    public partial class purchaseproduct : INotifyPropertyChanged
    {
        #region Private Members
        private bool m_areFieldsEnabled = true;                
        #endregion
        public bool isurduvisible { get; set; }
        public bool isenglishvisible { get; set; }
        public string purchaser_cde { get; set; }
        public bool AREFIELDSENABLED
        {

            get {

                return m_areFieldsEnabled;
            }
            set
            {

                m_areFieldsEnabled = value;
                NotifyPropertyChanged("AREFIELDSENABLED");

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public decimal totalweight { get; set; }
        public product selectedProduct { get; set; }
        public int totalreceivable { get; set; }
    }
}
