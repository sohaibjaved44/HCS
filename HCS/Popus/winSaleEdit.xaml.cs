using System;
using System.Windows;
using HCS.Controllers;

namespace HCS.Popus
{
    /// <summary>
    /// Interaction logic for winSaleEdit.xaml
    /// </summary>
    public partial class winSaleEdit : Window
    {
        #region Private Members 
        HCSController m_controller;
        saleproduct soldProduct;
        #endregion

        #region Public Properties
        
        HCSController CONTROLLER
        {
            get
            {
                return m_controller;
            }
            set
            {
                m_controller = value;
            }

        }
        
        
        #endregion

        #region Constructors and Loaders
        public winSaleEdit()
        {
            InitializeComponent();
        }
        public winSaleEdit(HCSController controller,saleproduct productSold)
        {
            InitializeComponent();

            CONTROLLER = controller;

            soldProduct = productSold;
        }

        private void winSaleEdit_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {
                CONTROLLER = new HCSController();

            }
            if(soldProduct==null )
            {
                soldProduct = new saleproduct() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };

            }

            bindProducts();
            bindFeedMillUsers();
            this.grdSaleProduct.DataContext = soldProduct;
            this.grdModel.DataContext = soldProduct;


        }

        #endregion


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                assignProductName();
                CONTROLLER.updateProductSold(soldProduct);

                MessageBox.Show("Edits Saved Successfully!");
                this.Close();
            }

        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            //btnSave.IsEnabled = true;

            soldProduct.weight = (soldProduct.noofbags * soldProduct.bagweight) + soldProduct.extraweight;
            soldProduct.price = soldProduct.rate * (soldProduct.weight / 40);
            soldProduct.recievableprice = soldProduct.price + soldProduct.labour;

            this.tbPrice.Text = soldProduct.price.ToString("##,#.00");
            this.tbRecievablePrice.Text = soldProduct.recievableprice.ToString("##,#.00");


        }


        #region Helper Methods

        private void bindProducts()
        {
            this.cmbProduct.ItemsSource = CONTROLLER.PRODUCTS;

        }
        private void bindFeedMillUsers()
        {
            this.cmbFeedMill.ItemsSource = CONTROLLER.FEEDMILLUSERS;


        }

        private void assignProductName()
        {
            if (cmbProduct.SelectedItem != null)
            {
                product product = (product)cmbProduct.SelectedItem;
                soldProduct.productname = product.productname_e;

            }


        }
        private bool Validate()
        {
            if (soldProduct.productid == 0 || soldProduct.feedmillid == 0 || soldProduct.drivernumber == null || soldProduct.vehicleno == null)
            {
                MessageBox.Show("Please Fill All the Fields!");
                return false;

            }

            return true;

        }
        #endregion

    }
}
