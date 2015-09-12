using System.Collections;
using System.Linq;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;

namespace HCS.reportwindows
{
    /// <summary>
    /// Interaction logic for winPurchaserReportViewer.xaml
    /// </summary>
    public partial class winPurchaserReportViewer : Window
    {
        #region Private Members

        purchaseproduct ProductPurchased;
        private bool m_isurduvisible;
        #endregion




        #region Constructors and Loaders
        public winPurchaserReportViewer()
        {
            InitializeComponent();
        }

        public winPurchaserReportViewer(purchaseproduct product,bool isurduvisible)
        {
            InitializeComponent();
            m_isurduvisible = isurduvisible;
            ProductPurchased = product;
        }

        #endregion

        #region Event Handlers

        private void CrystalReportViewer1_Loaded(object sender, RoutedEventArgs e)
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
            reportdata.Add(ProductPurchased);
            //report.SetDataSource(reportdata);
            using (var db = new HCSMLEntities1())
            {
                report.SetDataSource(from c in db.purchaseproducts where c.seqid == ProductPurchased.seqid select c);
            }
            CrystalReportsViewerSeller1.ViewerCore.ReportSource = report;
            //ReportDocument report = new ReportDocument();
            //report.Load("../../Reports/PurchaserReport.rpt");

            //ArrayList reportdata = new ArrayList();
            //reportdata.Add(ProductPurchased);

            //report.SetDataSource(reportdata);

            //using (var db = new HCSMLEntities())
            //{
            //    report.SetDataSource(from c in db.purchaseproducts where c.seqid == 6 select c);


            //}
            //crystalReportsViewer1.ViewerCore.ReportSource = report;
        }

        #endregion
    }
}
