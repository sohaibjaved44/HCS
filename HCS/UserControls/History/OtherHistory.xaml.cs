using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using HCS.Controllers;

namespace HCS.UserControls.History
{
    /// <summary>
    /// Interaction logic for OtherHistory.xaml
    /// </summary>
    public partial class OtherHistory : UserControl
    {
        #region Private Members

        private HCSController m_controller;
        private ObservableCollection<OtherKhata> m_History;
        private List<Seller> m_sellers;
        private int start = 0;
        private int itemCount = 7;
        private int totalItems = 0;
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

        public ObservableCollection<OtherKhata> HISTORY
        {
            get { return m_History; }
            set { m_History = value; }
        }

        private List<Seller> SELLERS
        {
            get { return m_sellers; }

            set { m_sellers = value; }
        }

        #endregion

        public OtherHistory()
        {
            InitializeComponent();
        }

        public OtherHistory(HCSController p_controller)
        {
            InitializeComponent();
            m_controller = p_controller;
        }

        private void ucOtherHistory_Loaded(object sender, RoutedEventArgs e)
        {
            HISTORY = new ObservableCollection<OtherKhata>();
            if (CONTROLLER == null)
            {
                CONTROLLER = new HCSController();
            }
            this.grdModel.DataContext = CONTROLLER;
            this.grdOtherHistory.DataContext = CONTROLLER;


            dgOtherHistory.ItemsSource = HISTORY;
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                dgOtherHistory.Columns[1].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[3].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[5].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[7].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[9].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[11].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[13].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[15].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[17].Visibility = Visibility.Collapsed;
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                dgOtherHistory.Columns[0].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[2].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[4].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[6].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[8].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[10].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[12].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[14].Visibility = Visibility.Collapsed;
                dgOtherHistory.Columns[16].Visibility = Visibility.Collapsed;
            }

            RefreshUsers();
            bindSellerTypes();
        }

        #region Helper Methods

        private void RefreshUsers()
        {
            ObservableCollection<OtherKhata> data = GetPartialUsers(start, itemCount, out totalItems);
            this.dgOtherHistory.ItemsSource = data;

            StartFrom.Text = (start + 1).ToString();
            EndFrom.Text = (start + data.Count).ToString();
            TotalItems.Text = totalItems.ToString();
        }

        private void bindSellerTypes()
        {
            this.cmbKhataType_e.ItemsSource = CONTROLLER.KHATATYPECODE;
            this.cmbKhataType_u.ItemsSource = CONTROLLER.KHATATYPECODE;
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            HISTORY = new ObservableCollection<OtherKhata>();
            string khatatypecode;
            if (CONTROLLER.ISENGLISHVISIBLE)
            {
                if (cmbKhataType_e.SelectedItem != null)
                {
                    KhataTypeCode khatatype = (KhataTypeCode) cmbKhataType_e.SelectedItem;

                    khatatypecode = khatatype.khatatypecde;
                    //filling the collection as per search criteria
                    using (var db = new HCSMLEntities1())
                    {
                        var qry = from k in db.OtherKhatas where k.khatatypecde == khatatypecode select k;
                        foreach (var item in (qry))
                        {
                            OtherKhata kh = new OtherKhata();
                            kh.date = item.date;
                            kh.activitycode = item.activitycode;
                            kh.id = item.id;
                            kh.payable_naam = item.payable_naam;
                            kh.productname = item.productname;
                            kh.purchaserid = item.purchaserid;
                            kh.purchasertypecode = item.purchasertypecode;
                            kh.receivable_jama = item.receivable_jama;
                            kh.sellerid = item.sellerid;
                            kh.sellertypecode = item.sellertypecode;
                            kh.sellername = getName(kh.sellertypecode, kh.sellerid);
                            kh.purchasername = getName(kh.purchasertypecode, kh.purchaserid);
                            kh.totalpayable_naam = item.totalpayable_naam;
                            kh.totalreceivable_jama = item.totalreceivable_jama;
                            HISTORY.Add(kh);
                        }
                    }
                    RefreshUsers();
                    //setTotalRecievable();
                }
            }
            if (CONTROLLER.ISURDUVISIBLE)
            {
                if (cmbKhataType_u.SelectedItem != null)
                {
                    KhataTypeCode khatatype = (KhataTypeCode) cmbKhataType_u.SelectedItem;

                    khatatypecode = khatatype.khatatypecde;
                    //filling the collection as per search criteria
                    using (var db = new HCSMLEntities1())
                    {
                        var qry = from k in db.OtherKhatas where k.khatatypecde == khatatypecode select k;
                        foreach (var item in (qry))
                        {
                            OtherKhata kh = new OtherKhata();
                            kh.date = item.date;
                            kh.activitycode = item.activitycode;
                            kh.id = item.id;
                            kh.payable_naam = item.payable_naam;
                            kh.productname = item.productname;
                            kh.purchaserid = item.purchaserid;
                            kh.purchasertypecode = item.purchasertypecode;
                            kh.receivable_jama = item.receivable_jama;
                            kh.sellerid = item.sellerid;
                            kh.sellertypecode = item.sellertypecode;
                            kh.sellername = getName(kh.sellertypecode, kh.sellerid);
                            kh.purchasername = getName(kh.purchasertypecode, kh.purchaserid);
                            kh.totalpayable_naam = item.totalpayable_naam;
                            kh.totalreceivable_jama = item.totalreceivable_jama;
                            HISTORY.Add(kh);
                        }
                    }
                    RefreshUsers();
                    //setTotalRecievable();
                }
            }
        }

        #region commented

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

        #endregion

        private ObservableCollection<OtherKhata> GetPartialUsers(int start, int itemCount, out int totalItems)
        {
            totalItems = HISTORY.Count;
            ObservableCollection<OtherKhata> filteredProducts = new ObservableCollection<OtherKhata>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                OtherKhata product = HISTORY[i];
                filteredProducts.Add(product);
            }
            return filteredProducts;
        }

        #region Pagination

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
            start = (totalItems/itemCount - 1)*itemCount;
            start += totalItems%itemCount == 0 ? 0 : itemCount;
            RefreshUsers();
        }

        #endregion

        private void showDetail_Click(object sender, RoutedEventArgs e)
        {
            ////HCS.History hstry = ((FrameworkElement)sender).DataContext as HCS.History;
            //HCS.History hstry = (HCS.History) dgOtherHistory.SelectedItem;
            //hstry.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
            //hstry.isurduvisible = CONTROLLER.ISURDUVISIBLE;
            //DetailedHistory dh = new DetailedHistory(CONTROLLER,hstry);
            //dh.Show();

            /*
                           individualuser obj = ((FrameworkElement)sender).DataContext as individualuser;
                           obj.isenglishvisible = CONTROLLER.ISENGLISHVISIBLE;
                           obj.isurduvisible = CONTROLLER.ISURDUVISIBLE;
                           individualUser = obj;
                           loadImage();
                           this.grdIndividualDetail.DataContext = individualUser;*/
        }

        private void cmbKhataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (CONTROLLER.ISENGLISHVISIBLE)
            //{
            //    if (cmbKhataType_e.SelectedItem != null)
            //    {
            //        sellertype sellerType = (sellertype)cmbKhataType_e.SelectedItem;
            //        if (sellerType.seller_cde == "00001")
            //        {
            //            loadIndividualSeller();
            //        }
            //        else if (sellerType.seller_cde == "00002")
            //        {
            //            loadCompanySeller();


            //        }
            //        else if (sellerType.seller_cde == "00003")
            //        {
            //            loadFeedMillUser();


            //        }
            //        else if (sellerType.seller_cde == "00004")
            //        {
            //            loadBankSUser();


            //        }


            //    }
            //}
            //if (CONTROLLER.ISURDUVISIBLE)
            //{
            //    if (cmbKhataType_u.SelectedItem != null)
            //    {
            //        sellertype sellerType = (sellertype)cmbKhataType_u.SelectedItem;
            //        if (sellerType.seller_cde == "00001")
            //        {
            //            loadIndividualSeller();
            //        }
            //        else if (sellerType.seller_cde == "00002")
            //        {
            //            loadCompanySeller();


            //        }
            //        else if (sellerType.seller_cde == "00003")
            //        {
            //            loadFeedMillUser();


            //        }
            //        else if (sellerType.seller_cde == "00004")
            //        {
            //            loadBankSUser();


            //        }

            //    }
            //}
        }


        private string getName(string typecode, int userid)
        {
            string retName = "";
            using (var db = new HCSMLEntities1())
            {
                if (CONTROLLER.ISENGLISHVISIBLE)
                {
                    if (typecode == "00001")
                    {
                        retName =
                            (from n in db.individualusers where n.individualid == userid select n.name_e).FirstOrDefault
                                ();
                    }
                    else if (typecode == "00002")
                    {
                        retName =
                            (from n in db.companyusers where n.companyid == userid select n.companyname_e)
                                .FirstOrDefault();
                    }
                    else if (typecode == "00003")
                    {
                        retName =
                            (from n in db.feedmillusers where n.feedmillid == userid select n.feemillname_e)
                                .FirstOrDefault();
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
                        retName =
                            (from n in db.individualusers where n.individualid == userid select n.name_u).FirstOrDefault
                                ();
                    }
                    else if (typecode == "00002")
                    {
                        retName =
                            (from n in db.companyusers where n.companyid == userid select n.companyname_u)
                                .FirstOrDefault();
                    }
                    else if (typecode == "00003")
                    {
                        retName =
                            (from n in db.feedmillusers where n.feedmillid == userid select n.feedmillname_u)
                                .FirstOrDefault();
                    }
                    else if (typecode == "00004")
                    {
                        retName = (from n in db.bankusers where n.bankid == userid select n.bankname_u).FirstOrDefault();
                    }
                }
            }
            return retName;
        }
    }
}
    