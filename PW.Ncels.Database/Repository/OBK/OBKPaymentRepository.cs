using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var contract = AppContext.OBK_Contract.FirstOrDefault(x => x.Id == contractId);
            var payment = AppContext.OBK_DirectionToPayments.FirstOrDefault(e => e.ContractId == contractId);
            var pay = new OBK_DirectionToPayments();
            if (contract != null)
            {
                if (payment == null)
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
                    pay.StatusId = GetPaymentStatus(OBK_Ref_PaymentStatus.OnFormation).Id;
                }
                else
                {
                    pay.Id = payment.Id;
                    //pay.CreateDate = DateTime.Now;
                    pay.ContractId = payment.ContractId;
                    pay.CreateEmployeeId = UserHelper.GetCurrentEmployee().Id;
                    pay.CreateEmployeeValue = UserHelper.GetCurrentEmployee().DisplayName;
                    pay.DirectionDate = DateTime.Now;
                    pay.Number = payment.Number;
                    pay.PayerId = payment.OBK_Declarant.Id;
                    pay.PayerValue = payment.OBK_Declarant.NameRu;
                    pay.IsDeleted = false;
                    pay.TotalPrice = GetTotalPriceCount(contractId);
                    pay.StatusId = payment.StatusId;
                }
                AppContext.OBK_DirectionToPayments.Add(pay);
                AppContext.SaveChanges();
            }
        }

        public decimal GetTotalPriceCount(Guid contractId)
        {
            var priceCounts = AppContext.OBK_ContractPrice.Where(e => e.ContractId == contractId).ToList();
            var tPrice = priceCounts.Sum(e => Convert.ToDecimal(e.PriceWithoutTax));
            return tPrice;
        }

        public IQueryable<OBK_DirectionToPaymentsView> GetDirectionToPayments(Guid currentUserId)
        {
            var data = AppContext.OBK_DirectionToPaymentsView
                .Where(e => e.CreateEmployeeId == currentUserId && e.IsDeleted == false);
            return data;
        }

        public OBK_Contract GetContract(Guid id)
        {
            return AppContext.OBK_Contract.FirstOrDefault(e => e.Id == id);
        }

        public OBK_Ref_PaymentStatus GetPaymentStatus(string code)
        {
            return AppContext.OBK_Ref_PaymentStatus.First(e => e.Code == code);
        }

        public void SendInvoiceToDeclarant(Guid id)
        {
            var directionPay = AppContext.OBK_DirectionToPayments.First(e => e.Id == id);
            directionPay.StatusId = GetPaymentStatus(OBK_Ref_PaymentStatus.SendToPayment).Id;
            AppContext.SaveChanges();
            //todo добавить изменения статуса договора
        }

        public decimal GetTotalPriceWithCount(Guid contractId)
        {
            var priceCounts = AppContext.OBK_ContractPrice.Where(e => e.ContractId == contractId).ToList();
            List<decimal> d = new List<decimal>();
            foreach (var p in priceCounts)
            {
                d.Add(Convert.ToDecimal(p.Count * p.PriceWithTax));
            }
            return Math.Round(d.Sum(), 2);
        }

        public decimal GetTotalPriceNDS(decimal nds)
        {
            return Math.Round(nds * (decimal)0.12, 2);
        }
    }
}
