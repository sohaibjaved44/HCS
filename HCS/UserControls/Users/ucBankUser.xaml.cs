using HCS.Controllers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCS.UserControls.Users
{
    /// <summary>
    /// Interaction logic for ucBankUser.xaml
    /// </summary>
    public partial class ucBankUser : UserControl
    {

        #region private members

        private HCSController m_controller ;
        private bankuser m_bankuser;
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
        public bool isenglishvisible { get; set; }
        public bool isurduvisible { get; set; }
        #endregion

        public ucBankUser()
        {
            InitializeComponent();
        }

        public ucBankUser(HCSController controller)
        {
            InitializeComponent();
            CONTROLLER = controller;
        }

        private void ucBankUser_Loaded(object sender, RoutedEventArgs e)
        {
            if (CONTROLLER == null)
            {

                CONTROLLER = new HCSController();

            }

            m_bankuser = new bankuser(){ date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE};
            grdbankDetail.DataContext = m_bankuser;

            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgExistingbank.Columns[1].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[3].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[5].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[7].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[9].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[11].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[13].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[15].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgExistingbank.Columns[0].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[2].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[4].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[6].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[8].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[10].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[12].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[14].Visibility = Visibility.Collapsed;
                dgExistingbank.Columns[16].Visibility = Visibility.Collapsed;
            }

            this.grdUploadImage.DataContext = m_bankuser;
            this.grdModel.DataContext = m_bankuser;
            RefreshUsers();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.DefaultExt = ".jpeg";


            if (!string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                m_bankuser.imagepath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                bankImgUpload.Source = imageSource;
                m_bankuser.imagetobyte = File.ReadAllBytes(m_bankuser.imagepath);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (m_bankuser.bankid > 0)
                {
                    CONTROLLER.updateBankUser(m_bankuser);
                }
                else
                {

                    CONTROLLER.saveBankUser(m_bankuser);
                }
                MessageBox.Show("Company User saved successfully!");
                RefreshUsers();
                resetAfterSave();
            }

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

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            bankuser obj = ((FrameworkElement)sender).DataContext as bankuser;
            m_bankuser = obj;
            loadImage();
            this.grdbankDetail.DataContext = m_bankuser;
        }





        private void RefreshUsers()
        {
            ObservableCollection<bankuser> data = GetPartialUsers(start, itemCount, out totalItems);
            dgExistingbank.ItemsSource = data;
            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }
        private ObservableCollection<bankuser> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = CONTROLLER.BANKUSERS.Count;
            ObservableCollection<bankuser> filteredProducts = new ObservableCollection<bankuser>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                CONTROLLER.BANKUSERS[i].isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                CONTROLLER.BANKUSERS[i].isurduvisible = CONTROLLER.ISURDUVISIBLE;
                bankuser user = CONTROLLER.BANKUSERS[i];
                filteredProducts.Add(user);
            }
            return filteredProducts;
        }

        private bool Validate()
        {
            if ((string.IsNullOrEmpty(m_bankuser.bankname_e) && string.IsNullOrEmpty(m_bankuser.bankname_u)) ||
                string.IsNullOrEmpty(m_bankuser.phone) || (string.IsNullOrEmpty(m_bankuser.address_e) && string.IsNullOrEmpty(m_bankuser.address_u)))
            {
                MessageBox.Show("Please Fill All the mandatory fields");
                return false;

            }
            return true;

        }

        private void resetAfterSave()
        {

            m_bankuser = new bankuser() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            bankImgUpload.Source = new BitmapImage();
            this.grdbankDetail.DataContext = m_bankuser;


        }

        private void loadImage()
        {
            if (m_bankuser != null && m_bankuser.imagetobyte != null)
            {

                Stream StreamObj = new MemoryStream(m_bankuser.imagetobyte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                bankImgUpload.Source = BitObj;
            }

            else
            {
                bankImgUpload.Source = new BitmapImage();

            }

        }
        
    }
}
