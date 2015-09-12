using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HCS.Controllers;
using HCS.reportwindows;

namespace HCS.UserControls.Sale
{
    /// <summary>
    /// Interaction logic for ucSaleProduct.xaml
    /// </summary>
    public partial class ucSaleProduct : UserControl
    {
        #region Private Members

        private saleproduct saleProduct;
        private HCSController m_controller;
        
        List<Seller> m_sellers;
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
        public ucSaleProduct()
        {
            InitializeComponent();
        }

        public ucSaleProduct(HCSController controller)
        {
            InitializeComponent();
            CONTROLLER = controller;
        }



        private void ucSaleProduct_Loaded(object sender, RoutedEventArgs e)
        {
            if (CONTROLLER == null)
            {
                CONTROLLER = new HCSController();
            }
            saleProduct = new saleproduct() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            bindProducts();
            bindFeedMillUsers();
            bindPurchaserTypes();
            bindSalType();
            this.grdSaleProduct.DataContext = saleProduct;
            this.grdModel.DataContext = saleProduct;
        }




        #endregion

        #region Event Handlers
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (saleProduct.productid != null && saleProduct.purchasertypecde != null && saleProduct.accountcharges != null && saleProduct.bardana != null)
            {
                saletype saletyp = new saletype();
                btnSave.IsEnabled = true;
                btnSaveUrdu.IsEnabled = true;
                if (cmbSaletype_e.SelectedItem != null)
                {
                    saletyp = (saletype)cmbSaletype_e.SelectedItem;
                }
                else if (cmbSaletype_e.SelectedItem != null)
                {
                    saletyp = (saletype)cmbSaletype_e.SelectedItem;
                }
                else
                {
                    MessageBox.Show("Select sale type first");
                }
                if (saletyp.saletypecde == "00001")//commission base
                {
                    saleProduct.thread = saleProduct.noofbags;
                    saleProduct.weight = (saleProduct.noofbags * saleProduct.bagweight) + saleProduct.extraweight;
                    saleProduct.fee = 1 * saleProduct.weight / 100;
                    tbFee.Text = saleProduct.fee.ToString();

                    saleProduct.price = saleProduct.rate * (saleProduct.weight / 40);
                    saleProduct.commissionamount = saleProduct.commissionpercent / 100 * saleProduct.price;
                    saleProduct.labour = saleProduct.labour*saleProduct.noofbags;
                    saleProduct.recievableprice = (saleProduct.price + saleProduct.labour + saleProduct.fee + saleProduct.thread + saleProduct.accountcharges + saleProduct.bardana);
                    this.tbCommissionAmount.Text = saleProduct.commissionamount.ToString();
                    this.tbRecievablePrice.Text = saleProduct.price.ToString("##,#.00");
                    this.tbPrice.Text = saleProduct.recievableprice.ToString("##,#.00");
                    
                    tbThread.Text = tbnoofbags.Text;
                }
                if (saletyp.saletypecde == "00002")//lump sum
                {
                    saleProduct.thread = saleProduct.noofbags;
                    saleProduct.weight = (saleProduct.noofbags * saleProduct.bagweight) + saleProduct.extraweight;
                    saleProduct.fee = 1 * saleProduct.weight / 100;
                    tbFee.Text = saleProduct.fee.ToString();

                    saleProduct.price = saleProduct.rate * (saleProduct.weight / 40);
                    saleProduct.commissionamount = saleProduct.commissionpercent / 100 * saleProduct.price;
                    saleProduct.recievableprice = (saleProduct.price + saleProduct.labour + saleProduct.fee + saleProduct.thread + saleProduct.accountcharges + saleProduct.bardana);
                    this.tbCommissionAmount.Text = saleProduct.commissionamount.ToString();
                    this.tbPrice.Text = saleProduct.price.ToString("##,#.00");
                    this.tbRecievablePrice.Text = saleProduct.recievableprice.ToString("##,#.00");
                    
                    tbThread.Text = tbnoofbags.Text;
                }
            }
            else
            {
                MessageBox.Show("Please select all fields!");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (saleProduct.productid != null && saleProduct.purchasertypecde != null && saleProduct.accountcharges != null && saleProduct.bardana != null)
            {
                assignProductName();
                assignPurchaserName();
                assignSellerName();
                //assignFeedMillName();
                CONTROLLER.saveProductSold(saleProduct);
                MessageBox.Show("Product Sold Saved Successfully!");
                
                //generateReport();
                resetAftersave();

            }
        }
        #endregion

        #region Helper Methods

        private void bindProducts()
        {
            this.cmbProduct_e.ItemsSource = CONTROLLER.PRODUCTS.FindAll(p => p.producttype != ProductType.Cash.GetStringValue());
            this.cmbProduct_u.ItemsSource = CONTROLLER.PRODUCTS.FindAll(p => p.producttype != ProductType.Cash.GetStringValue());
        }
        private void bindFeedMillUsers()
        {
            this.cmbPurchasername_e.ItemsSource = CONTROLLER.FEEDMILLUSERS;
            this.cmbPurchasername_u.ItemsSource = CONTROLLER.FEEDMILLUSERS;


        }

        private bool Validate()
        {
            if (saleProduct.productid == 0 || saleProduct.purchasername != null || saleProduct.drivernumber == null || saleProduct.vehicleno == null)
            {
                MessageBox.Show("Please Fill All the Fields!");
                return false;

            }

            return true;

        }
        private void resetAftersave()
        {
            saleProduct = new saleproduct() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };

            this.grdSaleProduct.DataContext = saleProduct;

        }

        private void assignProductName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbProduct_e.SelectedItem != null)
                {
                    product product = (product)cmbProduct_e.SelectedItem;
                    saleProduct.productname = product.productname_e;
                    saleProduct.selectedProduct = product;

                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbProduct_e.SelectedItem != null)
                {
                    product product = (product)cmbProduct_u.SelectedItem;
                    saleProduct.productname = product.productname_u;
                    saleProduct.selectedProduct = product;

                }
            }
        }

        private void assignSellerName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
                if (cmbSellername_e.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbSellername_e.SelectedItem;
                    saleProduct.sellername = sell.sellername_e;
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbSellername_u.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbSellername_u.SelectedItem;
                    saleProduct.sellername = sell.sellername_u;
                }
        }
        private void assignPurchaserName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
                if (cmbPurchasername_e.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbPurchasername_e.SelectedItem;
                    saleProduct.purchasername = sell.sellername_e;
                }
            if (CONTROLLER.ISURDUVISIBLE)
                if (cmbPurchasername_u.SelectedItem != null)
                {
                    Seller sell = (Seller)cmbPurchasername_u.SelectedItem;
                    saleProduct.purchasername = sell.sellername_u;
                }
        }

        /*
        private void assignFeedMillName()
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbPurchasername_e.SelectedItem != null)
                {
                    feedmilluser feedMill = (feedmilluser)cmbPurchasername_e.SelectedItem;
                    saleProduct.purchasername = feedMill.feemillname_e;
                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbPurchasername_u.SelectedItem != null)
                {
                    feedmilluser feedMill = (feedmilluser)cmbPurchasername_u.SelectedItem;
                    saleProduct.purchasername = feedMill.feemillname_e;
                }
            }

        }
        */
        private void generateReport()
        {
            if (confirmGenerate())
            {
                winFeedMillReportViewer reportviewer = new winFeedMillReportViewer(saleProduct,CONTROLLER.ISURDUVISIBLE);
                reportviewer.ShowDialog();
            }
        }

        private bool confirmGenerate()
        {
            string sMessageBoxText = "Do you Generate Feed Mill Reciept?";
            string sCaption = "Report Generation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
                return true;
            return false;
        }

        #endregion

        public bool controller { get; set; }

        private void CmbPurchasertype_u_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbPurchasertype_e.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbPurchasertype_e.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        bindIndividual();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        bindCompany();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        bindFeedMill();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        bindBank();
                    }
                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbPurchasertype_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbPurchasertype_u.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        bindIndividual();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        bindCompany();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        bindFeedMill();
                    }

                    else if (sellerType.seller_cde == "00004")
                    {
                        bindBank();
                    }
                }
            }
        }

        private void CmbSellertype_u_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbSellertype_e.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellertype_e.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        bindIndividualforSeller();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        bindCompanyforSeller();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        bindFeedMillforSeller();
                    }
                    else if (sellerType.seller_cde == "00004")
                    {
                        bindBankforSeller();
                    }
                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbSellertype_u.SelectedItem != null)
                {
                    sellertype sellerType = (sellertype)cmbSellertype_u.SelectedItem;
                    if (sellerType.seller_cde == "00001")
                    {
                        bindIndividualforSeller();
                    }
                    else if (sellerType.seller_cde == "00002")
                    {
                        bindCompanyforSeller();
                    }
                    else if (sellerType.seller_cde == "00003")
                    {
                        bindFeedMillforSeller();
                    }

                    else if (sellerType.seller_cde == "00004")
                    {
                        bindBankforSeller();
                    }
                }
            }
        }

        private void bindIndividual()
        {
            SELLERS = new List<Seller>();


            foreach (individualuser ind in CONTROLLER.INDIVIDUALUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.individualid, sellername_e = ind.name_e, sellername_u = ind.name_u });
            }
            cmbPurchasername_e.ItemsSource = SELLERS;
            cmbPurchasername_u.ItemsSource = SELLERS;
        }
        private void bindCompany()
        {
            SELLERS = new List<Seller>();


            foreach (companyuser ind in CONTROLLER.COMPANYUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.companyid, sellername_e = ind.companyname_e, sellername_u = ind.companyname_u });
            }
            cmbPurchasername_e.ItemsSource = SELLERS;
            cmbPurchasername_u.ItemsSource = SELLERS;
        }
        private void bindFeedMill()
        {
            SELLERS = new List<Seller>();


            foreach (feedmilluser ind in CONTROLLER.FEEDMILLUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.feedmillid, sellername_e = ind.feemillname_e, sellername_u = ind.feedmillname_u });
            }
            cmbPurchasername_e.ItemsSource = SELLERS;
            cmbPurchasername_u.ItemsSource = SELLERS;
        }
        private void bindBank()
        {
            SELLERS = new List<Seller>();


            foreach (bankuser ind in CONTROLLER.BANKUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.bankid, sellername_e = ind.bankname_e, sellername_u = ind.bankname_u });
            }
            cmbPurchasername_e.ItemsSource = SELLERS;
            cmbPurchasername_u.ItemsSource = SELLERS;
        }

        private void bindIndividualforSeller()
        {
            SELLERS = new List<Seller>();


            foreach (individualuser ind in CONTROLLER.INDIVIDUALUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.individualid, sellername_e = ind.name_e, sellername_u = ind.name_u });
            }
            cmbSellername_e.ItemsSource = SELLERS;
            cmbSellername_u.ItemsSource = SELLERS;
        }
        private void bindCompanyforSeller()
        {
            SELLERS = new List<Seller>();


            foreach (companyuser ind in CONTROLLER.COMPANYUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.companyid, sellername_e = ind.companyname_e, sellername_u = ind.companyname_u });
            }
            cmbSellername_e.ItemsSource = SELLERS;
            cmbSellername_u.ItemsSource = SELLERS;
        }
        private void bindFeedMillforSeller()
        {
            SELLERS = new List<Seller>();


            foreach (feedmilluser ind in CONTROLLER.FEEDMILLUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.feedmillid, sellername_e = ind.feemillname_e, sellername_u = ind.feedmillname_u });
            }
            cmbSellername_e.ItemsSource = SELLERS;
            cmbSellername_u.ItemsSource = SELLERS;
        }
        private void bindBankforSeller()
        {
            SELLERS = new List<Seller>();


            foreach (bankuser ind in CONTROLLER.BANKUSERS)
            {
                SELLERS.Add(new Seller() { sellerid = ind.bankid, sellername_e = ind.bankname_e, sellername_u = ind.bankname_u });
            }
            cmbSellername_e.ItemsSource = SELLERS;
            cmbSellername_u.ItemsSource = SELLERS;
        }

        private void bindPurchaserTypes()
        {
            cmbPurchasertype_e.ItemsSource = CONTROLLER.SELLERTYPES;
            cmbPurchasertype_u.ItemsSource = CONTROLLER.SELLERTYPES;

            cmbSellertype_e.ItemsSource = CONTROLLER.SELLERTYPES;
            cmbSellertype_u.ItemsSource = CONTROLLER.SELLERTYPES;
        }

        private void bindSalType()
        {
            cmbSaletype_e.ItemsSource = CONTROLLER.SALETYPE;
            cmbSaletype_u.ItemsSource = CONTROLLER.SALETYPE;
        }

    }
}
