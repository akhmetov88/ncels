using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Repository.OBK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Aspose.Words;
using Ncels.Helpers;
using PW.Ncels.Database.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.Repository.Common;

namespace PW.Ncels.Controllers
{
    [Authorize]
    public class OBKContractController : Controller
    {
        OBKContractRepository obkRepo;
        public OBKContractController()
        {
            obkRepo = new OBKContractRepository();
        }

        // GET: OBKContract
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContractAddition()
        {
            return View();
        }

        public ActionResult Contract(Guid? id, string listAction)
        {
            //ViewBag.ListAction = listAction;
            ViewBag.ListAction = "Index";
            return View(id);
        }

        public ActionResult LoadContract(Guid id)
        {
            OBKContractViewModel contract = obkRepo.LoadContract(id);
            return Json(contract, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchDrug(int regType, string drugNumber, string drugTradeName, bool drugEndDateExpired)
        {
            var list = obkRepo.GetSearchReestr(regType, drugNumber, drugTradeName, drugEndDateExpired);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContractSave(Guid Guid, OBKContractViewModel contractViewModel)
        {
            OBKContractViewModel savedContract = obkRepo.SaveContract2(Guid, contractViewModel);
            return Json(savedContract);
        }

        /// <summary>
        /// Получение списка договоров
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReadContract(ModelRequest request)
        {
            return Json(await obkRepo.GetContractList(request, true), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrganizationData(Guid guid)
        {
            object declarantJson = null;
            var organization = obkRepo.GetOrganizationData(guid);
            if (organization != null)
            {
                declarantJson = GetDeclarantJson(organization);
            }

            //var declarantContact = organization.OBK_DeclarantContact.OrderByDescending(x => x.CreateDate).FirstOrDefault();

            //var declarantJson = new
            //{
            //    organization.OrganizationFormId,
            //    organization.IsResident,
            //    organization.NameKz,
            //    organization.NameRu,
            //    organization.NameEn,
            //    organization.CountryId,
            //    declarantContact.AddressLegalRu,
            //    declarantContact.AddressLegalKz,
            //    declarantContact.AddressFact,
            //    declarantContact.Phone,
            //    declarantContact.Email,
            //    declarantContact.BossLastName,
            //    declarantContact.BossFirstName,
            //    declarantContact.BossMiddleName,
            //    declarantContact.BossPosition,
            //    declarantContact.BossDocType,
            //    declarantContact.IsHasBossDocNumber,
            //    declarantContact.BossDocNumber,
            //    declarantContact.BossDocCreatedDate,
            //    declarantContact.BossDocEndDate,
            //    declarantContact.BossDocUnlimited,
            //    declarantContact.SignLastName,
            //    declarantContact.SignFirstName,
            //    declarantContact.SignMiddleName,
            //    declarantContact.SignPosition,
            //    declarantContact.SignDocType,
            //    declarantContact.IsHasSignDocNumber,
            //    declarantContact.SignDocNumber,
            //    declarantContact.SignDocCreatedDate,
            //    declarantContact.SignDocEndDate,
            //    declarantContact.SignDocUnlimited,
            //    declarantContact.BankIik,
            //    declarantContact.BankBik,
            //    declarantContact.CurrencyId,
            //    declarantContact.BankNameRu,
            //    declarantContact.BankNameKz
            //};

            return Json(declarantJson, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetService(Guid service)
        {
            var serviceObj = obkRepo.GetService(service);
            return Json(serviceObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProduct(Guid contractId, OBKContractProductViewModel product)
        {
            OBKContractProductViewModel productInfo = obkRepo.SaveProduct(contractId, product);
            return Json(productInfo);
        }

        [HttpPost]
        public ActionResult DeleteProduct(Guid contractId, int productId)
        {
            var deleteState = obkRepo.DeleteProduct(contractId, productId);
            return Json(deleteState);
        }

        [HttpGet]
        public ActionResult GetProducts(Guid contractId)
        {
            var list = obkRepo.GetProducts(contractId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveContractPrice(Guid contractId, OBKContractServiceViewModel service)
        {
            OBKContractServiceViewModel serviceInfo = obkRepo.SaveContractPrice(contractId, service);
            return Json(serviceInfo);
        }

        [HttpPost]
        public ActionResult DeleteContractPrice(Guid contractId, Guid? serviceId)
        {
            var deleteState = obkRepo.DeleteContractPrice(contractId, serviceId);
            return Json(deleteState);
        }

        [HttpGet]
        public ActionResult GetContractPrices(Guid contractId)
        {
            var list = obkRepo.GetContractPrices(contractId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FindDeclarant(string bin)
        {
            if (bin != null && bin.Length == 12)
            {
                var organization = obkRepo.FindOrganization(bin);
                if (organization != null)
                {
                    var declarantJson = GetDeclarantJson(organization);
                    return Json(declarantJson, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        private object GetDeclarantJson(OBK_Declarant organization)
        {
            if (organization != null)
            {
                var declarantContact = organization.OBK_DeclarantContact.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                if (declarantContact != null)
                {
                    var declarantJson = new
                    {
                        organization.Id,
                        organization.OrganizationFormId,
                        organization.IsResident,
                        organization.NameKz,
                        organization.NameRu,
                        organization.NameEn,
                        organization.CountryId,
                        declarantContact.AddressLegalRu,
                        declarantContact.AddressLegalKz,
                        declarantContact.AddressFact,
                        declarantContact.Phone,
                        declarantContact.Email,
                        declarantContact.BossLastName,
                        declarantContact.BossFirstName,
                        declarantContact.BossMiddleName,
                        declarantContact.BossPosition,
                        declarantContact.BossPositionKz,
                        declarantContact.BossDocType,
                        declarantContact.IsHasBossDocNumber,
                        declarantContact.BossDocNumber,
                        declarantContact.BossDocCreatedDate,
                        declarantContact.BossDocEndDate,
                        declarantContact.BossDocUnlimited,
                        declarantContact.SignerIsBoss,
                        declarantContact.SignLastName,
                        declarantContact.SignFirstName,
                        declarantContact.SignMiddleName,
                        declarantContact.SignPosition,
                        declarantContact.SignPositionKz,
                        declarantContact.SignDocType,
                        declarantContact.IsHasSignDocNumber,
                        declarantContact.SignDocNumber,
                        declarantContact.SignDocCreatedDate,
                        declarantContact.SignDocEndDate,
                        declarantContact.SignDocUnlimited,
                        declarantContact.BankIik,
                        declarantContact.BankBik,
                        declarantContact.CurrencyId,
                        declarantContact.BankNameRu,
                        declarantContact.BankNameKz
                    };

                    return declarantJson;
                }
                else
                {
                    var declarantJson = new
                    {
                        organization.Id,
                        organization.OrganizationFormId,
                        organization.IsResident,
                        organization.NameKz,
                        organization.NameRu,
                        organization.NameEn,
                        organization.CountryId
                    };
                    return declarantJson;
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult ContractDeclarantSave(Guid guid, OBKDeclarantViewModel declarantViewModel)
        {
            bool exist;
            Guid? declarantId = null;
            var declarant = obkRepo.ContractDeclarantSave(guid, declarantViewModel, out exist);
            if (declarant != null)
            {
                declarantId = declarant.Id;
            }
            object returnValue = new { Exist = exist, DeclarantId = declarantId, IsResident = declarantViewModel.IsResident };
            return Json(returnValue);
        }

        [HttpPost]
        public ActionResult SaveDeclarantId(Guid contractId, Guid declarantId)
        {
            obkRepo.SaveDeclarant(contractId, declarantId);
            return Json(true);
        }

        [HttpPost]
        public ActionResult RemoveDeclarantId(Guid contractId)
        {
            obkRepo.DeleteDeclarant(contractId);
            return Json(true);
        }

        [HttpGet]
        public ActionResult GetNamesNonResidents(Guid? countryId)
        {
            var list = obkRepo.GetNamesNonResidents(countryId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ClearContactData(Guid contractId)
        {
            obkRepo.ClearContactData(contractId);
            return Json(true);
        }

        public ActionResult GetDeclarant(Guid contractId)
        {
            OBKDeclarantViewModel declarant = obkRepo.GetDeclarant(contractId);
            return Json(declarant, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDrugForms(int productId)
        {
            List<OBKDrugFormViewModel> drugForms = obkRepo.GetDrugForms(productId);
            return Json(drugForms, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMtParts(int productId)
        {
            List<OBKMtPartViewModel> mtParts = obkRepo.GetMtParts(productId);
            return Json(mtParts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendContractInProcessing(Guid contractId)
        {
            var state = obkRepo.SendContractInProcessing(contractId);
            return Json(state);
        }

        public ActionResult ContractTemplate(Guid id)
        {
            return PartialView(id);
        }

        public ActionResult GetContractTemplatePdf(Guid id)
        {
            var db = new ncelsEntities();
            string name = "Договор_на_проведение_оценки_безопасности_и_качества.pdf";
            StiReport report = new StiReport();
            try
            {
                report.Load(obkRepo.GetContractTemplatePath(id));//(Server.MapPath("~/Reports/Mrts/OBK/ObkContractDec.mrt"));
                foreach (var data in report.Dictionary.Databases.Items.OfType<StiSqlDatabase>())
                {
                    data.ConnectionString = UserHelper.GetCnString();
                }

                report.Dictionary.Variables["ContractId"].ValueObject = id;

                var price = new OBKContractRepository().GetPriceCount(id);
                report.Dictionary.Variables["PriceCount"].ValueObject = price;

                var priceText = RuDateAndMoneyConverter.ToTextTenge(Convert.ToDouble(price), false);
                report.Dictionary.Variables["PriceCountName"].ValueObject = priceText;

                report.Render(false);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("ex: " + ex.Message + " \r\nstack: " + ex.StackTrace);
            }
            Stream stream = new MemoryStream();
            report.ExportDocument(StiExportFormat.Pdf, stream);
            stream.Position = 0;

            try
            {
                var signData = db.OBK_ContractSignedDatas.Where(x => x.ContractId == id).FirstOrDefault();
                if (signData != null && signData.ApplicantSign != null && signData.CeoSign != null)
                {
                    Aspose.Words.Document doc = new Aspose.Words.Document(stream);
                    doc.InserQrCodesToEnd("ApplicantSign", signData.ApplicantSign);
                    doc.InserQrCodesToEnd("CeoSign", signData.CeoSign);
                }
            }
            catch (Exception ex)
            {

            }

            return new FileStreamResult(stream, "application/pdf");
        }

        public ActionResult GetSigners()
        {
            var signers = obkRepo.GetSigners();
            return Json(signers.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExpertOrganizations()
        {
            var expertOrganizations = obkRepo.GetExpertOrganizations();
            return Json(expertOrganizations.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowComment(Guid modelId, string idControl)
        {
            var model = obkRepo.GetComments(modelId, idControl);
            if (model == null)
            {
                model = new OBK_ContractCom();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ShowCommentPrice(Guid contractPriceId)
        {
            var model = obkRepo.GetCommentsPrice(contractPriceId);
            if (model == null)
            {
                model = new OBK_ContractPriceCom();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ShowCommentProduct(int productId)
        {
            var model = obkRepo.GetCommentsProduct(productId);
            if (model == null)
            {
                model = new OBK_RS_ProductsCom();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ShowCommentProductsSerie(int productSerieId)
        {
            var model = obkRepo.GetCommentsProductsSerie(productSerieId);
            if (model == null)
            {
                model = new OBK_Products_SeriesCom();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult SignOperation(string id)
        {
            var IsSuccess = true;
            var preambleXml = "sss";
            return Json(new
            {
                IsSuccess,
                preambleXml
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignData(Guid id)
        {
            return Json(new
            {
                IsSuccess = true,
                preambleXml = obkRepo.GetDataForSign(id)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignContract(Guid contractId, string signedData)
        {
            var status = obkRepo.SignContractApplicant(contractId, signedData);

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHistory(Guid? modelId)
        {
            var result = obkRepo.GetExtHistory(modelId);

            return Json(new { isSuccess = true, result });
        }
    }
}