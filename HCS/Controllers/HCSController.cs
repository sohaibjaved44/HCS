using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using HCS.Enums;

namespace HCS.Controllers
{
    public class HCSController
    {
        #region Private Members



        private HCSMLEntities1 dbContext;
        private List<individualuser> m_individualUsers;
        private List<companyuser> m_companyUsers;
        private List<bankuser> m_bankUsers;
        private List<feedmilluser> m_feedMillUsers;
        private List<purchaseruser> m_purchaserUsers;
        private List<sellertype> m_sellerTypes;
        private List<KhataTypeCode> m_khataTypeCode;
        private List<product> m_products;
        private List<saletype> m_saletype;
        private bool m_isUrduVisible;
        private bool m_isEnglishVisible;
        private decimal m_oldTotalReceivable = 0;
        private bool m_canUserEdit;
        private purchaseproduct m_purchaseProduct;
        private saleproduct m_saleProduct;
        #endregion

        #region Public Properties
        public saleproduct SALEPRODUCT
        {
            get { return m_saleProduct; }
            set { m_saleProduct = value; }

        }


        public purchaseproduct PURCHASEPRODUCT
        {
            get { return m_purchaseProduct; }
            set { m_purchaseProduct = value; }

        }
        public List<individualuser> INDIVIDUALUSERS
        {
            get { return m_individualUsers; }
            set { m_individualUsers = value; }

        }
        public List<saletype> SALETYPE
        {
            get { return m_saletype; }
            set { m_saletype = value; }

        }
        public List<companyuser> COMPANYUSERS
        {
            get { return m_companyUsers; }
            set { m_companyUsers = value; }

        }

        public List<bankuser> BANKUSERS
        {
            get { return m_bankUsers; }
            set { m_bankUsers = value; }

        }


        public List<feedmilluser> FEEDMILLUSERS
        {
            get { return m_feedMillUsers; }
            set { m_feedMillUsers = value; }

        }

        public List<purchaseruser> PURCHASERUSERS
        {
            get { return m_purchaserUsers; }
            set { m_purchaserUsers = value; }

        }

        public List<sellertype> SELLERTYPES
        {
            get { return m_sellerTypes; }
            set { m_sellerTypes = value; }

        }

        public List<KhataTypeCode> KHATATYPECODE
        {
            get { return m_khataTypeCode; }
            set { m_khataTypeCode = value; }

        }
        public List<product> PRODUCTS
        {
            get { return m_products; }
            set { m_products = value; }

        }

        public bool ISURDUVISIBLE
        {
            get { return m_isUrduVisible; }
            set { m_isUrduVisible = value; }

        }
        public bool ISENGLISHVISIBLE
        {

            get { return m_isEnglishVisible; }
            set { m_isEnglishVisible = value; }

        }

        public decimal OLDRECEIVABLEPRICE
        {
            get { return m_oldTotalReceivable; }
            set { m_oldTotalReceivable = value; }

        }


        public bool CANUSEREDIT
        {
            get { return m_canUserEdit; }
            set { m_canUserEdit = value; }

        }
        
        #endregion

        #region constructors

        public HCSController()
        {


        }

        public HCSController(bool canUserEdit)
        {

            dbContext = new HCSMLEntities1();
            CANUSEREDIT = canUserEdit;
            loadIndividual();
            loadCompany();
            loadBank();
            loadFeedMillUsers();
            loadPurchasers();
            loadSellerType();
            loadProducts();
            loadSaleType();
            loadKhataType();
        }
        
        #endregion

        #region Private Methods

        //private void updateIndividualUserColl(individualuser IndividualUser)
        //{
        //    individualuser temp = INDIVIDUALUSERS.Where(p => p.individualid == IndividualUser.individualid).FirstOrDefault();

        //    if(temp!=null )
        //    {
        //        INDIVIDUALUSERS.Remove(temp);
        //        INDIVIDUALUSERS.Add(IndividualUser);
        //    }

        //}

        #endregion

        #region Public Methods

        public void loadSaleType()
        {
            SALETYPE = dbContext.saletypes.ToList<saletype>();
        }

        public void loadIndividual()
        {


            INDIVIDUALUSERS = dbContext.individualusers.ToList<individualuser>();

        }

        public void loadCompany()
        {

            COMPANYUSERS = dbContext.companyusers.ToList<companyuser>();

        }
        public void loadBank()
        {

            BANKUSERS = dbContext.bankusers.ToList<bankuser>();

        }
        public void loadFeedMillUsers()
        {

            FEEDMILLUSERS = dbContext.feedmillusers.ToList<feedmilluser>();

        }

        public void loadPurchasers()
        {

            PURCHASERUSERS = dbContext.purchaserusers.ToList<purchaseruser>();

        }

        public void loadSellerType()
        {

            SELLERTYPES= dbContext.sellertypes.ToList<sellertype>();
        }

        public void loadKhataType()
        {
            KHATATYPECODE = dbContext.KhataTypeCodes.ToList<KhataTypeCode>();
        }
        public void loadProducts()
        {

            PRODUCTS = dbContext.products.ToList<product>();


        }

        #region save Sale Purchase

        public void saveProductPurchased(purchaseproduct purchasedPorduct)
        {

            #region commented handling sellertotal receivable table
            /*
            SellerTotalPayable totalPayables = dbContext.SellerTotalPayables.Where(p => p.sellerid == purchasedPorduct.sellerid).FirstOrDefault();
            if (totalPayables != null)
            {
                if(totalPayables.totalreceivable>0)
                {
                    if(totalPayables.totalreceivable>purchasedPorduct.payableprice)
                    {
                        totalPayables.totalreceivable -= (int)purchasedPorduct.payableprice;
                        totalPayables.totalpayable = 0;
                        purchasedPorduct.totalpayable = 0;
                        purchasedPorduct.totalreceivable = (int) totalPayables.totalreceivable;
                    }
                    else
                    {
                        purchasedPorduct.totalpayable =purchasedPorduct.payableprice- totalPayables.totalreceivable;
                        purchasedPorduct.totalreceivable = 0;
                        totalPayables.totalreceivable = 0;
                        totalPayables.totalpayable = purchasedPorduct.totalpayable;
                    }

                }
                else
                { 
                purchasedPorduct.totalpayable = totalPayables.totalpayable + purchasedPorduct.payableprice;
                totalPayables.totalpayable = purchasedPorduct.totalpayable;
                }
                var entry = dbContext.Entry<SellerTotalPayable>(totalPayables);
                if (entry.State.Equals(EntityState.Detached))
                {
                    dbContext.SellerTotalPayables.Attach(totalPayables);
                }
                entry.State = EntityState.Modified;


            }

            else
            {
                totalPayables = new SellerTotalPayable();
                purchasedPorduct.totalpayable = purchasedPorduct.payableprice;
                totalPayables.seller_cde = purchasedPorduct.seller_cde;
                totalPayables.sellerid = purchasedPorduct.sellerid;
                totalPayables.totalpayable = purchasedPorduct.totalpayable;

                dbContext.SellerTotalPayables.Add(totalPayables);

                }*/

            #endregion

            //saving in PurchaseProduct table                  
            dbContext.purchaseproducts.Add(purchasedPorduct);
            dbContext.SaveChanges();
            #region Khata
            
            var result = from n in dbContext.Khatas where n.bpid == purchasedPorduct.purchaserid select n;
            //now putting entries in KHATA table for purchaser(e.g ans) khata
            Khata kh = new Khata()
            {
                entrycode = "p",//indicates purchaser record
                purchaseproductid =  purchasedPorduct.seqid,
                saleproductid = 0,
                activitycode = ActivityType.Purchase.GetStringValue(),
                date = System.DateTime.Now,
                productid = purchasedPorduct.productid,                
                bpid = purchasedPorduct.purchaserid,
                sellertypecode = purchasedPorduct.seller_cde,
                payable_naam = purchasedPorduct.price,
                receivable_jama = 0,
                purchasertypecode = purchasedPorduct.purchaser_cde,
                totalpayable_naam = (result != null && result.Count() > 0) ? result.Sum(p => p.payable_naam) + purchasedPorduct.price : purchasedPorduct.price,
                totalreceivable_jama = (result != null && result.Count() > 0) ? result.Sum(p => p.receivable_jama) : 0
            };
            dbContext.Khatas.Add(kh);
            //now putting entries in KHATA table for seller(e.g Kissaan) khata
            result = from n in dbContext.Khatas where n.bpid == purchasedPorduct.sellerid select n;            

            kh = new Khata()
            {
                entrycode = "s",//indicates seller record
                purchaseproductid = purchasedPorduct.seqid,
                saleproductid = 0,
                activitycode = ActivityType.Purchase.GetStringValue(),
                date = System.DateTime.Now,
                productid = purchasedPorduct.productid,               
                bpid = purchasedPorduct.sellerid,
                sellertypecode = purchasedPorduct.seller_cde,
                payable_naam = 0,
                receivable_jama = purchasedPorduct.payableprice,
                purchasertypecode = purchasedPorduct.purchaser_cde,
                totalpayable_naam = (result != null && result.Count() > 0) ? result.Sum(p => p.payable_naam) : 0,
                totalreceivable_jama = (result != null && result.Count() > 0) ? result.Sum(p => p.receivable_jama) + purchasedPorduct.payableprice : purchasedPorduct.payableprice

            };
            dbContext.Khatas.Add(kh);
        #endregion
        #region Other Khata
            //Now adding in otherKhata table for labor commission etc.

           //here collecting information about labour khata so as to calculate previous values of naam and jama
            string khatatypecode = KhataType.Labour.GetStringValue(); 
            var resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase
            if (purchasedPorduct.selectedProduct.producttype != ProductType.Cash.GetStringValue()&& purchasedPorduct.selectedProduct.producttype != ProductType.cheque.GetStringValue())//there will be no other khata in case of cash.
            {
                OtherKhata ok = new OtherKhata()
                {
                    purchaseproductid = purchasedPorduct.seqid,
                    saleproductid = 0,
                    khatatypecde = KhataType.Labour.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = purchasedPorduct.labour,
                    payable_naam = 0,
                    productname = purchasedPorduct.productname,
                    weight = purchasedPorduct.totalweight,
                    activitycode = ActivityType.Purchase.GetStringValue(),
                    purchaserid = purchasedPorduct.purchaserid,
                    sellerid = purchasedPorduct.sellerid,
                    sellertypecode = purchasedPorduct.seller_cde,
                    purchasertypecode =  purchasedPorduct.purchaser_cde,
                    totalpayable_naam = (resultOtherKhata!= null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + purchasedPorduct.price : purchasedPorduct.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);

                if (purchasedPorduct.seller_cde != "00002")//if purchasing from commission shop then no commission to be entered 
                {
                    //here collecting information about commission khata so as to calculate previous values of naam and jama
                    khatatypecode = KhataType.Commission.GetStringValue();
                    resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase
                    ok = new OtherKhata()
                    {
                        purchaseproductid = purchasedPorduct.seqid,
                        saleproductid = 0,
                        khatatypecde = KhataType.Commission.GetStringValue(),
                        date = System.DateTime.Now,
                        receivable_jama = purchasedPorduct.commissionamt,
                        payable_naam = 0,
                        productname = purchasedPorduct.productname,
                        weight = purchasedPorduct.totalweight,
                        activitycode = ActivityType.Purchase.GetStringValue(),
                        purchaserid = purchasedPorduct.purchaserid,
                        sellerid = purchasedPorduct.sellerid,
                        sellertypecode = purchasedPorduct.seller_cde,
                        purchasertypecode = purchasedPorduct.purchaser_cde,
                        totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) : 0,
                        totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) + purchasedPorduct.payableprice : purchasedPorduct.payableprice

                    };
                    dbContext.OtherKhatas.Add(ok);
                }
            }

       #endregion
            dbContext.SaveChanges();

        }

        public void saveProductSold(saleproduct productSold)
        {
            try
            {

           
            #region commented code save to feedmilltotla receivaable
            /*
            FeedMillTotalRecievable totalRecievable = dbContext.FeedMillTotalRecievables.Where(p => p.feedmillid == productSold.feedmillid).FirstOrDefault();
            if (totalRecievable != null)
            {
                if(totalRecievable.totalPayable>0)
                {
                    if(totalRecievable.totalPayable>=productSold.recievableprice)
                    {
                        totalRecievable.totalPayable -= productSold.recievableprice;
                        productSold.totalpayable = totalRecievable.totalPayable;
                        totalRecievable.totalPriceRecievable = 0;
                        productSold.totalpricerecieveable = 0;

                    }
                    else
                    {

                        productSold.totalpricerecieveable = productSold.recievableprice - totalRecievable.totalPayable;
                        totalRecievable.totalPriceRecievable = productSold.totalpricerecieveable;

                        productSold.totalpayable = 0;
                        totalRecievable.totalPayable = 0;
                    
                    }

                }
                else
                { 
                productSold.totalpricerecieveable = totalRecievable.totalPriceRecievable + productSold.recievableprice;
                totalRecievable.totalPriceRecievable = productSold.totalpricerecieveable;
                }
                var entry = dbContext.Entry<FeedMillTotalRecievable>(totalRecievable);
                if (entry.State.Equals(EntityState.Detached))
                {
                    dbContext.FeedMillTotalRecievables.Attach(totalRecievable);
                }
                entry.State = EntityState.Modified;


            }

            else
            {
                totalRecievable = new FeedMillTotalRecievable();
                productSold.totalpricerecieveable = productSold.recievableprice;
                totalRecievable.feedmillid = productSold.feedmillid;
                totalRecievable.totalPriceRecievable = productSold.totalpricerecieveable;

                dbContext.FeedMillTotalRecievables.Add(totalRecievable);

            }
            */
#endregion
            //entry in product sold
            dbContext.saleproducts.Add(productSold);
                dbContext.SaveChanges();
            #region Khata
            var result = from n in dbContext.Khatas where n.bpid == productSold.purchaserid select n;
            //entry in khata table for purchaser
            Khata kh = new Khata()
            {
                entrycode = "p",//indicates purchaser record
                purchaseproductid =0,
                saleproductid = productSold.seqid,
                activitycode = ActivityType.Sale.GetStringValue(),
                date = System.DateTime.Now,
                productid = productSold.productid,
                bpid = productSold.purchaserid,
                //sellerid = productSold.sellerid,
                sellertypecode = productSold.sellertypecde,
                payable_naam = productSold.price,
                receivable_jama = 0,
                purchasertypecode = productSold.purchasertypecde,
                totalpayable_naam = (result != null && result.Count() > 0) ? result.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                totalreceivable_jama = (result != null && result.Count() > 0) ? result.Sum(p => p.receivable_jama) : 0
            };
            dbContext.Khatas.Add(kh);

            //now putting entries in KHATA table for seller(e.g Kissaan) khata
            result = from n in dbContext.Khatas where n.bpid == productSold.sellerid select n;
            kh = new Khata()
            {
                entrycode = "s",//indicates purchaser record
                purchaseproductid = 0,
                saleproductid = productSold.seqid,
                activitycode = ActivityType.Sale.GetStringValue(),
                date = System.DateTime.Now,
                productid = productSold.productid,
                //purchaserid = productSold.purchaserid,
                bpid = productSold.sellerid,
                sellertypecode = productSold.sellertypecde,
                payable_naam = 0,
                receivable_jama = productSold.recievableprice,
                totalpayable_naam = (result != null && result.Count() > 0) ? result.Sum(p => p.payable_naam) : 0,
                totalreceivable_jama = (result != null && result.Count() > 0) ? result.Sum(p => p.receivable_jama) + productSold.recievableprice : productSold.recievableprice,
                purchasertypecode = productSold.purchasertypecde
            };
            dbContext.Khatas.Add(kh);

            #endregion
            //-------------------------------------------------------
            //Now adding in otherKhata table for labor commission etc.
            //-------------------------------------------------------
            #region Other Khata

            //here collecting information about labour khata so as to calculate previous values of naam and jama
            string khatatypecode = KhataType.Labour.GetStringValue();
            var resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase

            OtherKhata ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Labour.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.labour,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);


                //here collecting information about labour khata so as to calculate previous values of naam and jama
                khatatypecode = KhataType.Commission.GetStringValue();
                resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase
                ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Commission.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.commissionamount,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);
                //here collecting information about labour khata so as to calculate previous values of naam and jama
                khatatypecode = KhataType.Bardana.GetStringValue();
                resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase

                ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Bardana.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.bardana,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);
                //here collecting information about labour khata so as to calculate previous values of naam and jama
                khatatypecode = KhataType.Fee.GetStringValue();
                resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase

                ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Fee.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.fee,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);
                //here collecting information about labour khata so as to calculate previous values of naam and jama
                khatatypecode = KhataType.Munshiyana.GetStringValue();
                resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase

                ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Munshiyana.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.accountcharges,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);
                //here collecting information about labour khata so as to calculate previous values of naam and jama
                khatatypecode = KhataType.Sootli.GetStringValue();
                resultOtherKhata = from n in dbContext.OtherKhatas where n.khatatypecde == khatatypecode select n;//activity code indicates it is khata agains purchase

                ok = new OtherKhata()
                {
                    activitycode = ActivityType.Sale.GetStringValue(),
                    khatatypecde = KhataType.Sootli.GetStringValue(),
                    date = System.DateTime.Now,
                    receivable_jama = productSold.thread,
                    payable_naam = 0,
                    productname = productSold.productname,
                    weight = productSold.weight,
                    purchaserid = productSold.purchaserid,
                    sellerid = productSold.sellerid,
                    sellertypecode = productSold.sellertypecde,
                    purchasertypecode = productSold.purchasertypecde,
                    totalpayable_naam = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.payable_naam) + productSold.price : productSold.price,
                    totalreceivable_jama = (resultOtherKhata != null && resultOtherKhata.Count() > 0) ? resultOtherKhata.Sum(p => p.receivable_jama) : 0
                };
                dbContext.OtherKhatas.Add(ok);

            #endregion

            dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        #endregion

        public void saveIndividualUser(individualuser IndividualUser)
        {
            try
            {
                dbContext.individualusers.Add(IndividualUser);
                dbContext.SaveChanges();
                INDIVIDUALUSERS.Add(IndividualUser);
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show(ex.EntityValidationErrors.ToString());
            }            
        }

        public void saveCompanyUser(companyuser CompanyUser)
        {
            dbContext.companyusers.Add(CompanyUser);
            dbContext.SaveChanges();
            COMPANYUSERS.Add(CompanyUser);
        }

        //saveBankUser
        public void saveBankUser(bankuser bankuser)
        {
            dbContext.bankusers.Add(bankuser);
            dbContext.SaveChanges();
            BANKUSERS.Add(bankuser);
        }
        //updateCompanyUser


        public void saveFeedMillUser(feedmilluser FeedMillUser)
        {
            dbContext.feedmillusers.Add(FeedMillUser);
            dbContext.SaveChanges();
            FEEDMILLUSERS.Add(FeedMillUser);

        }

        public void savePurchaserUser(purchaseruser PurchaserUser)
        {
            dbContext.purchaserusers.Add(PurchaserUser);
            dbContext.SaveChanges();
            PURCHASERUSERS.Add(PurchaserUser);
        }


        public void updateIndividualUser(individualuser IndividualUser)
        {
            var entry = dbContext.Entry<individualuser>(IndividualUser);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.individualusers.Attach(IndividualUser);
            }
            entry.State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void updateCompanyUser(companyuser CompanyUser)
        {
            var entry = dbContext.Entry<companyuser>(CompanyUser);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.companyusers.Attach(CompanyUser);
            }
            entry.State = EntityState.Modified;
        }

        public void updateBankUser(bankuser bankuser)
        {
            var entry = dbContext.Entry<bankuser>(bankuser);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.bankusers.Attach(bankuser);
            }
            entry.State = EntityState.Modified;
        }


        public void updateFeedMillUser(feedmilluser FeedMillUser)
        {
            var entry = dbContext.Entry<feedmilluser>(FeedMillUser);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.feedmillusers.Attach(FeedMillUser);
            }
            entry.State = EntityState.Modified;
        }

        public void updatePurchaserUser(purchaseruser PurchaserUser)
        {
            var entry = dbContext.Entry<purchaseruser>(PurchaserUser);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.purchaserusers.Attach(PurchaserUser);
            }
            entry.State = EntityState.Modified;
        }


        public List<saleproduct> getFeedMillHistory(int feedMillId)
        {

           return dbContext.saleproducts.Where(p => p.feedmillid == feedMillId).ToList<saleproduct>();
        }
        public List<purchaseproduct> getPurchaserHistory(int purchaserId)
        {

            return dbContext.purchaseproducts.Where(p => p.purchaserid == purchaserId).ToList<purchaseproduct>();
        }

        public List<saleproduct> getSaleHistory(int saleId)
        {

            return dbContext.saleproducts.Where(p => p.seqid == saleId).ToList<saleproduct>();
        }
        
        public List<purchaseproduct> getHistoryPurchaseProduct(string sellertype,int id)
        {
            return dbContext.purchaseproducts.Where(p => p.seller_cde == sellertype && (p.purchaserid==id || p.sellerid == id )).ToList<purchaseproduct>();
        }
        public List<saleproduct> getHistorySaleProduct(string sellertype, int sellerId)
        {
            return dbContext.saleproducts.Where(p => p.sellertypecde == sellertype && p.seqid == sellerId).ToList<saleproduct>();
        }

        public feedmilluser getFeedMillById(int feedMillId)
        {

            return dbContext.feedmillusers.Where(p => p.feedmillid == feedMillId).FirstOrDefault();

        }
        public void saveFeedMillPayment(saleproduct payment)
        {
            payment.date = DateTime.Now;
            FeedMillTotalRecievable feedMillPayment = dbContext.FeedMillTotalRecievables.Where(p => p.feedmillid == payment.feedmillid).FirstOrDefault();
            feedMillPayment.totalPriceRecievable = payment.totalpricerecieveable;
            feedMillPayment.totalPayable += payment.totalpayable;
            payment.totalpayable = feedMillPayment.totalPayable;
            var entry = dbContext.Entry<FeedMillTotalRecievable>(feedMillPayment);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.FeedMillTotalRecievables.Attach(feedMillPayment);
            }
            entry.State = EntityState.Modified;
 
            dbContext.saleproducts.Add(payment);
            dbContext.SaveChanges();

           


        }
        public void saveSellerPayment(purchaseproduct payment)
        {
            payment.date = DateTime.Now;
            SellerTotalPayable sellerPayment = dbContext.SellerTotalPayables.Where(p => p.seller_cde== payment.seller_cde && p.sellerid==payment.sellerid).FirstOrDefault();
            sellerPayment.totalpayable = payment.totalpayable;
            sellerPayment.totalreceivable += payment.totalreceivable;
            payment.totalreceivable = (int) sellerPayment.totalreceivable;
            var entry = dbContext.Entry<SellerTotalPayable>(sellerPayment);
            if (entry.State.Equals(EntityState.Detached))
            {
                dbContext.SellerTotalPayables.Attach(sellerPayment);
            }
            entry.State = EntityState.Modified;
 
            dbContext.purchaseproducts.Add(payment);
            dbContext.SaveChanges();




        }

        public void updateProductSold(saleproduct productSold)
        {
            removeSale(productSold);
            FeedMillTotalRecievable totalRecievable = dbContext.FeedMillTotalRecievables.Where(p => p.feedmillid == productSold.feedmillid).FirstOrDefault();
            if (totalRecievable != null)
            {
                if (totalRecievable.totalPayable > 0)
                {
                    if (productSold.recievableprice > (totalRecievable.totalPayable + OLDRECEIVABLEPRICE))
                    {
                        totalRecievable.totalPriceRecievable = productSold.recievableprice - (totalRecievable.totalPayable + OLDRECEIVABLEPRICE);
                        productSold.totalpricerecieveable = totalRecievable.totalPriceRecievable;
                        totalRecievable.totalPayable = 0;
                        productSold.totalpayable = 0;
                    }
                    else
                    {

                        totalRecievable.totalPayable = totalRecievable.totalPayable - productSold.recievableprice + OLDRECEIVABLEPRICE;
                        productSold.totalpayable = totalRecievable.totalPayable;
                        totalRecievable.totalPriceRecievable = 0;
                        productSold.totalpricerecieveable = 0;
                    }
                }

                else
                {
                    if (OLDRECEIVABLEPRICE > (totalRecievable.totalPriceRecievable + productSold.recievableprice))
                    {
                        totalRecievable.totalPayable = OLDRECEIVABLEPRICE - (totalRecievable.totalPriceRecievable + productSold.recievableprice);
                        productSold.totalpayable = totalRecievable.totalPayable;
                        totalRecievable.totalPriceRecievable = 0;
                        productSold.totalpricerecieveable = 0;

                    }
                    else
                    {

                        productSold.totalpricerecieveable = totalRecievable.totalPriceRecievable + productSold.recievableprice - OLDRECEIVABLEPRICE;
                        totalRecievable.totalPriceRecievable = productSold.totalpricerecieveable;
                        productSold.totalpayable = 0;
                        totalRecievable.totalPayable = 0;
                    }
                }
                var entry = dbContext.Entry<FeedMillTotalRecievable>(totalRecievable);
                if (entry.State.Equals(EntityState.Detached))
                {
                    dbContext.FeedMillTotalRecievables.Attach(totalRecievable);
                }
                entry.State = EntityState.Modified;


            }

            dbContext.saleproducts.Add(productSold);
            dbContext.SaveChanges();

        }

        private void removeSale(saleproduct removeProduct)
        {
            dbContext.saleproducts.Remove(removeProduct);
            dbContext.SaveChanges();


        }

        public List<purchaseproduct> getPurchaseByDateRange( DateTime from , DateTime to)
        {
            return dbContext.purchaseproducts.Where(p => p.date >= from && p.date <= to).ToList<purchaseproduct>();


        }

        public List<saleproduct> getSaleByDateRange(DateTime from , DateTime to)
        {
            return dbContext.saleproducts.Where(p => p.date >= from && p.date <= to).ToList<saleproduct>();
        }
        
        #endregion


    }
}
