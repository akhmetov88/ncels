using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Repository.Common;
using PW.Ncels.Database.Repository.Expertise;
using PW.Ncels.Database.Repository.OBK;
using PW.Ncels.Database.Repository.Security;
using PW.Prism.ViewModels.Expertise;

namespace PW.Prism.Controllers.OBK
{
    [Authorize]
    public class SafetyAssessmentController : PrimsSafetyAssessmentController
    {
        public ActionResult ListRegister([DataSourceRequest] DataSourceRequest request, string type, int stage, DeclarationRegistryFilter customFilter = null)
        {
            var stageName = ExpStageNameHelper.GetName(stage);
            ActionLogger.WriteInt(stageName + ": Получение списка заявлений");
            var list =
                new SafetyAssessmentRepository().SafetyAssessmentRegisterList(type, stage,
                    UserHelper.GetCurrentEmployee().Id, customFilter);
            var result = list.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult Design(Guid[] id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = GetAssessmentStage(id[0]);
            model.OBK_AssessmentDeclaration.Applicant = new EmployeesRepository().GetById(model.OBK_AssessmentDeclaration.EmployeeId);
            //ViewBag.CanRefuseExpertise =
            //    model.StageId == CodeConstManager.STAGE_PRIMARY /*&& model.EXP_DIC_StageStatus.Code != EXP_DIC_StageStatus.Completed*/ &&
            //    model.OBK_AssessmentDeclaration.StatusId != CodeConstManager.STATUS_EXP_ON_REFUSING_ID
            //    && model.OBK_AssessmentDeclaration.StatusId != CodeConstManager.STATUS_EXP_REFUSED_ID;
            FillDeclarationControl(model.OBK_AssessmentDeclaration);
            var stageName = ExpStageNameHelper.GetName(model.StageId);
            ActionLogger.WriteInt(stageName + ": Получение заявления №" + model.OBK_AssessmentDeclaration.Number);
            return PartialView(model);
        }

        /// <summary>
        /// прикрепленные файлы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AttachFileView(Guid? id)
        {
            var model = GetAssessmentDeclaration(id.ToString());

            if (model.Type_Id == int.Parse(CodeConstManager.OBK_SA_SERIAL))
            {
                var repository = new UploadRepository();
                var list = repository.GetAttachListEdit(id, CodeConstManager.ATTACH_SERIAL_SA_FILE_CODE);
                return PartialView(list);
            }
            if (model.Type_Id == int.Parse(CodeConstManager.OBK_SA_PARTY)) {
                var repository = new UploadRepository();
                var list = repository.GetAttachListEdit(id, CodeConstManager.ATTACH_PARTY_SA_FILE_CODE);
                return PartialView(list);
            }
            if (model.Type_Id == int.Parse(CodeConstManager.OBK_SA_DECLARATION))
            {
                var repository = new UploadRepository();
                var list = repository.GetAttachListEdit(id, CodeConstManager.ATTACH_DECLARATION_SA_FILE_CODE);
                return PartialView(list);
            }
            return PartialView(null);
        }

        [HttpPost]
        public virtual ActionResult GetContract(Guid id)
        {
            var contract = new SafetyAssessmentRepository().GetContractById(id);

            if (contract == null)
            {
                return Json(new { isSuccess = false });
            }
            var products = new SafetyAssessmentRepository().GetRsProductsAndSeries(contract.Id);

            var result = new OBK_AssessmentDeclaration();

            var resultProducts = new List<OBK_RS_Products>();
            foreach (var product in products)
            {
                var prod = new OBK_RS_Products();
                prod.Id = product.Id;
                prod.NameRu = product.NameRu;
                prod.NameKz = product.NameKz;
                prod.ProducerNameRu = product.ProducerNameRu;
                prod.ProducerNameKz = product.ProducerNameKz;
                prod.CountryNameRu = product.CountryNameRu;
                prod.CountryNameKZ = product.CountryNameKZ;
                prod.TnvedCode = product.TnvedCode;
                prod.KpvedCode = product.KpvedCode;
                prod.Price = product.Price;
                foreach (var productSeries in product.OBK_Procunts_Series)
                {
                    var prodSeries = new OBK_Procunts_Series();
                    prodSeries.Id = productSeries.Id;
                    prodSeries.Series = productSeries.Series;
                    prodSeries.SeriesStartdate = productSeries.SeriesStartdate;
                    prodSeries.SeriesEndDate = productSeries.SeriesEndDate;
                    prodSeries.SeriesParty = productSeries.SeriesParty;
                    prod.OBK_Procunts_Series.Add(prodSeries);
                }
                resultProducts.Add(prod);
            }
            result.ObkRsProductses = resultProducts;
            return Json(new { isSuccess = true, result });
        }

        /// <summary>
        /// вренуть в работу заявителю
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Reject(Guid id, string note)
        {
            var model = GetAssessmentStage(id);
            model.OBK_AssessmentDeclaration.DesignNote = note;
            return PartialView(model);
        }
        /// <summary>
        /// вренуть в работу заявителю
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RejectConfirm(Guid? id, string note)
        {
            if (id != null)
            {
                var assessmentStage = GetAssessmentStage(id);
                var model = assessmentStage.OBK_AssessmentDeclaration;
                if (model != null)
                {
                    model.DesignNote = note;
                    model.DesignDate = DateTime.Now;
                    model.StatusId = CodeConstManager.STATUS_REJECT_ID;
                    new SafetyAssessmentRepository().Update(model);
                    var history = new OBK_AssessmentDeclarationHistory()
                    {
                        DateCreate = DateTime.Now,
                        AssessmentDeclarationId = model.Id,
                        StatusId = model.StatusId,
                        UserId = UserHelper.GetCurrentEmployee().Id,
                        Note = model.DesignNote
                    };
                    new SafetyAssessmentRepository().SaveHisotry(history, UserHelper.GetCurrentEmployee().Id);
                }
            }
            return Json("Ok!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmeRefuseSafety(Guid stageId)
        {
            return PartialView(stageId);
        }
        public ActionResult RefuseInSafety(Guid stageId)
        {
            var repo = new AssessmentStageRepository();
            repo.StartRefuseInSafety(stageId, true);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// назначить исполнителя
        /// </summary>
        /// <param name="id">список заявлении</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetExecuter()
        {
            return PartialView(Guid.NewGuid());
        }
        [HttpPost]
        public ActionResult SetExecuter(Guid[] stages, Guid[] executors)
        {
            var okbExecutor = new SafetyAssessmentRepository();
            okbExecutor.SendToWork(stages, executors);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}