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
    /// Interaction logic for ucFeedMillUser.xaml
    /// </summary>
    public partial class ucFeedMillUser : UserControl
    {
        #region Private Members

        feedmilluser FeedMillUser;
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
        public ucFeedMillUser()
        {
            InitializeComponent();
        }

        public ucFeedMillUser(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }


        private void ucFeedMillUser_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {
                CONTROLLER = new HCSController();

            }

            FeedMillUser = new feedmilluser() { date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE }; 
            this.grdFeedMillDetail.DataContext = FeedMillUser;
            this.grdUploadImage.DataContext = FeedMillUser;
            this.grdModel.DataContext = FeedMillUser;

            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgExistingFeedMill.Columns[1].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[3].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[5].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[7].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[9].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[11].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[13].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[15].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgExistingFeedMill.Columns[0].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[2].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[4].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[6].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[8].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[10].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[12].Visibility = Visibility.Collapsed;
                dgExistingFeedMill.Columns[14].Visibility = Visibility.Collapsed;
            }






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
                FeedMillUser.imagepath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                feedmillImageUpload.Source = imageSource;
                FeedMillUser.imagetobyte = File.ReadAllBytes(FeedMillUser.imagepath);
            }

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(Validate())
            {
                if(FeedMillUser.feedmillid>0)
                {
                    CONTROLLER.updateFeedMillUser(FeedMillUser);
                }
                else
                { 
                CONTROLLER.saveFeedMillUser(FeedMillUser);
                }
                MessageBox.Show("Feed Mill User Saved Successfully!");
                RefreshUsers();
                resetAfterSave();
            }

        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            feedmilluser obj = ((FrameworkElement)sender).DataContext as feedmilluser;
            obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
            FeedMillUser = obj;
            loadImage();
            this.grdFeedMillDetail.DataContext = FeedMillUser;
        }
        #endregion

     

        #region Helper Methods
       
        private bool Validate()
        {
            if ((string.IsNullOrEmpty(FeedMillUser.feemillname_e) && string.IsNullOrEmpty(FeedMillUser.feedmillname_u)) ||
                string.IsNullOrEmpty(FeedMillUser.phone) || (string.IsNullOrEmpty(FeedMillUser.address_e) && string.IsNullOrEmpty(FeedMillUser.address_u)))
            {
                MessageBox.Show("Please Fill all the mandatory fields!");
                return false;

            }
            return true;
        }
        
        private void resetAfterSave()
        {

            FeedMillUser = new feedmilluser() { date = DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            feedmillImageUpload.Source = new BitmapImage();
            this.grdFeedMillDetail.DataContext = FeedMillUser;

        }

        private void loadImage()
        {
            if (FeedMillUser != null && FeedMillUser.imagetobyte != null)
            {

                Stream StreamObj = new MemoryStream(FeedMillUser.imagetobyte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                 feedmillImageUpload.Source = BitObj;
            }

            else
            {
                feedmillImageUpload.Source = new BitmapImage();

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
            ObservableCollection<feedmilluser> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgExistingFeedMill.ItemsSource=data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<feedmilluser> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = CONTROLLER.FEEDMILLUSERS.Count;
            ObservableCollection<feedmilluser> filteredProducts = new ObservableCollection<feedmilluser>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                feedmilluser user = CONTROLLER.FEEDMILLUSERS[i];
                filteredProducts.Add(user);
            }
            return filteredProducts;
        }
        #endregion

        


    }
}
