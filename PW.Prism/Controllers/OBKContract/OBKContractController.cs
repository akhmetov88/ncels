using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models.OBK;
using PW.Ncels.Database.Repository.Common;
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
            IQueryable<OBK_ContractRegisterView> query = obkRepo.GetContracts();
            var xxx = Json(query.ToDataSourceResult(request));
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

            var contractTypes = db.OBK_Ref_Type.Where(x => x.ViewOption == CodeConstManager.OBK_VIEW_OPTION_SHOW_ON_CREATE).OrderBy(x => x.Id).Select(o => new { o.Id, Name = o.NameRu, o.Code, o.NameKz });


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

            var obkContract = db.OBK_Contract.Where(x => x.Id == id).FirstOrDefault();
            obkContract.ObkRsProductCount = productInfo.Count;
            ViewBag.Contract = obkContract;
            ViewBag.ContractTypes = new SelectList(contractTypes, "Id", "Name", obkContract.Type);
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
            ViewData["Courrency"] = new SelectList(currencies, "Id", "Name");
            ViewBag.declarant = declarant;
            ViewBag.productInfo = productInfo;
            ViewBag.prices = prices;

            ViewBag.ShowProductComments = true;
            ViewBag.HidePriceAndCurrency = true;
            #region Attachments
            var repository = new UploadRepository();
            string type = "";
            if (declarant.IsResident)
            {
                type = CodeConstManager.ATTACH_CONTRACT_FILE_RESIDENT;
            }
            else
            {
                type = CodeConstManager.ATTACH_CONTRACT_FILE_NON_RESIDENT;
            }
            var list = repository.GetAttachListEdit(id, type);
            ViewBag.ListAttachments = list;
            #endregion


            ViewBag.ShowMeetsRequirementsBtn = IsMeetsRequirementsBtnAllowed(id.Value);
            ViewBag.ShowDoesNotMeetRequirementsBtn = ViewBag.ShowMeetsRequirementsBtn;
            ViewBag.ShowReturnToApplicantBtn = true;
            ViewBag.ShowSendToBossForApprovalBtn = true;



            return PartialView("Contract", contract);
        }

        private bool IsMeetsRequirementsBtnAllowed(Guid contractId)
        {
            bool result = false;

            if (EmployePermissionHelper.CanViewMeetAndNotMeetRqrmntsBtnObkContract)
            {
                var employeeId = UserHelper.GetCurrentEmployee().Id;

                var stages = db.OBK_ContractRegisterView.Where(x => x.ContractId == contractId && x.ExecutorId == employeeId && x.StageStatusCode == OBK_Ref_StageStatus.InWork);

                bool coz = false;
                Guid cozStageId = Guid.Empty;
                foreach (var item in stages)
                {
                    if (item.ContractStageStageId == CodeConstManager.STAGE_OBK_COZ)
                    {
                        coz = true;
                        cozStageId = item.ContractStageId;
                        break;
                    }
                }

                if (coz && cozStageId != Guid.Empty)
                {
                    var stageObk = db.OBK_ContractStage.Where(x => x.Id == cozStageId).FirstOrDefault();
                    var childStages = db.OBK_ContractStage.Where(x => x.ParentStageId == cozStageId);

                    int notFinishedCount = 0;
                    foreach (var childStage in childStages)
                    {
                        if (childStage.ResultId == 0 || childStage.ResultId == null)
                        {
                            notFinishedCount++;
                        }
                    }

                    if (notFinishedCount == 0)
                    {
                        if (stageObk.ResultId == null && stageObk.ResultId == 0)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    Guid stageId = Guid.Empty;
                    foreach (var item in stages)
                    {
                        if (item.ContractStageStageId == CodeConstManager.STAGE_OBK_UOBK)
                        {
                            stageId = item.ContractStageId;
                            break;
                        }
                    }
                    if (stageId != Guid.Empty)
                    {
                        var stage = db.OBK_ContractStage.Where(x => x.Id == stageId).FirstOrDefault();
                        if (stage.ResultId == null || stage.ResultId == 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
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

        [HttpPost]
        public ActionResult SaveCommentProduct(string productId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            obkRepo.SaveCommentProduct(productId, isError, comment, fieldValue, userId, fieldDisplay);

            return Json(new { Success = true });
        }

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
    
        [HttpPost]
        public ActionResult SaveCommentProductsSerie(string productSerieId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            obkRepo.SaveCommentProductsSerie(productSerieId, isError, comment, fieldValue, userId, fieldDisplay);

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

        [HttpGet]
        public ActionResult SetExecutor()
        {
            return PartialView(Guid.NewGuid());
        }

        [HttpPost]
        public ActionResult SetExecutor(Guid executorId, Guid stageId)
        {
            obkRepo.SendToWork(stageId, executorId);
            return Json("OK");
        }
    }
}