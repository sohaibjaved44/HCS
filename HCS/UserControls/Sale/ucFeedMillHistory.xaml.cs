using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HCS.Controllers;
using HCS.Popus;

namespace HCS.UserControls.Sale
{
    /// <summary>
    /// Interaction logic for ucFeedMillHistory.xaml
    /// </summary>
    public partial class ucFeedMillHistory : UserControl
    {

        #region Private Members

        private HCSController m_controller;
        private int start = 0;
        private int itemCount = 10;
        private int totalItems = 0;
        private ObservableCollection<saleproduct> m_FeedMillHistory;
        private saleproduct SaleProduct;
        private int feedMillId=0;
        private decimal totalRecievable = 0;
        #endregion

        #region Public Properties


        public HCSController CONTROLLER
        {

            get { return m_controller; }
            set { m_controller = value; }

        }

        public ObservableCollection<saleproduct> FEEDMILLHISTORY
        {
            get { return m_FeedMillHistory; }
            set { m_FeedMillHistory = value; }

        }

        #endregion

        #region Constructors and Loaders

        public ucFeedMillHistory()
        {
            InitializeComponent();
        }

        public ucFeedMillHistory(HCSController controller)
        {
            InitializeComponent();
            CONTROLLER = controller;
        
        }

        private void ucFeedMillHistory_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {

                CONTROLLER = new HCSController();
            }
            this.grdModel.DataContext = CONTROLLER;
            this.grdFeedMillHistory.DataContext = CONTROLLER;
            FEEDMILLHISTORY = new ObservableCollection<saleproduct>();

            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgSalesHistory.Columns[1].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[3].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[5].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[7].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[9].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[11].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[13].Visibility = Visibility.Collapsed;

            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgSalesHistory.Columns[0].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[2].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[4].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[6].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[8].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[10].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[12].Visibility = Visibility.Collapsed;
            }


            RefreshUsers();
            bindFeedMill();
        }

        #endregion

        #region Event Handlers
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(cmbFeedMill.SelectedItem!=null)
            {
                feedmilluser SelectedFeedMillUser = (feedmilluser)cmbFeedMill.SelectedItem;
                feedMillId = SelectedFeedMillUser.feedmillid;
                getFeedMillHistory();
            }


        }



        private void btnRecieveAmount_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(tbAmountRecieved.Text))
            {
                decimal amountReceived = Convert.ToDecimal(tbAmountRecieved.Text);

                SaleProduct = new saleproduct();
                SaleProduct.feedmillid = feedMillId;
                SaleProduct.amountrecieved = amountReceived;
                if (amountReceived > totalRecievable)
                {
                    SaleProduct.totalpricerecieveable = 0;
                    SaleProduct.totalpayable = amountReceived - totalRecievable;
                }
                else
                {
                    SaleProduct.totalpricerecieveable = totalRecievable - amountReceived;
                    SaleProduct.totalpayable = 0;

                }
                CONTROLLER.saveFeedMillPayment(SaleProduct);
                FEEDMILLHISTORY.Add(SaleProduct);
                MessageBox.Show("Amount Paid Saved Successfully!");
                RefreshUsers();
                setTotalRecievable();                
            
            }

        }

        #endregion

        #region Helper Methods
        
        void bindFeedMill()
        {

            this.cmbFeedMill.ItemsSource = CONTROLLER.FEEDMILLUSERS;

        }

        private void RefreshUsers()
        {
            ObservableCollection<saleproduct> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgSalesHistory.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private void getFeedMillHistory()
        {

            FEEDMILLHISTORY = new ObservableCollection<saleproduct>(CONTROLLER.getFeedMillHistory(feedMillId));
            RefreshUsers();
            setTotalRecievable(); 
        }
        private ObservableCollection<saleproduct> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = FEEDMILLHISTORY.Count;
            ObservableCollection<saleproduct> filteredProducts = new ObservableCollection<saleproduct>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                saleproduct product = FEEDMILLHISTORY[i];
                filteredProducts.Add(product);
            }
            return filteredProducts;
        }
        private void setTotalRecievable()
        {
            
            if(FEEDMILLHISTORY!=null && FEEDMILLHISTORY.Count>0)
            {
                totalRecievable = FEEDMILLHISTORY.LastOrDefault().totalpricerecieveable;
               if(CONTROLLER.CANUSEREDIT)
               { 

                btnSave.IsEnabled = true;
                btnSaveUrdu.IsEnabled = true;
               }
            }
            else
            {
                btnSave.IsEnabled = false;
                btnSaveUrdu.IsEnabled = false;
                totalRecievable = 0;
            }

            tbTotalAmountRecievable.Text = totalRecievable.ToString("##,#.00");
            tbAmountRecieved.Text = "";

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

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (CONTROLLER.CANUSEREDIT)
            {
                saleproduct obj = ((FrameworkElement)sender).DataContext as saleproduct;
                if (obj.productid > 0)
                {
                    obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
                    obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                    CONTROLLER.OLDRECEIVABLEPRICE = obj.recievableprice;
                    winSaleEdit window = new winSaleEdit(CONTROLLER, obj);
                    window.ShowDialog();
                    getFeedMillHistory();

                }
            }
        }




    }
}
