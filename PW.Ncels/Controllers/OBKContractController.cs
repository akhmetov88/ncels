using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Repository.OBK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
                        declarantContact.BossDocType,
                        declarantContact.IsHasBossDocNumber,
                        declarantContact.BossDocNumber,
                        declarantContact.BossDocCreatedDate,
                        declarantContact.BossDocEndDate,
                        declarantContact.BossDocUnlimited,
                        declarantContact.SignLastName,
                        declarantContact.SignFirstName,
                        declarantContact.SignMiddleName,
                        declarantContact.SignPosition,
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
            var declarant = obkRepo.ContractDeclarantSave(guid, declarantViewModel);
            return Json(declarant.Id);
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
            var drugForms = obkRepo.GetDrugForms(productId);
            return Json(drugForms, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMtParts(int productId)
        {
            var mtParts = obkRepo.GetMtParts(productId);
            return Json(mtParts, JsonRequestBehavior.AllowGet);
        }
    }
}