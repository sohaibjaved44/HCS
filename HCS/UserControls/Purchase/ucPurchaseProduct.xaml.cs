using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HCS.Controllers;
using HCS.reportwindows;

namespace HCS.UserControls.Purchase
{
    /// <summary>
    /// Interaction logic for ucPurchaseProduct.xaml
    /// </summary>
    public partial class ucPurchaseProduct : UserControl
    {
        #region Private Members
       purchaseproduct purchaseProduct;
       List<Seller> m_sellers;
       HCSController m_controller;
       
       private string m_seller_cde;
       private string m_purchaser_cde;
        private bool isCalculated = false;
        #endregion

        #region Public Properties

       HCSController CONTROLLER
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

        

        #endregion

        #region Constructors and Loaders

        public ucPurchaseProduct()
        {
            InitializeComponent();
        }

        public ucPurchaseProduct(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }

        private void ucPurchaseProduct_Loaded(object sender, RoutedEventArgs e)
        {
            if (CONTROLLER == null)
            {
                CONTROLLER = new HCSController();
            }


            purchaseProduct = new purchaseproduct() { date= DateTime.Now , isenglishvisible=CONTROLLER.ISENGLISHVISIBLE,isurduvisible=CONTROLLER.ISURDUVISIBLE };
           
            this.grdPurchaseProduct.DataContext = purchaseProduct;
            this.grdModel.DataContext = purchaseProduct;
            bindProducts();
            bindSellerType();
            bindPurchasers();
            bindPurchaserType();
            bindBank();


        }

        #endregion

        #region Event Handlers

        private void cmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbBankName_u.IsEnabled = false;
            cmbBankName_e.IsEnabled = false;
            tbChequeNo.IsEnabled = false;


            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbProduct_e.SelectedItem != null)
                {
                    purchaseProduct.selectedProduct = (product)cmbProduct_e.SelectedItem;
                    if (purchaseProduct.selectedProduct.producttype == ProductType.Cash.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = false;
                        tbPrice.IsReadOnly = false;
                    }
                    else if (purchaseProduct.selectedProduct.producttype == ProductType.Goods.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = true;
                        tbPrice.IsReadOnly = true;

                    }
                    else if (purchaseProduct.selectedProduct.producttype == ProductType.cheque.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = false;
                        tbPrice.IsReadOnly = false;
                        cmbBankName_e.IsEnabled = true;
                        tbChequeNo.IsEnabled = true;
                    }
                }
            }
            else if (CONTROLLER.ISURDUVISIBLE)
            {
                
                if (cmbProduct_u.SelectedItem != null)
                {
                    purchaseProduct.selectedProduct = (product) cmbProduct_u.SelectedItem;
                    if (purchaseProduct.selectedProduct.producttype == ProductType.Cash.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = false;
                        tbPrice.IsReadOnly = false;
                    }
                    else if (purchaseProduct.selectedProduct.producttype == ProductType.Goods.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = true;
                        tbPrice.IsReadOnly = true;

                    }
                    else if (purchaseProduct.selectedProduct.producttype == ProductType.cheque.GetStringValue())
                    {
                        purchaseProduct.AREFIELDSENABLED = false;
                        tbPrice.IsReadOnly = false;
                        cmbBankName_u.IsEnabled = true;
                        tbChequeNo.IsEnabled = true;
                    }

                }
            }
        }


        
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            
            if (!Validate())
            {
                return;
            }
            
            
            btnSave.IsEnabled = true;
            btnSaveUrdu.IsEnabled = true;
            purchaseProduct.totalweight = purchaseProduct.noofbags * purchaseProduct.bagweight + purchaseProduct.extraweight;
            if(purchaseProduct.selectedProduct.producttype!=ProductType.Cash.GetStringValue() &&
                purchaseProduct.selectedProduct.producttype != ProductType.cheque.GetStringValue()
                )
            {
                purchaseProduct.price = purchaseProduct.rate * (purchaseProduct.totalweight / 40);
            }
            purchaseProduct.commissionamt = purchaseProduct.price * (purchaseProduct.commission / 100);
            if (tbLabour.Text != "")
            {
                purchaseProduct.labour = Convert.ToDecimal(tbLabour.Text)*purchaseProduct.noofbags;
            }
            if (purchaseProduct.seller_cde == "00002")//if commission shop then add labor to purchaser khata (price +labor) and dont subtract from payable price
            {
                purchaseProduct.payableprice = purchaseProduct.price;
                purchaseProduct.price = purchaseProduct.price + purchaseProduct.labour;
                
            }
            else
            {
                purchaseProduct.payableprice = purchaseProduct.price;
                purchaseProduct.payableprice = purchaseProduct.price - purchaseProduct.commissionamt - purchaseProduct.labour;
            }

            
            this.tbPrice.Text = purchaseProduct.price.ToString("##,#.00");
            this.tbCommissionAmount.Text = purchaseProduct.commissionamt.ToString("##,#.00");
            this.tbPayablePrice.Text = purchaseProduct.payableprice.ToString("##,#.00");        
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
               
                assignProductName();
                assignSellerName();
                assignPurchaserName();


                purchaseProduct.seller_cde = m_seller_cde;
                purchaseProduct.purchaser_cde = m_purchaser_cde;
                CONTROLLER.saveProductPurchased(purchaseProduct);
                MessageBox.Show("Product Purchased Successfully!");
                generateReport();
                resetAfterSave();
            }
        }

        private void cmbSellerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSellerName_e.ItemsSource = null;
            cmbSellerName_u.ItemsSource = null;
            if(CONTROLLER.ISENGLISHVISIBLE)
                if(cmbSellerType_e.SelectedItem!=null )
                {
                    sellertype sellerType = (sellertype)cmbSellerType_e.SelectedItem;
                    m_seller_cde = sellerType.seller_cde;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller();
                        
                        lblCommission_e.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_e.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;

                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller();
                        
                        

                        lblCommission_e.Visibility = Visibility.Collapsed;
                        tbCommission.Visibility = Visibility.Collapsed;

                        lblCommissionAmount_e.Visibility = Visibility.Collapsed;
                        tbCommissionAmount.Visibility = Visibility.Collapsed;
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        
                        loadFeedmillSeller();
                        lblCommission_e.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_e.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;
                      
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSeller();
                        lblCommission_e.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_e.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;
                    }   
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbSellerType_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellerType_u.SelectedItem;
                    m_seller_cde = sellerType.seller_cde;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller();
                        
                        

                        lblCommission_u.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_u.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller();                        
                        

                        lblCommission_u.Visibility = Visibility.Collapsed;
                        tbCommission.Visibility = Visibility.Collapsed;

                        lblCommissionAmount_u.Visibility = Visibility.Collapsed;
                        tbCommissionAmount.Visibility = Visibility.Collapsed;
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        loadFeedmillSeller();
                        lblCommission_u.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_u.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSeller();
                        lblCommission_u.Visibility = Visibility.Visible;
                        tbCommission.Visibility = Visibility.Visible;

                        lblCommissionAmount_u.Visibility = Visibility.Visible;
                        tbCommissionAmount.Visibility = Visibility.Visible;
                    }   
                }
        }

        private void cmbPurchsaserType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbPruchaser_u.ItemsSource = null;
            cmbPruchaser_e.ItemsSource = null;
            if (CONTROLLER.ISENGLISHVISIBLE)
                if (cmbPruchasertype_e.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbPruchasertype_e.SelectedItem;
                    m_purchaser_cde = sellerType.seller_cde;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller1();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller1();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        loadFeedmillSeller1();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSeller1();
                    }
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbPruchasertype_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbPruchasertype_u.SelectedItem;
                    m_purchaser_cde = sellerType.seller_cde;
                    if (sellerType.seller_cde == "00001")
                    {
                        loadIndividualSeller1();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        loadCompanySeller1();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        loadFeedmillSeller1();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        loadBankSeller1();
                    }
                }
        }

        #endregion

        #region Helper Methods

        #region Bind Combos
        void bindProducts()
        {
            this.cmbProduct_e.ItemsSource = CONTROLLER.PRODUCTS;
            this.cmbProduct_u.ItemsSource = CONTROLLER.PRODUCTS;
        }
        void bindSellerType()
        {
            this.cmbSellerType_e.ItemsSource = CONTROLLER.SELLERTYPES;
            this.cmbSellerType_u.ItemsSource = CONTROLLER.SELLERTYPES;
        }

        void bindBank()
        {
            this.cmbBankName_e.ItemsSource = CONTROLLER.BANKUSERS;
            this.cmbBankName_u.ItemsSource = CONTROLLER.BANKUSERS;
        }

        void bindPurchasers()
        {

            this.cmbPruchaser_e.ItemsSource = CONTROLLER.PURCHASERUSERS;
            this.cmbPruchaser_u.ItemsSource = CONTROLLER.PURCHASERUSERS;

        }
        void bindPurchaserType()
        {
            this.cmbPruchasertype_e.ItemsSource = CONTROLLER.SELLERTYPES;
            this.cmbPruchasertype_u.ItemsSource = CONTROLLER.SELLERTYPES;
        }
        #endregion


        #region bind Users
        private void loadIndividualSeller()
        {
            SELLERS = new List<Seller>();            
            foreach(individualuser ind in CONTROLLER.INDIVIDUALUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.individualid, sellername_e = ind.name_e, sellername_u = ind.name_u});
            }
            this.cmbSellerName_e.ItemsSource = SELLERS;
            this.cmbSellerName_u.ItemsSource = SELLERS;
        }
        private void loadCompanySeller()
        {
            SELLERS = new List<Seller>();        
            foreach (companyuser cmpy in CONTROLLER.COMPANYUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = cmpy.companyid, sellername_e = cmpy.companyname_e, sellername_u = cmpy.companyname_u});
            }
            this.cmbSellerName_e.ItemsSource = SELLERS;
            this.cmbSellerName_u.ItemsSource = SELLERS;
        }
        private void loadFeedmillSeller()
        {
            SELLERS = new List<Seller>();
            foreach (feedmilluser cmpy in CONTROLLER.FEEDMILLUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = cmpy.feedmillid, sellername_e = cmpy.feemillname_e, sellername_u = cmpy.feedmillname_u });
            }
            this.cmbSellerName_e.ItemsSource = SELLERS;
            this.cmbSellerName_u.ItemsSource = SELLERS;

        }
        private void loadBankSeller()
        {
            SELLERS = new List<Seller>();
            foreach (bankuser bnk in CONTROLLER.BANKUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = bnk.bankid, sellername_e = bnk.bankname_e, sellername_u = bnk.bankname_u });
            }
            this.cmbSellerName_e.ItemsSource = SELLERS;
            this.cmbSellerName_u.ItemsSource = SELLERS;

        }





        private void loadIndividualSeller1()
        {
            SELLERS = new List<Seller>();            
            foreach(individualuser ind in CONTROLLER.INDIVIDUALUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.individualid, sellername_e = ind.name_e, sellername_u = ind.name_u});
            }
            this.cmbPruchaser_u.ItemsSource = SELLERS;
            this.cmbPruchaser_e.ItemsSource = SELLERS;
        }
        private void loadCompanySeller1()
        {
            SELLERS = new List<Seller>();        
            foreach (companyuser cmpy in CONTROLLER.COMPANYUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = cmpy.companyid, sellername_e = cmpy.companyname_e, sellername_u = cmpy.companyname_u});
            }
            this.cmbPruchaser_u.ItemsSource = SELLERS;
            this.cmbPruchaser_e.ItemsSource = SELLERS;
        }
        private void loadFeedmillSeller1()
        {
            SELLERS = new List<Seller>();
            foreach (feedmilluser cmpy in CONTROLLER.FEEDMILLUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = cmpy.feedmillid, sellername_e = cmpy.feemillname_e, sellername_u = cmpy.feedmillname_u });
            }
            this.cmbPruchaser_u.ItemsSource = SELLERS;
            this.cmbPruchaser_e.ItemsSource = SELLERS;

        }
        private void loadBankSeller1()
        {
            SELLERS = new List<Seller>();
            foreach (bankuser bnk in CONTROLLER.BANKUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = bnk.bankid, sellername_e = bnk.bankname_e, sellername_u = bnk.bankname_u });
            }
            this.cmbPruchaser_u.ItemsSource = SELLERS;
            this.cmbPruchaser_e.ItemsSource = SELLERS;

        }
        #endregion


        private bool Validate()
        {
            if(purchaseProduct.productid==0 || purchaseProduct.seller_cde==null || purchaseProduct.sellerid==0 || purchaseProduct.purchaserid==0)
            {
                MessageBox.Show("Please Fill All the Fields!");
                return false;
            }

            return true;

        }
        private void resetAfterSave()
        {
            purchaseProduct = new purchaseproduct() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            this.grdPurchaseProduct.DataContext = purchaseProduct;


        }

        private void assignSellerName()
        {
            if(CONTROLLER.ISENGLISHVISIBLE)
                if(cmbSellerName_e.SelectedItem!=null)
                {
                    Seller sell = (Seller)cmbSellerName_e.SelectedItem;
                    purchaseProduct.sellername = sell.sellername_e;
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbSellerName_u.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbSellerName_u.SelectedItem;
                    purchaseProduct.sellername = sell.sellername_u;
                }
        }
        private void assignPurchaserName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
                if (cmbPruchaser_e.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbPruchaser_e.SelectedItem;
                    purchaseProduct.purchasername = sell.sellername_e;
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbPruchaser_u.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbPruchaser_u.SelectedItem;
                    purchaseProduct.purchasername = sell.sellername_u;
                }
        }
        private void assignProductName()
        {
            if(CONTROLLER.ISENGLISHVISIBLE)
                if (cmbProduct_e.SelectedItem != null)
                {
                    product product = (product)cmbProduct_e.SelectedItem;
                    purchaseProduct.productname = product.productname_e;
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbProduct_u.SelectedItem != null)
                {
                    product product = (product)cmbProduct_u.SelectedItem;
                    purchaseProduct.productname = product.productname_u;
                }
        }
        /*private void assignPurchaserName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
                if (cmbPruchaser_e.SelectedItem != null)
                {
                    purchaseruser purchaser = (purchaseruser)cmbPruchaser_e.SelectedItem;
                    purchaseProduct.purchasername = purchaser.purchasername_e;                
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbPruchaser_u.SelectedItem != null)
                {
                    purchaseruser purchaser = (purchaseruser)cmbPruchaser_u.SelectedItem;
                    purchaseProduct.purchasername = purchaser.purchasername_u;
                }
        }
        */
        private void generateReport()
        {            
            generatePurchaserReport();
            //generateSellerReport();
        }

        private void generateSellerReport()
        {
            if(confirmSellerGenerate())
            {
                winSellerReportViewer sellerreport = new winSellerReportViewer(purchaseProduct,CONTROLLER.ISURDUVISIBLE);
                sellerreport.ShowDialog();
            }


        }
        private void generatePurchaserReport()
        {

            if(confirmPurchaserGenerate())
            {
                winPurchaserReportViewer purchaserReport = new winPurchaserReportViewer(purchaseProduct, CONTROLLER.ISURDUVISIBLE);
                purchaserReport.ShowDialog();

            }

        }

        private bool confirmSellerGenerate()
        {
            string sMessageBoxText = "Do you Generate Seller Reciept?";
            string sCaption = "Report Generation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
                return true;
            return false;


        }
        private bool confirmPurchaserGenerate()
        {
            string sMessageBoxText = "Do you Generate Purchaser Reciept?";
            string sCaption = "Report Generation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
                return true;
            return false;


        }

        #endregion

    

    }
}
