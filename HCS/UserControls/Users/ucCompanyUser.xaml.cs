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
    /// Interaction logic for ucCompanyUser.xaml
    /// </summary>
    public partial class ucCompanyUser : UserControl
    {
        #region Private Members
        private companyuser CompanyUser;
        private  HCSController m_controller;
        private int start = 0;
        private int itemCount = 7;
        private int totalItems = 0;
  
        #endregion

        #region Public Properties

        public HCSController CONTROLLER
        {
            get { return m_controller; }
            set { m_controller = value; }
        }

        #endregion

        #region Constructors and Loaders

        public ucCompanyUser()
        {
            InitializeComponent();
        }

        public ucCompanyUser(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }


        private void ucCompanyUser_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {

                CONTROLLER = new HCSController();

            }
            
            CompanyUser = new companyuser() { date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE }; 
            grdCompanyDetail.DataContext = CompanyUser;

            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgExistingCompany.Columns[1].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[3].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[5].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[7].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[9].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[11].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[13].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[15].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgExistingCompany.Columns[0].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[2].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[4].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[6].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[8].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[10].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[12].Visibility = Visibility.Collapsed;
                dgExistingCompany.Columns[14].Visibility = Visibility.Collapsed;
            }

            this.grdUploadImage.DataContext = CompanyUser;
            this.grdModel.DataContext = CompanyUser;
            RefreshUsers();
        }

        #endregion

        #region Even Handlers
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.DefaultExt = ".jpeg";


            if (!string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                CompanyUser.imagepath= openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                companyImgUpload.Source = imageSource;
                CompanyUser.imagetobyte = File.ReadAllBytes(CompanyUser.imagepath);
            }


        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                if (CompanyUser.companyid > 0)
                {
                    CONTROLLER.updateCompanyUser(CompanyUser);
                }
                else
                {

                    CONTROLLER.saveCompanyUser(CompanyUser);
                }
                MessageBox.Show("Company User saved successfully!");
                RefreshUsers();
                resetAfterSave();
            }

        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {

            companyuser obj = ((FrameworkElement)sender).DataContext as companyuser;
            CompanyUser = obj;
            loadImage();
            this.grdCompanyDetail.DataContext = CompanyUser;
        }


        #endregion
        
        #region Helper Methods

        private bool Validate()
        {
            if ((string.IsNullOrEmpty(CompanyUser.companyname_e) && string.IsNullOrEmpty(CompanyUser.companyname_u)) ||
                string.IsNullOrEmpty(CompanyUser.phone) || (string.IsNullOrEmpty(CompanyUser.address_e) && string.IsNullOrEmpty(CompanyUser.address_u)))            
            {
                MessageBox.Show("Please Fill All the mandatory fields");
                return false;

            }
            return true;

        }

        private void resetAfterSave()
        {

            CompanyUser = new companyuser() { date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            companyImgUpload.Source = new BitmapImage();
            this.grdCompanyDetail.DataContext = CompanyUser;


        }


        private void loadImage()
        {
            if (CompanyUser != null &&  CompanyUser.imagetobyte!=null)
            {

                Stream StreamObj = new MemoryStream(CompanyUser.imagetobyte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
               companyImgUpload.Source = BitObj;
            }

            else
            {
                companyImgUpload.Source = new BitmapImage();

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
            if ((start-itemCount) > -1)
            { 
                start -= itemCount;
                RefreshUsers();
            }
        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            if ((start+itemCount) < totalItems)
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
            ObservableCollection<companyuser> data = GetPartialUsers(start, itemCount, out totalItems);
            dgExistingCompany.ItemsSource = data;
            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<companyuser> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = CONTROLLER.COMPANYUSERS.Count;
            ObservableCollection<companyuser> filteredProducts = new ObservableCollection<companyuser>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                CONTROLLER.COMPANYUSERS[i].isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                CONTROLLER.COMPANYUSERS[i].isurduvisible = CONTROLLER.ISURDUVISIBLE;
                companyuser user = CONTROLLER.COMPANYUSERS[i];
                filteredProducts.Add(user);
            }
            return filteredProducts;
        }
        #endregion

        
    }
}
