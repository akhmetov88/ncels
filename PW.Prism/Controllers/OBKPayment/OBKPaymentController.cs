﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ncels.Helpers;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Repository.DirectionToPay;
using PW.Ncels.Database.Repository.OBK;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace PW.Prism.Controllers.OBKPayment
{
    public class OBKPaymentController : Controller
    {
        OBKPaymentRepository payRepo = new OBKPaymentRepository();

        // GET: OBKPayment
        public ActionResult Index()
        {
            return PartialView(Guid.NewGuid());
        }

        public JsonResult ReadDirectionList([DataSourceRequest] DataSourceRequest request)
        {
            //var currentUserId = UserHelper.GetCurrentEmployee().Id;
            var data = payRepo.GetDirectionToPayments();
            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Guid contractId)
        {
            var model = payRepo.GetContract(contractId);
            return PartialView(model);
        }

        public ActionResult SendToDeclarant(Guid id)
        {
            payRepo.SendInvoiceToDeclarant(id);
            return Json(new { IsSuccess = true });
        }

        public ActionResult GetContractPrice(Guid id)
        {
            var model = payRepo.GetContract(id);
            List<ContractPrice> result = new List<ContractPrice>();
            int i = 1;
            foreach (var com in model.OBK_ContractPrice)
            {
                var cPrice = new ContractPrice();
                cPrice.Line = i;
                cPrice.Count = com.Count;
                cPrice.PriceWithTax = Math.Round(TaxHelper.GetCalculationTax(com.OBK_Ref_PriceList.Price), 2) ;
                cPrice.Price = Math.Round(com.Count * TaxHelper.GetCalculationTax(com.OBK_Ref_PriceList.Price), 2);
                cPrice.ServiceName = com.OBK_Ref_PriceList.NameRu;
                cPrice.ProductName = com.OBK_RS_Products.NameRu;
                result.Add(cPrice);
                i++;
            }
            return Json(new { isSuccess = true, result });
        }
        [HttpGet]
        public ActionResult GetSignDirectionToPayment(Guid id)
        {
            var signData = payRepo.GetSignData(id);
            return Json(new { data = signData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSignedPayment(Guid paymentId, string signedData)
        {
            payRepo.SaveSignPay(paymentId, signedData);
            return Json("Ok");
        }

        public ActionResult GetEmployee()
        {
            return Json(new {UserHelper.GetCurrentEmployee().Id}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DocumentExportFilePdf(string contractId)
        {
            string name = "Счет на оплату.pdf";
            StiReport report = new StiReport();
            try
            {
                report.Load(Server.MapPath("~/Reports/Mrts/OBK/1c/ObkInvoicePayment.mrt"));
                foreach (var data in report.Dictionary.Databases.Items.OfType<StiSqlDatabase>())
                {
                    data.ConnectionString = UserHelper.GetCnString();
                }

                report.Dictionary.Variables["ContractId"].ValueObject = contractId;
                //итого
                var totalPriceWithCount = payRepo.GetTotalPriceCount(Guid.Parse(contractId));
                report.Dictionary.Variables["TotalPriceWithCount"].ValueObject = totalPriceWithCount;
                //в том числе НДС
                var totalPriceNDS = payRepo.GetTotalPriceNDS(totalPriceWithCount);
                report.Dictionary.Variables["TotalPriceNDS"].ValueObject = totalPriceNDS;
                //прописью
                var priceText = RuDateAndMoneyConverter.CurrencyToTxtTenge(Convert.ToDouble(totalPriceWithCount), false);
                report.Dictionary.Variables["TotalPriceWithCountName"].ValueObject = priceText;
                report.Dictionary.Variables["ChiefAccountant"].ValueObject = payRepo.GetEmpoloyee(Guid.Parse("E1EE3658-0C35-41EB-99FD-FDDC4D07CEC4"));
                report.Dictionary.Variables["Executor"].ValueObject = payRepo.GetEmpoloyee(Guid.Parse("55377FAC-A5F0-4093-BBB6-18BD28E53BE1"));
                report.Render(false);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("ex: " + ex.Message + " \r\nstack: " + ex.StackTrace);
            }
            var stream = new MemoryStream();
            report.ExportDocument(StiExportFormat.Word2007, stream);
            stream.Position = 0;
            return File(stream, "application/pdf", name);
        }

        public class ContractPrice
        {
            public int Line { get; set; }
            public string ServiceName { get; set; }
            public string ProductName { get; set; }
            public int Count { get; set; }
            public double PriceWithTax { get; set; }
            public double Price { get; set; }
        }
    }
}