using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;

namespace PW.Ncels.Database.Repository.OBK
{
    public class OBKPaymentRepository : ARepository
    {
        public OBKPaymentRepository()
        {
            AppContext = CreateDatabaseContext();
        }

        public OBKPaymentRepository(bool isProxy)
        {
            AppContext = CreateDatabaseContext(isProxy);
        }

        public OBKPaymentRepository(ncelsEntities context) : base(context) { }


        public void SavePayments(Guid contractId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            var pay = new OBK_DirectionToPayments();
            if (contract != null)
            {
                pay.Id = Guid.NewGuid();
                pay.CreateDate = DateTime.Now;
                pay.ContractId = contractId;
                pay.CreateEmployeeId = UserHelper.GetCurrentEmployee().Id;
                pay.CreateEmployeeValue = UserHelper.GetCurrentEmployee().DisplayName;
                pay.DirectionDate = DateTime.Now;
                pay.Number = contract.Number;
                pay.PayerId = contract.OBK_Declarant.Id;
                pay.PayerValue = contract.OBK_Declarant.NameRu;
                pay.IsDeleted = false;
                pay.TotalPrice = GetTotalPriceCount(contractId);
            }
            AppContext.SaveChanges();
        }

        public decimal GetTotalPriceCount(Guid contractId)
        {
            var priceCounts = AppContext.OBK_ContractPrice.Where(e => e.ContractId == contractId).ToList();
            decimal tPrice = 0;
            foreach (var priceCount in priceCounts)
            {
                tPrice = Convert.ToDecimal(priceCount.PriceWithTax);
            } 
            return tPrice;
        }
    }
}
