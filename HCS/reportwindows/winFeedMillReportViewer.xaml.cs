using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace HCS.reportwindows
{
    /// <summary>
    /// Interaction logic for winReportViewer.xaml
    /// </summary>
    public partial class winFeedMillReportViewer : Window
    {
        saleproduct productSold;
        private bool m_isurduvisible;
        public winFeedMillReportViewer()
        {
            InitializeComponent();
        }

        public winFeedMillReportViewer(saleproduct product, bool isurduvisible)
        {
            InitializeComponent();
            m_isurduvisible = isurduvisible;
            productSold = product;
        }

        private void CrystalReportViewer1_Loaded(object sender, RoutedEventArgs e)
        {
            ReportDocument report = new ReportDocument();
            if (m_isurduvisible)
            {
                report.Load("../../Reports/SaleReceipt_u.rpt");
            }
            if (!m_isurduvisible)
            {
                report.Load("../../Reports/SaleReceipt_e.rpt");
            }
            
            ArrayList reportdata = new ArrayList();
            reportdata.Add(productSold);
          
            using(var db = new HCSMLEntities1())
            {               
                try
                {
                    report.SetDataSource(from c in db.saleproducts where c.seqid == productSold.seqid select c);
                }
                catch (NotSupportedException ex)
                {}
                catch (Exception ex)
                {}
               
            }
            crystalReportsViewer1.ViewerCore.ReportSource = report;
            report.Dispose();
        }
    }
}
