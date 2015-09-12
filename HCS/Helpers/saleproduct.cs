namespace HCS
{
   public partial class saleproduct
    {
        public bool isurduvisible { get; set; }
        public bool isenglishvisible { get; set; }

        public int purchaserid { get; set; }
        public int sellerid { get; set; }

        public product selectedProduct { get; set; }
    }
}
