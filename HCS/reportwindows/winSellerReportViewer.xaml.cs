using System.Collections;
using System.Linq;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using HCS.Controllers;

namespace HCS.reportwindows
{
    /// <summary>
    /// Interaction logic for winSellerReportViewer.xaml
    /// </summary>
    public partial class winSellerReportViewer : Window
    {
        #region Private Members

        private purchaseproduct porodcutPurchased;
        private bool m_isurduvisible;
        #endregion
        #region Public Properties
       
        #endregion



        #region Constructors and Loaders
        public winSellerReportViewer()
        {
            InitializeComponent();
        }

        public winSellerReportViewer(purchaseproduct Product,bool isurduvisible)
        {
            InitializeComponent();
            m_isurduvisible = isurduvisible;
            porodcutPurchased = Product;
        }

        #endregion

        #region Event Handlers

        private void CrystalReportsViewerSeller_Loaded(object sender, RoutedEventArgs e)
        {
            ReportDocument report = new ReportDocument();
            if (!m_isurduvisible)
            {
                report.Load("../../Reports/ProdcutPurchase.rpt");
            }
            if (m_isurduvisible)
            {
                report.Load("../../Reports/ProductPurchase_u.rpt");
            }
            
            ArrayList reportdata = new ArrayList();
            reportdata.Add(porodcutPurchased);
            //report.SetDataSource(reportdata);
            using (var db = new HCSMLEntities1())
            {
                report.SetDataSource(from c in db.purchaseproducts where c.seqid == porodcutPurchased.seqid select c);
            }
            CrystalReportsViewerSeller.ViewerCore.ReportSource = report;
            report.Dispose();
        }
        #endregion
    }
}
