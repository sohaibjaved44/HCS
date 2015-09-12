using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCS.Controllers;
using HCS.Helpers;
using HCS.UserControls;
using HCS.UserControls.History;
using HCS.UserControls.Purchase;
using HCS.UserControls.Reports;
using HCS.UserControls.Sale;
using HCS.UserControls.Users;

namespace HCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :Window, INotifyPropertyChanged
    {
        #region Private Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null){
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private MainWin mw;
        private HCSController m_controller;
        private string languagecode;
        private bool canUserEdit = false;
        
        #endregion

        #region Public Properties
       
        public HCSController CONTROLLER
        {
            get { return m_controller; }
            set { m_controller = value; }

        }
        
        
        #endregion

        #region Constructors and Loaders
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string language,bool canEdit)
        {
            InitializeComponent();
            languagecode = language;
            canUserEdit = canEdit;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWin = new MainWin();
            CONTROLLER = new HCSController(canUserEdit);
            setLanguage();
            this.grdModel.DataContext = CONTROLLER;
            MainWin.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            MainWin.isurduvisible = CONTROLLER.ISURDUVISIBLE;
        }

        public MainWin MainWin
        {
            get { return mw; }
            set { mw = value; OnPropertyChanged("MainWin"); }
        }
        #endregion

        #region Event Handlers



        private void RibbonLoaded(object sender, RoutedEventArgs e)
        {
            Grid child = VisualTreeHelper.GetChild((DependencyObject)sender, 0) as Grid;
            if (child != null)
            {
                child.RowDefinitions[0].Height = new GridLength(0);
            }
        }


        private void IndividualUser_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucIndividualUser tab = new ucIndividualUser(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

        }

        private void CompanyUser_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucCompanyUser tab = new ucCompanyUser(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;
        }

        private void FeedMillUser_Click(object sender, RoutedEventArgs e)
        {

            removePreviousTabs();

            ucFeedMillUser tab = new ucFeedMillUser(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

        }

        private void PurchaserUser_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucPurchaserUser tab = new  ucPurchaserUser(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;


        }


        private void PurchaseProduct_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucPurchaseProduct tab = new ucPurchaseProduct(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;


        }

        private void PurchaserHistory_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucPurchaserHistory tab = new ucPurchaserHistory(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;
        }


        private void SellerHistory_Click(object sender, RoutedEventArgs e)
        {

            removePreviousTabs();
            ucSellerHistory tab = new ucSellerHistory(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

        }


        private void SaleProduct_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucSaleProduct tab = new ucSaleProduct(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

        }

        private void FeedMillHistory_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            ucFeedMillHistory tab = new ucFeedMillHistory(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            
            removePreviousTabs();
            ucDailyReport tab = new ucDailyReport(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;

            
        }


        private void BankUser_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();

            ucBankUser tab = new ucBankUser(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;
        }

        #endregion

        #region Helper Methods
        
        void removePreviousTabs()
        {

            if (GenericTabs.Items.Count > 0)
            {
                GenericTabs.Items.RemoveAt(0);

            }
            

        }

        void setLanguage()
        {
            if(languagecode=="00001")
            {
                CONTROLLER.ISURDUVISIBLE = true;
                CONTROLLER.ISENGLISHVISIBLE = false;
            }
            else if(languagecode=="00002")
            {
                CONTROLLER.ISURDUVISIBLE = false;
                CONTROLLER.ISENGLISHVISIBLE = true;

            }


        }

        #endregion

        private void OtherHistory_Click(object sender, RoutedEventArgs e)
        {
            removePreviousTabs();
            OtherHistory tab = new OtherHistory(CONTROLLER);
            GenericTabs.Items.Add(tab);
            GenericTabs.SelectedIndex = 0;
        }


        
    }
}
