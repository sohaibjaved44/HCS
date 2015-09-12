//using CrystalDecisions.CrystalReports.Engine;

using System.Linq;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;

namespace HCS
{
    /// <summary>
    /// Interaction logic for winReportTesting.xaml
    /// </summary>
    public partial class winReportTesting : Window
    {
        public winReportTesting()
        {
            InitializeComponent();
        }

        private void CrystalReportViewer1_Loaded(object sender, RoutedEventArgs e)
        {
            ReportDocument report = new ReportDocument();
            report.Load("../../Reports/ProductPurchaseHistory.rpt");

            //ArrayList reportdata = new ArrayList();
            //reportdata.Add(porodcutPurchased);

            //report.SetDataSource(reportdata);

            using (var db = new HCSMLEntities1())
            {
                report.SetDataSource(from c in db.purchaseproducts where c.seqid == 47 select c);


            }
            //crystalReportsViewer1.ViewerCore.ReportSource = report;
        }
    }
}
