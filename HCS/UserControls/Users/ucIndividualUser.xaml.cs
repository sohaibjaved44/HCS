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
    /// Interaction logic for ucIndividualUser.xaml
    /// </summary>
    public partial class ucIndividualUser : UserControl
    {
        #region Private Members
        individualuser individualUser;
        HCSController m_controller;
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

        public ucIndividualUser()
        {
            InitializeComponent();
        }

        public ucIndividualUser(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }



        private void ucIndividualUser_Loaded(object sender, RoutedEventArgs e)
        {
            if(CONTROLLER==null)
            {

                CONTROLLER = new HCSController();


            }





            individualUser = new individualuser() { date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            grdIndividualDetail.DataContext = individualUser;
            this.grdUploadImage.DataContext = individualUser;
            this.grdModel.DataContext = individualUser;

            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgExistingIndividual.Columns[1].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[3].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[5].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[7].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[9].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[11].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[13].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[15].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[17].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgExistingIndividual.Columns[0].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[2].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[4].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[6].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[8].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[10].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[12].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[14].Visibility = Visibility.Collapsed;
                dgExistingIndividual.Columns[16].Visibility = Visibility.Collapsed;

            }




            // this.dgExistingIndividual.ItemsSource = CONTROLLER.INDIVIDUALUSERS;
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
                    individualUser.imagepath = openFileDialog1.FileName;
                    ImageSource imageSource = new BitmapImage(new Uri(openFileDialog1.FileName));
                    imgUpload.Source = imageSource;
                    individualUser.imagetobyte = File.ReadAllBytes(individualUser.imagepath);
                }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (individualUser.individualid > 0)
                {
                    CONTROLLER.updateIndividualUser(individualUser);
                }
                else
                {
                    CONTROLLER.saveIndividualUser(individualUser);
                }
                    //this.dgExistingIndividual.ItemsSource = new ObservableCollection<individualuser>(CONTROLLER.INDIVIDUALUSERS); 
                
                MessageBox.Show("User Saved Successfully !");
                RefreshUsers();
                resetAfterSave();
            
            }
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
        {
            individualuser obj = ((FrameworkElement)sender).DataContext as individualuser;
            obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
            individualUser = obj;
            loadImage();
            this.grdIndividualDetail.DataContext = individualUser;
          
        
        }


        #endregion

        #region Helper Methods
        
        private bool Validate()
        {

            if ((string.IsNullOrEmpty(individualUser.name_u) && string.IsNullOrEmpty(individualUser.name_e))||
                string.IsNullOrEmpty(individualUser.mobile) || (string.IsNullOrEmpty(individualUser.address_e) && string.IsNullOrEmpty(individualUser.address_u)))
            {
                MessageBox.Show("Please Fill All the Mandatory Fields");
                return false;
            }
            
                return true;
        
        }
        private void resetAfterSave()
        {

            individualUser = new individualuser() { date=DateTime.Now, isenglishvisible = CONTROLLER.ISENGLISHVISIBLE, isurduvisible = CONTROLLER.ISURDUVISIBLE };
            imgUpload.Source = new BitmapImage();
            this.grdIndividualDetail.DataContext = individualUser;


        }

        private void loadImage()
        {
            if (individualUser != null && individualUser.imagetobyte != null)
            {

                Stream StreamObj = new MemoryStream(individualUser.imagetobyte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                imgUpload.Source = BitObj;
            }

            else
            {
                imgUpload.Source = new BitmapImage();

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
            ObservableCollection<individualuser> data = GetPartialUsers(start, itemCount, out totalItems);
            dgExistingIndividual.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private ObservableCollection<individualuser> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = CONTROLLER.INDIVIDUALUSERS.Count;
            ObservableCollection<individualuser> filteredProducts = new ObservableCollection<individualuser>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                individualuser user = CONTROLLER.INDIVIDUALUSERS[i];
                filteredProducts.Add(user);
            }
            return filteredProducts;
        }
        #endregion
    }
}
