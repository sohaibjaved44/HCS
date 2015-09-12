using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CrystalDecisions.CrystalReports.Engine;
using HCS.Controllers;
using HCS.datasets;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;

namespace HCS.UserControls.Reports
{
    /// <summary>
    /// Interaction logic for ucDailyReport.xaml
    /// </summary>
    public partial class ucDailyReport : UserControl
    {
        #region Private Members
        
        private HCSController m_controller;
        private GenerateReportInfo data;
        private List<saleproduct> producstSold;
        private List<purchaseproduct> productsPurchased;
        
        #endregion

        #region Public Properties
        
        public HCSController CONTROLLER
        {
            get { return m_controller; }
            set { m_controller = value; }


        }
        
        
        #endregion
        
        #region Constructors and Loaders
        public ucDailyReport()
        {
            InitializeComponent();
        }

        public ucDailyReport(HCSController controller)
        {
            InitializeComponent();

            CONTROLLER = controller;
        }

        private void ucDailyReport_Loaded(object sender, RoutedEventArgs e)
        {
            data = new GenerateReportInfo() { FROMDATE = DateTime.Now, TODATE = DateTime.Now , isenglishvisible=CONTROLLER.ISENGLISHVISIBLE,isurduvisible=CONTROLLER.ISURDUVISIBLE };
            this.grdReportGeneration.DataContext = data;
            this.grdModel.DataContext = data;
        }

        #endregion

        #region Event Handlers

        private void btnPurchaseReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ٹیسٹ دورانیہ میں یہ سہولت میسر نہیں۔");
            return;
            /*
            productsPurchased = CONTROLLER.getPurchaseByDateRange(data.FROMDATE, data.TODATE);
            producstSold = CONTROLLER.getSaleByDateRange(data.FROMDATE, data.TODATE);
            if (productsPurchased == null)
                productsPurchased = new List<purchaseproduct>();


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf File |*.pdf";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {

                Document doc = new Document(PageSize.LETTER, 1, 1, 42, 35);
              
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));

                doc.Open();
                if (CONTROLLER.ISENGLISHVISIBLE)
                {
                    Paragraph para = new Paragraph("AL HAMD COMMISSION SHOP",
                        FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.RED));
                    para.Alignment = PdfPCell.ALIGN_CENTER;

                    doc.Add(para);

                    Paragraph para1 = new Paragraph("Report",
                        FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE));
                    para1.Alignment = PdfPCell.ALIGN_CENTER;

                    doc.Add(para1);
                }
                if (CONTROLLER.ISURDUVISIBLE)
                {
                    Paragraph para = new Paragraph("alhamdالحمد کمیشن شاپ",
                        FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.RED));
                    para.Alignment = PdfPCell.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("report رپورٹ",
                        FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE));
                    para1.Alignment = PdfPCell.ALIGN_CENTER;
                    doc.Add(para1);

                }
                PdfPTable table = new PdfPTable(3);//3is number of columns
                table.SpacingBefore = 20f;
                float[] widths = new float[] { 1.5f, 1.5f, 1.5f};
                table.SetWidths(widths);
                table.AddCell(new Phrase("Product Name", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Payable amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Amount Paid", FontFactory.GetFont("Arial", 10, Font.BOLD)));


                foreach (purchaseproduct product in productsPurchased)
                {
                    table.AddCell(product.productname);
                    table.AddCell(product.amoutpaid.ToString());
                    table.AddCell(product.payableprice.ToString());
                }

                ///////////////////////////////////////////////////////////////////////
                //table.HorizontalAlignment = Element.ALIGN_LEFT;
                ///////////////////////////////////////////////////////////////////////
                PdfPTable table1 = new PdfPTable(3);//3is number of columns
                table.SpacingBefore = 20f;
                //float[] widths = new float[] { 1.5f, 1.5f, 1.5f };
                table1.SetWidths(widths);
                table1.AddCell(new Phrase("Product Name", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table1.AddCell(new Phrase("Received amount", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table1.AddCell(new Phrase("Amount Receiveable", FontFactory.GetFont("Arial", 10, Font.BOLD)));


                foreach (saleproduct product in producstSold)
                {
                    table1.AddCell(product.productname);
                    table1.AddCell(product.amountrecieved.ToString());
                    table1.AddCell(product.recievableprice.ToString());
                }




                PdfPTable headerTable = new PdfPTable(2);
               
                headerTable.AddCell(new Phrase("Payments", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                headerTable.AddCell(new Phrase("Receiving", FontFactory.GetFont("Arial", 10, Font.BOLD)));


                PdfPTable MainTable = new PdfPTable(2);
                MainTable.SpacingBefore = 20f;
                               
                PdfPCell mainTableCell1 = new PdfPCell(table);
                PdfPCell mainTableCell2 = new PdfPCell(table);
                MainTable.AddCell(mainTableCell1);
                MainTable.AddCell(mainTableCell2);



                doc.Add(headerTable);
                doc.Add(MainTable);                
                doc.Close();

                MessageBox.Show("Report Generated Successfully!");

            }

            */
                     
        }

        private void btnFeedMillReport_Click(object sender, RoutedEventArgs e)
        {
            producstSold = CONTROLLER.getSaleByDateRange(data.FROMDATE, data.TODATE);

            if (producstSold == null)
                producstSold = new List<saleproduct>();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf File |*.pdf";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {

                Document doc = new Document(PageSize.LETTER, 1, 1, 42, 35);
                PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));

                doc.Open();
                Paragraph para = new Paragraph("AL HAMD COMMISSION SHOP", FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.RED));
                para.Alignment = PdfPCell.ALIGN_CENTER;

                doc.Add(para);
                Paragraph para1 = new Paragraph("Feed Mill History Report", FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLUE));
                para1.Alignment = PdfPCell.ALIGN_CENTER;

                doc.Add(para1);

                PdfPTable table = new PdfPTable(7);
                table.SpacingBefore = 20f;
                float[] widths = new float[] { 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f };
                table.SetWidths(widths);
         
                table.AddCell(new Phrase("Date", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Feed Mill", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Product", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Amount Received", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Receivable", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Total Receivable", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                table.AddCell(new Phrase("Total Payable", FontFactory.GetFont("Arial", 10, Font.BOLD)));


                foreach (saleproduct sale in producstSold)
                {
                    table.AddCell(sale.date.ToString("M/d/yyyy"));
                    table.AddCell(sale.purchasername);
                    table.AddCell(sale.productname);
                    table.AddCell(sale.amountrecieved.ToString());
                    table.AddCell(sale.recievableprice.ToString());
                    table.AddCell(sale.totalpricerecieveable.ToString());
                    table.AddCell(sale.totalpayable.ToString());

                   

                }
                doc.Add(table);
                doc.Close();

                MessageBox.Show("Feed Mill Report Generated Successfully!");

            }



        }

        #endregion

        private void CrystalReportViewer1_Loaded(object sender, RoutedEventArgs e)
        {

            using (var db = new HCSMLEntities1())
            {
                productsPurchased = (from p in db.purchaseproducts select p).ToList();
                producstSold = (from p in db.saleproducts select p).ToList();
            }
            var report = new ReportDocument();
                if (CONTROLLER.ISENGLISHVISIBLE)
                {
                    report.Load("../../Reports/SalePurchaseReport.rpt");
                }
                if (CONTROLLER.ISURDUVISIBLE)
                {
                    report.Load("../../Reports/SalePurchaseReport_e.rpt");
                }

                //ArrayList reportdata = new ArrayList();
                //reportdata.Add(productsPurchased);                 

                //report.Database.Tables[1].SetDataSource(reportdata);
                //var purchasedata = (from p in db.purchaseproducts select p);
                report.Subreports["PaymentReport.rpt"].SetDataSource(productsPurchased);

                //reportdata = new ArrayList();
                //reportdata.Add(producstSold);

                //report.Database.Tables[0].SetDataSource(reportdata);  
                //var saledata = from p in db.saleproducts select p;
                report.Subreports["ReceivingReport.rpt"].SetDataSource(producstSold);

                CrystalReportsViewerSeller1.ViewerCore.ReportSource = report;
                //CrystalReportsViewerSeller1.Refresh(report);
                report.Dispose();
                        
        }


    }
}
