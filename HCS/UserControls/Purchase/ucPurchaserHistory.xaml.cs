using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using HCS.Controllers;

namespace HCS.UserControls.Purchase
{
    /// <summary>
    /// Interaction logic for ucPurchaserHistory.xaml
    /// </summary>
    public partial class ucPurchaserHistory : UserControl
    {
        
        #region Private Members

        private HCSController m_controller;
        private int start = 0;
        private int itemCount = 7;
        private int totalItems = 0;
        private ObservableCollection<purchaseproduct> m_PurchaserHistory;
        
        private int purchaserId = 0;
        
        #endregion

        #region Public Properties

       
        public HCSController CONTROLLER
        {

            get { return m_controller; }
            set { m_controller = value; }

        }

        public ObservableCollection<purchaseproduct> PURCHASEHSITORY
        {
            get { return m_PurchaserHistory; }
            set { m_PurchaserHistory = value; }

        }

       
        #endregion

        #region Constructors and Loaders
        public ucPurchaserHistory()
        {
            InitializeComponent();
        }

        public ucPurchaserHistory( HCSController controller)
        {
            InitializeComponent();
            CONTROLLER = controller;
        
        }

        private void ucPurchaserHistory_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {
                CONTROLLER = new HCSController();
            }
            this.grdModel.DataContext = CONTROLLER;
            this.grdPurchaserHistory.DataContext = CONTROLLER;
            PURCHASEHSITORY = new ObservableCollection<purchaseproduct>();
            //SALEHISTORY = new ObservableCollection<saleproduct>();
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgPurchaser.Columns[1].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[3].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[5].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[7].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[9].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgPurchaser.Columns[0].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[2].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[4].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[6].Visibility = Visibility.Collapsed;
                dgPurchaser.Columns[8].Visibility = Visibility.Collapsed;
            }


            RefreshUsers();
        
            bindPurchasers();
        }

        #endregion

        #region Event Handlers
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPurchaser.SelectedItem != null)
            {
                purchaseruser SelectedFeedMillUser = (purchaseruser)cmbPurchaser.SelectedItem;
                purchaserId = SelectedFeedMillUser.purchaserid;
                PURCHASEHSITORY = new ObservableCollection<purchaseproduct>(CONTROLLER.getPurchaserHistory(purchaserId));
                //SALEHISTORY = new ObservableCollection<saleproduct>(CONTROLLER.getSaleHistory(purchaserId));
                RefreshUsers();
                //setTotalRecievable();
            }

        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

      

        #region Helper Methods 

        private void bindPurchasers()
        {

            this.cmbPurchaser.ItemsSource = CONTROLLER.PURCHASERUSERS;

        }

        private void RefreshUsers()
        {
            ObservableCollection<purchaseproduct> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgPurchaser.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<purchaseproduct> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = PURCHASEHSITORY.Count;
            ObservableCollection<purchaseproduct> filteredProducts = new ObservableCollection<purchaseproduct>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                purchaseproduct product = PURCHASEHSITORY[i];
                filteredProducts.Add(product);
            }
            return filteredProducts;
        }
        #endregion

        private void BtnFirst_OnClick(object sender, RoutedEventArgs e)
        {
            start = 0;
            RefreshUsers();

        }

        private void BtnPrevious_OnClick(object sender, RoutedEventArgs e)
        {
            if ((start - itemCount) > -1)
            {
                start -= itemCount;
                RefreshUsers();
            }
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            if ((start + itemCount) < totalItems)
            {
                start += itemCount;
                RefreshUsers();
            }
        }

        private void BtnLast_OnClick(object sender, RoutedEventArgs e)
        {
            start = (totalItems / itemCount - 1) * itemCount;
            start += totalItems % itemCount == 0 ? 0 : itemCount;
            RefreshUsers();
        }

    }
}
