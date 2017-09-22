using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Repository.OBK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PW.Prism.Controllers.OBKContract
{
    public class OBKContractController : Controller
    {
        private ncelsEntities db = UserHelper.GetCn();
        private OBKContractRepository obkRepo;

        public OBKContractController()
        {
            obkRepo = new OBKContractRepository();
        }

        // GET: OBKContract
        public ActionResult Index()
        {
            return PartialView(Guid.NewGuid());
        }

        public ActionResult ListContract([DataSourceRequest] DataSourceRequest request)
        {
            List<OBKContractViewModel> query = obkRepo.GetContracts();
            return Json(query.ToDataSourceResult(request));
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Document document = db.Documents.Find(id);
            //if (document == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.UiId = Guid.NewGuid().ToString();

            var countries = db.Dictionaries.Where(x => x.Type == "Country").ToList();
            var organizationForms = db.Dictionaries.Where(x => x.Type == "OpfType").ToList();
            var docTypes = db.Dictionaries.Where(x => x.Type == "OBKContractDocumentType").ToList();
            var currencies = db.Dictionaries.Where(x => x.Type == "Currency").ToList();


            var contract = obkRepo.LoadContract(id.Value);
            var declarant = obkRepo.GetDeclarant(id.Value);
  
            //declarant.NameEn
            //

            var productInfo = obkRepo.GetProducts(id.Value);
            var prices = obkRepo.GetContractPrices(id.Value);

            ViewBag.Countries = new SelectList(countries, "Id", "Name", declarant.CountryId);
            ViewBag.OrganizationForms = new SelectList(organizationForms, "Id", "Name", declarant.OrganizationFormId);
            Guid selectedNonResident = Guid.Empty;
            if (declarant.IsConfirmed)
            {
                selectedNonResident = declarant.Id.Value;
            }
            ViewBag.NamesNonResidents = new SelectList((IEnumerable<object>)obkRepo.GetNamesNonResidents(declarant.CountryId), "Id", "Name", selectedNonResident);
            ViewBag.DocTypes = new SelectList(docTypes, "Id", "Name", contract.BossDocType);
            ViewBag.BoolValues = new SelectList(new List<SelectListItem> {
                new SelectListItem { Selected = false, Text="Нет", Value = false.ToString()},
                new SelectListItem { Selected = false, Text="Да", Value = true.ToString()},
            }, "Value", "Text", contract.IsHasBossDocNumber.ToString());
            ViewBag.Currencies = new SelectList(currencies, "Id", "Name", contract.CurrencyId);
            ViewBag.declarant = declarant;
            ViewBag.productInfo = productInfo;
            ViewBag.prices = prices;

            return PartialView("Contract", contract);
        }

        public ActionResult ListAttaches([DataSourceRequest] DataSourceRequest request, Guid contractId)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.FileLinkViews.Include("FileCategory").AsNoTracking().Where(e => e.DocumentId == contractId);
            return Json(query.ToDataSourceResult(request, c => new
            {
                c.Id,
                c.Version,
                c.CreateDate,
                c.Category,
                c.FileName,
                c.FilePath,
                c.CategoryId,
                c.DocumentId,
                c.ParentFileName
            }));
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

            //var repository = new SafetyAssessmentRepository();
            //var model = repository.GetComments(modelId, idControl);
            //if (model == null)
            //{
            //    model = new OBK_AssessmentDeclarationCom();
            //}
            //model.ObkAssessmentDeclarationFieldHistories = repository.GetFieldHistories(modelId, idControl);
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView(model);
            //}

            //return View(model);
        }
        [HttpPost]
        public virtual ActionResult SaveComment(string modelId, string idControl, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            obkRepo.SaveComment(modelId, idControl, isError, comment, fieldValue, userId, fieldDisplay);

            //new SafetyAssessmentRepository().SaveComment(modelId, idControl, isError, comment, fieldValue, userId, fieldDisplay);

            return Json(new { Success = true });

        }

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

        [HttpPost]
        public virtual ActionResult SaveCommentPrice(string contractPriceId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            obkRepo.SaveCommentPrice(contractPriceId, isError, comment, fieldValue, userId, fieldDisplay);

            return Json(new { Success = true });
        }

        public ActionResult RequestView()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetContractPrices(Guid contractId)
        {
            var list = obkRepo.GetContractPrices(contractId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}