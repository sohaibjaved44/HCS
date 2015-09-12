using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using HCS.Controllers;

namespace HCS.UserControls.History
{


    /// <summary>
    /// Interaction logic for DetailedHistory.xaml
    /// </summary>
    /// 
    public partial class DetailedHistory : Window
    {
        private HCS.History m_History;
        public HCSController Controller { get; set; }

        public HCS.History HISTORY
        {
            get { return m_History; }
            set { m_History = value; }
        }
        public DetailedHistory()
        {
            InitializeComponent();
        }

        public DetailedHistory(HCSController p_controller, Khata khata)
        {
            InitializeComponent();
            HISTORY = getDetail(p_controller, khata);
           // grdDetailHistory.DataContext = HISTORY;
        }

        private HCS.History getDetail(HCSController p_controller, Khata khata)
        {
            HCS.History history = new HCS.History();
            Controller = p_controller;
            m_History = new HCS.History()
            {


            };

            return history;
        }

        
            

        private void DetailedHistory_Loaded(object sender, RoutedEventArgs e)
        {
            grdDetail.DataContext = Controller;
            grdDetailHistory.DataContext = HISTORY;
        }

        
    }
}
