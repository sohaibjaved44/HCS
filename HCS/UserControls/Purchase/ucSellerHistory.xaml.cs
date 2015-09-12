using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HCS.Controllers;
using HCS.Enums;
using HCS.Popus;
using HCS.UserControls.History;

namespace HCS.UserControls.Purchase
{
    /// <summary>
    /// Interaction logic for ucSellerHistory.xaml
    /// </summary>
    public partial class ucSellerHistory : UserControl
    {
        #region Private Members 
        
        private HCSController m_controller;
        List<Seller> m_sellers;
        private int start = 0;
        private int itemCount = 7;
        private int totalItems = 0;
        //private ObservableCollection<purchaseproduct> m_purchaseProductHistory;
        //private ObservableCollection<saleproduct> m_SaleProductHistory;
        //private ObservableCollection<HCS.History> m_History;
        private ObservableCollection<Khata> m_History;
        private string sellertype;
        private int sellerid;
        private purchaseproduct PurchaseProduct;
        private decimal totalPayable = 0;

        #endregion

        #region Public Properties
        
        
        public HCSController CONTROLLER
        {

            get { return m_controller; }
            set { m_controller = value; }

        }
        

        List<Seller> SELLERS
        {

            get
            {
                return m_sellers;

            }

            set
            {

                m_sellers = value;
            }

        }
        #region Commented Poperties of History
        //public ObservableCollection<saleproduct> SP_SALEHISTORY
        //{
        //    get { return m_SaleProductHistory; }
        //    set { m_SaleProductHistory = value; }

        //}

        //public ObservableCollection<purchaseproduct> PP_SELLERHSITORY
        //{
        //    get { return m_purchaseProductHistory; }
        //    set { m_purchaseProductHistory = value; }

        //}
        public ObservableCollection<Khata> HISTORY
        {
            get { return m_History; }
            set { m_History = value; }

        }
        #endregion


        #endregion

        #region Constructors and Loaders
        public ucSellerHistory()
        {
            InitializeComponent();
        }

        public ucSellerHistory(HCSController controller)
        {
            InitializeComponent();
            CONTROLLER = controller;
        }

        private void ucSellerHistory_Loaded(object sender, RoutedEventArgs e)
        {
            HISTORY = new ObservableCollection<Khata>();
            if (CONTROLLER == null)
            {
                CONTROLLER = new HCSController();
            }
            this.grdModel.DataContext = CONTROLLER;
            this.grdSellerHistory.DataContext = CONTROLLER;
            //PP_SELLERHSITORY = new ObservableCollection<purchaseproduct>();
            //SP_SALEHISTORY = new ObservableCollection<saleproduct>();
            //HISTORY = new ObservableCollection<HCS.History>();
            
            dgSalesHistory.ItemsSource = HISTORY;
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgSalesHistory.Columns[1].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[3].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[5].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[7].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[9].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[11].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[13].Visibility = Visibility.Collapsed;
                dgSalesHistory.Columns[15].Visibility = Visibility.Collapsed;
                
                
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
                dgSalesHistory.Columns[14].Visibility = Visibility.Collapsed;
                
            }

            RefreshUsers();
            bindSellerTypes();
        }
        
        #endregion

        #region Event Handlers

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            HISTORY = new ObservableCollection<Khata>();
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbSellerType_e.SelectedItem != null && cmbSeller_e.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellerType_e.SelectedItem;
                    Seller seller = (Seller)cmbSeller_e.SelectedItem;
                    sellertype = sellerType.seller_cde;
                    sellerid = seller.sellerid;
                    //filling the collection as per search criteria
                    using (var db = new HCSMLEntities1())
                    {
                        var qry = from k in db.Khatas where k.bpid==sellerid select k;
                        foreach (var item in (qry))
                        {
                            Khata kh = new Khata();
                            
                            kh.date = item.date;
                            kh.activitycode = item.activitycode;
                            kh.id = item.id;
                            kh.payable_naam = item.payable_naam;
                            kh.productid = item.productid;
                            kh.bpid = item.bpid;
                            kh.purchasertypecode = item.purchasertypecode;
                            kh.receivable_jama = item.receivable_jama;
                            kh.sellertypecode = item.sellertypecode;
                            kh.bpname = getName(kh.sellertypecode, kh.bpid);
                            kh.totalpayable_naam = item.totalpayable_naam;
                            kh.totalreceivable_jama = item.totalreceivable_jama;
                            kh.purchaseproductid = item.purchaseproductid;
                            kh.saleproductid = item.saleproductid;
                            HISTORY.Add(kh);
                        }
                    }

                    //PP_SELLERHSITORY = new ObservableCollection<purchaseproduct>(CONTROLLER.getHistoryPurchaseProduct(sellertype, sellerid));
                    //SP_SALEHISTORY = new ObservableCollection<saleproduct>(CONTROLLER.getHistorySaleProduct(sellertype, sellerid));
                    //HISTORY = new ObservableCollection<HCS.History>(fillHistory(SP_SALEHISTORY,PP_SELLERHSITORY));
                    RefreshUsers();
                    //setTotalRecievable();
                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbSellerType_u.SelectedItem != null && cmbSeller_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellerType_u.SelectedItem;
                    Seller seller = (Seller)cmbSeller_u.SelectedItem;
                    sellertype = sellerType.seller_cde;
                    sellerid = seller.sellerid;
                    using (var db = new HCSMLEntities1())
                    {
                        var qry = from k in db.Khatas where k.bpid == sellerid select k;
                        foreach (var item in (qry))
                        {
                            Khata kh = new Khata();
                            kh.date = item.date;
                            kh.activitycode = item.activitycode;
                            kh.id = item.id;
                            kh.payable_naam = item.payable_naam;
                            kh.productid = item.productid;
                            kh.bpid = item.bpid;
                            kh.purchasertypecode = item.purchasertypecode;
                            kh.receivable_jama = item.receivable_jama;
                            kh.sellertypecode = item.sellertypecode;
                            kh.bpname = getName(kh.sellertypecode, kh.bpid);                            
                            kh.totalpayable_naam = item.totalpayable_naam;
                            kh.totalreceivable_jama = item.totalreceivable_jama;
                            kh.productname = getProductName(kh.productid);
                            kh.purchaseproductid = item.purchaseproductid;
                            kh.saleproductid = item.saleproductid;
                            HISTORY.Add(kh);
                        }
                    }
                    //PP_SELLERHSITORY = new ObservableCollection<purchaseproduct>(CONTROLLER.getHistoryPurchaseProduct(sellertype, sellerid));
                    //SP_SALEHISTORY = new ObservableCollection<saleproduct>(CONTROLLER.getHistorySaleProduct(sellertype, sellerid));
                    //HISTORY = new ObservableCollection<HCS.History>(fillHistory(SP_SALEHISTORY, PP_SELLERHSITORY));
                    RefreshUsers();
                    //setTotalRecievable();
                }
            }
           

        }

        private string getProductName(int productId)
        {
            string retName = "";
            using (var db = new HCSMLEntities1())
            {
                if (CONTROLLER.ISENGLISHVISIBLE)
                {
                    retName = (from p in db.products where p.productid == productId select p.productname_e).FirstOrDefault();
                }
                if (CONTROLLER.ISURDUVISIBLE)
                {
                    retName = (from p in db.products where p.productid == productId select p.productname_u).FirstOrDefault();
                }
            }
            return retName;
        }
         
        private string getName(string typecode,int userid)
        {
            string retName = "";
            using (var db = new HCSMLEntities1())
            {
                if (CONTROLLER.ISENGLISHVISIBLE)
                {
                    if (typecode == "00001")
                    {
                        retName = (from n in db.individualusers where n.individualid == userid select n.name_e).FirstOrDefault();
                    }
                    else if (typecode == "00002")
                    {
                        retName = (from n in db.companyusers where n.companyid == userid select n.companyname_e).FirstOrDefault();
                    }
                    else if (typecode == "00003")
                    {
                        retName = (from n in db.feedmillusers where n.feedmillid == userid select n.feemillname_e).FirstOrDefault();
                    }
                    else if (typecode == "00004")
                    {
                        retName = (from n in db.bankusers where n.bankid == userid select n.bankname_e).FirstOrDefault();
                    }
                   
                }
                if (CONTROLLER.ISURDUVISIBLE)
                {
                    if (typecode == "00001")
                    {
                        retName = (from n in db.individualusers where n.individualid == userid select n.name_u).FirstOrDefault();
                    }
                    else if (typecode == "00002")
                    {
                        retName = (from n in db.companyusers where n.companyid == userid select n.companyname_u).FirstOrDefault();
                    }
                    else if (typecode == "00003")
                    {
                        retName = (from n in db.feedmillusers where n.feedmillid == userid select n.feedmillname_u).FirstOrDefault();
                    }
                    else if (typecode == "00004")
                    {
                        retName = (from n in db.bankusers where n.bankid == userid select n.bankname_u).FirstOrDefault();
                    }

                }
            }
            return retName;
        }
        #region Commenting as we are using khata table now
        /*
        private ObservableCollection<HCS.History> fillHistory(ObservableCollection<saleproduct> p_spSaleHistory, ObservableCollection<purchaseproduct> p_ppSellerHistory)
        {
            ObservableCollection<HCS.History> retHistory = new ObservableCollection<HCS.History>();


            foreach (var item in p_ppSellerHistory)
            {
                HCS.History history = new HCS.History();

                history.seqid = item.seqid;
                history.amoutpaid = item.amoutpaid;
                history.bagweight = item.bagweight;
                history.commission = item.commission;
                history.commissionamt = item.commissionamt;
                history.date = item.date;
                history.extraweight = item.extraweight;
                history.labour = item.labour;
                history.noofbags = item.noofbags;
                history.payableprice = item.payableprice;
                history.purchasername = item.purchasername;
                history.price = item.price;
                history.productid = item.productid;
                history.productname = item.productname;
                history.purchaserid = item.purchaserid;
                history.rate = item.rate;
                history.seller_cde = item.seller_cde;
                history.sellerid = item.sellerid;
                history.sellername = item.sellername;
                history.sellertype = item.sellertype;
                history.totalpayable = item.totalpayable;                

                retHistory.Add(history);
                
            }

            foreach (var item in p_spSaleHistory)
            {
                HCS.History history = new HCS.History();

                history.seqid = item.seqid;
                history.amoutpaid = 0;
                history.bagweight = item.bagweight;
                history.commission = item.commissionpercent;
                history.commissionamt = item.commissionamount;
                history.date = item.date;
                history.extraweight = item.extraweight;
                history.labour = item.labour;
                history.noofbags = item.noofbags;
                history.payableprice = 0;
                history.purchasername = item.purchasername;
                history.price = item.price;
                history.productid = item.productid;
                history.productname = item.productname;
                //history.purchaserid = item.purchasertypecde;
                history.rate = item.rate;
                //history.seller_cde = 
                //history.sellerid = item.pu;
                history.sellername = item.sellername;
                //history.sellertype = item.sellertype;
                history.totalpayable = item.totalpayable;
                history.totalreceivable = item.totalpricerecieveable;
                //history.received = item.re
                retHistory.Add(history);
            }

            return retHistory;
        }*/
#endregion
        private void cmbSellerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbSellerType_e.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellerType_e.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        loadFeedMillUser();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSUser();
                    }
                }  
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbSellerType_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellerType_u.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        loadFeedMillUser();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSUser();
                    }
                }  
            }
            

        }

        
        
        #endregion

        #region Helper Methods
        void bindSellerTypes()
        {

            this.cmbSellerType_e.ItemsSource = CONTROLLER.SELLERTYPES;
            this.cmbSellerType_u.ItemsSource = CONTROLLER.SELLERTYPES;
        }

        private void loadIndividualSeller()
        {
            SELLERS = new List<Seller>();

            foreach (individualuser ind in CONTROLLER.INDIVIDUALUSERS)
            {

                SELLERS.Add(new Seller() { sellerid = ind.individualid, sellername_e = ind.name_e ,sellername_u = ind.name_u});

            }
            this.cmbSeller_e.ItemsSource = SELLERS;
            this.cmbSeller_u.ItemsSource = SELLERS;
        }

        private void loadCompanySeller()
        {
            SELLERS = new List<Seller>();
         
            foreach (companyuser cmpy in CONTROLLER.COMPANYUSERS)
            {

                SELLERS.Add(new Seller() { sellerid = cmpy.companyid, sellername_e = cmpy.companyname_e ,sellername_u = cmpy.companyname_u});

            }
            this.cmbSeller_e.ItemsSource = SELLERS;
            this.cmbSeller_u.ItemsSource = SELLERS;
        }
        private void loadBankSUser()
        {
            SELLERS = new List<Seller>();

            foreach (bankuser bnk in CONTROLLER.BANKUSERS)
            {

                SELLERS.Add(new Seller() { sellerid = bnk.bankid, sellername_e = bnk.bankname_e, sellername_u = bnk.bankname_u });

            }
            this.cmbSeller_e.ItemsSource = SELLERS;
            this.cmbSeller_u.ItemsSource = SELLERS;
        }
        private void loadFeedMillUser()
        {
            SELLERS = new List<Seller>();

            foreach (feedmilluser fmu in CONTROLLER.FEEDMILLUSERS)
            {

                SELLERS.Add(new Seller() { sellerid = fmu.feedmillid, sellername_e = fmu.feedmillname_u, sellername_u = fmu.feedmillname_u });

            }
            this.cmbSeller_e.ItemsSource = SELLERS;
            this.cmbSeller_u.ItemsSource = SELLERS;
        }

        //private void setTotalRecievable()
        //{

        //    if (PP_SELLERHSITORY  != null && PP_SELLERHSITORY.Count>0)
        //    {
        //        totalPayable = PP_SELLERHSITORY.LastOrDefault().totalpayable;
        //        if (CONTROLLER.CANUSEREDIT)
        //        {
        //            btnSave.IsEnabled = true;
        //            btnSaveUrdu.IsEnabled = true;
        //        }
        //    }
        //    else
        //    {
        //        btnSave.IsEnabled = false;
        //        btnSaveUrdu.IsEnabled = false;
        //        totalPayable = 0;
        //    }

        //    tbTotalAmountPayable.Text = totalPayable.ToString("##,#.00");
        //   tbAmountPaid.Text= "";

        //}

        #endregion

     private void RefreshUsers()
        {
            ObservableCollection<Khata> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgSalesHistory.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<Khata> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = HISTORY.Count ;
            ObservableCollection<Khata> filteredProducts = new ObservableCollection<Khata>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                Khata product = HISTORY[i];
                filteredProducts.Add(product);
            }
            return filteredProducts;
        }

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

        //private void btnPayAmount_Click(object sender, RoutedEventArgs e)
        //{
  
        //    if (!string.IsNullOrEmpty(tbAmountPaid.Text))
        //    {
        //        decimal amountPaid = Convert.ToDecimal(tbAmountPaid.Text);
        //        PurchaseProduct = new purchaseproduct();
        //        PurchaseProduct.sellerid = sellerid;
        //        PurchaseProduct.seller_cde = sellertype;
        //        PurchaseProduct.amoutpaid = amountPaid;
        //        if (amountPaid > totalPayable)
        //        {
        //            PurchaseProduct.totalpayable = 0;
        //            PurchaseProduct.totalreceivable = (int) (amountPaid - totalPayable);

        //        }
        //        else
        //        {
        //            PurchaseProduct.totalpayable = totalPayable - amountPaid;
        //            PurchaseProduct.totalreceivable = 0;
        //        }
        //            CONTROLLER.saveSellerPayment(PurchaseProduct);
        //        PP_SELLERHSITORY.Add(PurchaseProduct);
        //        MessageBox.Show("Amount Paid Saved Successfully!");
        //        RefreshUsers();
        //        //setTotalRecievable();

        //    }

        //}

        private void showDetail_Click(object sender, RoutedEventArgs e)
        {
            
            //HCS.History hstry = ((FrameworkElement)sender).DataContext as HCS.History;
            Khata khata = (Khata)dgSalesHistory.SelectedItem;
            int id = 0;
            if (khata.activitycode == ActivityType.Purchase.GetStringValue())
            {
                CONTROLLER.PURCHASEPRODUCT = new purchaseproduct();
                //fill data context for purchase Product and then invoke purchase Product screen here.
                using (var db = new HCSMLEntities1())
                {
                    var purchaseproductfromdb = from product in db.purchaseproducts
                        where product.seqid == khata.purchaseproductid
                        select product;
                    CONTROLLER.PURCHASEPRODUCT = (purchaseproduct) purchaseproductfromdb.FirstOrDefault();

                    CONTROLLER.PURCHASEPRODUCT.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                    CONTROLLER.PURCHASEPRODUCT.isurduvisible = CONTROLLER.ISURDUVISIBLE;                    
                }
                //invoking screen to show purchase detail.
                winPurchaseProductPopUp wppp = new winPurchaseProductPopUp(CONTROLLER);
                wppp.Show();
            }
            if (khata.activitycode == ActivityType.Sale.GetStringValue())
            {
                CONTROLLER.SALEPRODUCT = new saleproduct();
                using (var db = new HCSMLEntities1())
                {
                    var saleproductfromdb = from saleProduct in db.saleproducts
                                                where saleProduct.seqid == khata.saleproductid
                                                select saleProduct;
                    CONTROLLER.SALEPRODUCT = (saleproduct)saleproductfromdb.FirstOrDefault();
                    CONTROLLER.SALEPRODUCT.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                    CONTROLLER.SALEPRODUCT.isurduvisible = CONTROLLER.ISURDUVISIBLE;
                }
                //invoke sale Product screen
                winSaleProductPopUp wspp = new winSaleProductPopUp(CONTROLLER);
                wspp.Show();
            }
                                        

            /*
            individualuser obj = ((FrameworkElement)sender).DataContext as individualuser;
            obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
            individualUser = obj;
            loadImage();
            this.grdIndividualDetail.DataContext = individualUser;*/
            
        }



    }
}
