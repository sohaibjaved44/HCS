using System.Collections.Generic;
using System.Linq;

namespace HCS.Controllers
{
    class PurchaseController
    {
        #region Private Members

        HCSMLEntities1 db;
        
        #endregion
       public PurchaseController()
        {

            db = new HCSMLEntities1();


        }


        public List<product> loadProducts()
        {

            return db.products.ToList<product>();


        }

        public List<sellertype> loadSellerType()
        {

            return db.sellertypes.ToList<sellertype>();
        }

        public List<purchaseruser> loadPurchasers()
        {

            return db.purchaserusers.ToList<purchaseruser>();

        }

        public List<individualuser> loadIndividual()
        {


            return db.individualusers.ToList<individualuser>();

        }

        public List<companyuser> loadCompany()
        {

            return db.companyusers.ToList<companyuser>();

        }

        public void saveProductPurchased(purchaseproduct purchasedPorduct)
        {
            db.purchaseproducts.Add(purchasedPorduct);
            db.SaveChanges();

        }

         ~PurchaseController()
        {

            db.Dispose();

        }
    }
}
