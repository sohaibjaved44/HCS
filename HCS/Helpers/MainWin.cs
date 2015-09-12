using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCS.Helpers
{
    public partial class MainWin : INotifyPropertyChanged
    {
        private bool _urduvisible;
        private bool _englishvisible;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));//event is raised here which notifies xaml that property is changed.
            }
        }
        public bool isurduvisible { get { return _urduvisible; } set { _urduvisible = value; OnPropertyChanged("isurduvisible"); } }
        public bool isenglishvisible { get { return _englishvisible; } set { _englishvisible = value; OnPropertyChanged("isenglishvisible"); } }
    }
}
