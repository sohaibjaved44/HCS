using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HCS.Controllers;
using Microsoft.Win32;

namespace HCS.UserControls
{
    /// <summary>
    /// Interaction logic for ucPurchaserUser.xaml
    /// </summary>
    public partial class ucPurchaserUser : UserControl
    {
        #region Private Members 
        
        purchaseruser Purchaser;
        HCSController m_contoller;

        private int start = 0;
        private int itemCount = 7;
        private int totalItems = 0;

        #endregion

        #region Public Properties
        HCSController CONTROLLER
        {
            get { return m_contoller; }
            set { m_contoller = value; }
        }


        #endregion


        #region Constructors and Loaders
        public ucPurchaserUser()
        {
            InitializeComponent();
        }

        public ucPurchaserUser(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }


        private void ucPurchaserUser_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {

                CONTROLLER = new HCSController();

            }
            Purchaser = new purchaseruser() {date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            this.grdPurchaserDetail.DataContext = Purchaser;
            this.grdUploadImage.DataContext = Purchaser;
            this.grdModel.DataContext = Purchaser;
            RefreshUsers();
        }

        #endregion

        #region Event Handlers
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.DefaultExt = ".jpeg";


            if (!string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                Purchaser.imagepath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                purchaserImageUpload.Source = imageSource;
                Purchaser.imagetobyte = File.ReadAllBytes(Purchaser.imagepath);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                if (Purchaser.purchaserid > 0)
                {
                    CONTROLLER.updatePurchaserUser(Purchaser);

                }
                else
                {
                    CONTROLLER.savePurchaserUser(Purchaser);
                }
                 MessageBox.Show("Purchase User Saved Successfully!");
                RefreshUsers();
                    
                resetAfterSave();

            }

        }


        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            purchaseruser obj = ((FrameworkElement)sender).DataContext as purchaseruser;
            obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
      
            Purchaser = obj;
            loadImage();
            this.grdPurchaserDetail.DataContext = Purchaser;

        }


        #endregion

        #region Helper Methods
       
        private bool Validate()
        {
            if (string.IsNullOrEmpty(Purchaser.purchasername_e) ||
                string.IsNullOrEmpty(Purchaser.mobile) ||
                string.IsNullOrEmpty(Purchaser.address_e))
            {
                MessageBox.Show("Please Fill All the Mandatory Fields!");
                return false;
            }

            return true;
        }

        private void resetAfterSave()
        {
            Purchaser = new purchaseruser() { date=DateTime.Now ,isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            purchaserImageUpload.Source = new BitmapImage();
            this.grdPurchaserDetail.DataContext = Purchaser;


        }

        private void loadImage()
        {
            if (Purchaser != null && Purchaser.imagetobyte != null)
            {

                Stream StreamObj = new MemoryStream(Purchaser.imagetobyte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                purchaserImageUpload.Source = BitObj;
            }

            else
            {

                purchaserImageUpload.Source = new BitmapImage();

            }

        }
       
        #endregion


        #region Paging Functions

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


        private void RefreshUsers()
        {
            ObservableCollection<purchaseruser> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgExistingPurchaser.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<purchaseruser> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = CONTROLLER.PURCHASERUSERS.Count;
            ObservableCollection<purchaseruser> filteredProducts = new ObservableCollection<purchaseruser>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                purchaseruser user = CONTROLLER.PURCHASERUSERS[i];
                filteredProducts.Add(user);
            }
            return filteredProducts;
        }
        #endregion

        

        
    }
}
