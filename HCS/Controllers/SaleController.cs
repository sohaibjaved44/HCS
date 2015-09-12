using System.Collections.Generic;
using System.Linq;

namespace HCS.Controllers
{
    class SaleController
    {
        #region Private Members
        HCSMLEntities1 saleDb;
        
        #endregion

        #region Consturctor
        public SaleController()
        {

            saleDb = new HCSMLEntities1();
        }
        #endregion

        #region public Methods
        
        public List<product> getProducts()
        {

            return saleDb.products.ToList<product>();


        }

        public List<feedmilluser> getFeedMillUsers()
        {

            return saleDb.feedmillusers.ToList<feedmilluser>();

        }
        
        public void saveProductSold(saleproduct productSold)
        {
            saleDb.saleproducts.Add(productSold);
            saleDb.SaveChanges();

        }

        #endregion
    }
}
