﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Notifications;

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
                    pay.OBK_DirectionSignData = new OBK_DirectionSignData()
                    {
                        DirectionToPaymentId = pay.Id,
                        ChiefAccountantId = Guid.Parse("E1EE3658-0C35-41EB-99FD-FDDC4D07CEC4"),
                        ExecutorId = Guid.Parse("55377FAC-A5F0-4093-BBB6-18BD28E53BE1")
                    };
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
                    pay.OBK_DirectionSignData = payment.OBK_DirectionSignData;
                }
                AppContext.OBK_DirectionToPayments.Add(pay);
                AppContext.SaveChanges();
            }
        }
        /// <summary>
        /// расчет стоимости с НДС
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public decimal GetTotalPriceCount(Guid contractId)
        {
            var priceCounts = AppContext.OBK_ContractPrice.Where(e => e.ContractId == contractId).ToList();
            var tPrice = priceCounts.Sum(e => Math.Round(Convert.ToDecimal(TaxHelper.GetCalculationTax(e.OBK_Ref_PriceList.Price) * e.Count), 2));
            return tPrice;
        }

        public IQueryable<OBK_DirectionToPaymentsView> GetDirectionToPayments()
        {
            var data = AppContext.OBK_DirectionToPaymentsView
                .Where(e => e.IsDeleted == false);
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
            directionPay.OBK_Contract.Status = CodeConstManager.STATUS_OBK_EXPECTED_PAYMENT;
            AppContext.SaveChanges();
            //отправка уведоления
            new NotificationManager().SendNotification(
                string.Format("По Вашему договору №{0} поступил счет на оплату", directionPay.OBK_Contract.Number),
                ObjectType.ObkContract, directionPay.OBK_Contract.Id, (Guid) directionPay.OBK_Contract.EmployeeId);
            
            //todo добавить изменения статуса договора
        }
        /// <summary>
        /// НДС для формирования печатной формы
        /// </summary>
        /// <param name="nds"></param>
        /// <returns></returns>
        public decimal GetTotalPriceNDS(decimal nds)
        {
            return Math.Round(TaxHelper.GetTaxWithPrice(nds), 2);
        }

        public string GetEmpoloyee(Guid userId)
        {
            return AppContext.Employees.FirstOrDefault(e=>e.Id == userId)?.ShortName;
        }

        public Dictionary GetDictionary(Guid? id)
        {
            if (id == null)
                return null;
            return AppContext.Dictionaries.FirstOrDefault(e => e.Id == id);
        }

        public string GetSignData(Guid id)
        {
            var payment = AppContext.OBK_DirectionToPayments.FirstOrDefault(e => e.Id == id);
            var contract = AppContext.OBK_Contract.FirstOrDefault(e => e.Id == payment.ContractId);
            var unitsBank = AppContext.UnitsBanks.FirstOrDefault(e => e.UnitsId == contract.ExpertOrganization);

            List<ContractPriceSignData> contractPriceSign = AppContext.OBK_ContractPrice
                .Where(e => e.ContractId == contract.Id)
                .Select(e => new ContractPriceSignData()
                {
                    ContractPriceName = e.OBK_RS_Products.NameRu,
                    ContractPriceDicName = e.OBK_Ref_PriceList.NameRu,
                    ContractPrice = e.OBK_Ref_PriceList.Price * 1.12,
                    ContractPriceCount = e.Count,
                    ContractPriceTotal = e.OBK_Ref_PriceList.Price * 1.12 * e.Count,
                }).ToList();
            
            var result = new OBKPaymentSignData {
                Id = id,
                ContractId = payment?.ContractId,
                ContactNumber = contract?.Number,
                ContactStartDate = contract?.StartDate,
                ContactTypeName = contract?.OBK_Ref_Type.NameRu,
                UnitsName = contract?.Unit.Name,
                UnitsAddress = contract?.Unit.LegalAddress,
                UnitsPhone = contract?.Unit.Phone,
                UnitsBin = contract?.Unit.Bin,
                UnitsIIk = unitsBank?.IIK,
                UnitsKbe = unitsBank?.KBE,
                UnitsBankName = unitsBank?.BankNameRu,
                UnitsBankSwift = unitsBank?.SWIFT,
                UnitsBankCode = unitsBank?.Code,
                InvoiceNuber1C = payment?.InvoiceNumber1C,
                InvoiceDate1C = payment?.InvoiceDatetime1C,
                DeclarantBin = contract?.OBK_Declarant.Bin,
                DeclarantOrgName = GetDictionary(contract?.OBK_Declarant.OrganizationFormId).Name,
                DeclarantName = contract?.OBK_Declarant.NameRu,
                DeclarantCountryName = GetDictionary(contract?.OBK_DeclarantContact.CurrencyId).Name,
                DeclarantAddressLegal = contract?.OBK_DeclarantContact.AddressLegalRu,
                ContractPriceNds = GetTotalPriceCount(payment.ContractId),
                ContractPriceTotalText = RuDateAndMoneyConverter.CurrencyToTxtTenge(Convert.ToDouble(GetTotalPriceCount(payment.ContractId)), false),
                ChiefAccountant = GetEmpoloyee(Guid.Parse("E1EE3658-0C35-41EB-99FD-FDDC4D07CEC4")),
                Executor = GetEmpoloyee(Guid.Parse("55377FAC-A5F0-4093-BBB6-18BD28E53BE1")),
                ContractPriceSignDatas = contractPriceSign
            };
            var xmlData = SerializeHelper.SerializeDataContract(result);
            return xmlData.Replace("utf-16", "utf-8");
        }

        public void SaveSignPay(Guid paymentId, string signedData)
        {
            var directionSignData =
                AppContext.OBK_DirectionSignData.FirstOrDefault(e => e.DirectionToPaymentId == paymentId);
        }


        #region оплата Job

        public List<OBK_DirectionToPayments> GetDeclarantPaid()
        {
            return AppContext.OBK_DirectionToPayments.Where(e => e.IsPaid && !e.IsNotFullPaid && (e.SendNotification == null || e.SendNotification == "sendNotFullPaid"))
                .ToList();
        }

        public List<OBK_DirectionToPayments> GetDeclarantNotFullPaid()
        {
            return AppContext.OBK_DirectionToPayments.Where(e => e.IsPaid && e.IsNotFullPaid && e.SendNotification == null)
                .ToList();
        }

        public List<OBK_DirectionToPayments> GetPaidExpired()
        {
            var result = AppContext.OBK_DirectionToPayments
                .Where(e => (bool) !e.IsDeleted &&
                            e.OBK_Contract.Status != CodeConstManager.STATUS_OBK_PAYMENT_EXPIRED &&
                            e.OBK_Contract.Status == CodeConstManager.STATUS_OBK_EXPECTED_PAYMENT &&
                            e.OBK_Contract.Status == CodeConstManager.STATUS_OBK_NOT_FULL_PAYMENT)
                .ToList();
            List<OBK_DirectionToPayments> dPayments = new List<OBK_DirectionToPayments>();
            foreach (var r in result)
            {
                if (GetWordDate(r.DirectionDate) < DateTime.Now)
                    dPayments.Add(r);
            }
            return dPayments;
        }

        public DateTime GetWordDate(DateTime sendDate)
        {
            var factDate = sendDate.AddDays(5);
            if (factDate.DayOfWeek == DayOfWeek.Saturday)
                factDate.AddDays(2);
            if (factDate.DayOfWeek == DayOfWeek.Sunday)
                factDate.AddDays(1);
            return factDate;
        }

        public void UpdateStatus(OBK_DirectionToPayments dicPayments)
        {
            dicPayments.OBK_Contract.Status = CodeConstManager.STATUS_OBK_PAYMENT_EXPIRED;
            AppContext.SaveChanges();
        }

        public void UpdateNotificationToPayment(OBK_DirectionToPayments dicPayments, bool payStatus)
        {
            dicPayments.SendNotification = payStatus ? "send" : "sendNotFullPaid";
            dicPayments.OBK_Contract.Status = payStatus ? CodeConstManager.STATUS_OBK_ACTIVE : CodeConstManager.STATUS_OBK_NOT_FULL_PAYMENT;
            AppContext.SaveChanges();
        }

        #endregion
    }
}
