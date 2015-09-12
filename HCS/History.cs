using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCS
{
    public class History
    {

        public int seqid { get; set; }
        public int productid { get; set; }
        public System.DateTime date { get; set; }
        public string seller_cde { get; set; }
        public int sellerid { get; set; }
        public string sellername { get; set; }
        public int noofbags { get; set; }
        public decimal rate { get; set; }
        public decimal bagweight { get; set; }
        public decimal extraweight { get; set; }
        public decimal commission { get; set; }
        public decimal labour { get; set; }
        public int purchaserid { get; set; }
        public decimal commissionamt { get; set; }
        public decimal price { get; set; }
        public decimal payableprice { get; set; }
        public string productname { get; set; }
        public decimal totalpayable { get; set; }
        public decimal amoutpaid { get; set; }
        public string purchasername { get; set; }
        public decimal totalreceivable { get; set; }
        public decimal received { get; set; }
        public sellertype sellertype { get; set; }
        public bool isenglishvisible { get; set; }

        public bool isurduvisible { get; set; }
    }
}
