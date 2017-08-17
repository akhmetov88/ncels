using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Ncels.Helpers;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.Expertise;
using PW.Ncels.Database.Repository.Expertise;

namespace PW.Ncels.Database.Repository.OBK
{
    /// <summary>
    /// 
    /// </summary>
    public class SafetyAssessmentRepository : ARepository
    {

        public SafetyAssessmentRepository()
        {
            AppContext = CreateDatabaseContext();
        }

        public SafetyAssessmentRepository(bool isProxy)
        {
            AppContext = CreateDatabaseContext(isProxy);
        }

        public SafetyAssessmentRepository(ncelsEntities context) : base(context)
        {
        }

        /// <summary>
        /// Получение списка контрактов
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public IQueryable<OBK_Contract> GetContractsByStatuses(Guid employeeId)
        {
            return AppContext.OBK_Contract.Where(e => e.EmployeeId == employeeId);
        }

        /// <summary>
        /// Показать список c проставлением дополнительной информации по владельцу
        /// </summary>
        /// <param name="employeeId">владелец</param>
        /// <returns></returns>
        public IEnumerable<OBK_Contract> GetActiveContractListWithInfo(Guid employeeId)
        {
            var list = GetContractsByStatuses(employeeId).OrderBy(e => e.StartDate);

            foreach (var contract in list)
            {
                var buider = new StringBuilder("№" + contract.Number);
                buider.Append("; Дата:" + contract.CreatedDate.ToShortDateString());
                contract.ContractInfo = buider.ToString();
            }
            return list;
        }

        /// <summary>
        /// Поиск договора по id
        /// </summary>
        public OBK_Contract GetContractById(Guid? id)
        {
            return AppContext.OBK_Contract.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Справочник Тип заявления ОБК
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OBK_Ref_Type> GetObkRefTypes()
        {
            return AppContext.OBK_Ref_Type.ToList();
        }

        public OBK_Ref_Type GetObkRefTypes(string type)
        {
            return AppContext.OBK_Ref_Type.FirstOrDefault(e => e.Code == type);
        }

        /// <summary>
        /// Список стран
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dictionary> GetCounties()
        {
            return AppContext.Dictionaries.Where(o => o.Type == "Country").ToList();
        }

        /// <summary>
        /// валюта
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dictionary> GetObkCurrencies()
        {
            return AppContext.Dictionaries.Where(o => o.Type == "Currency").ToList();
        }

        public OBK_AssessmentDeclaration GetById(string modelId)
        {
            return AppContext.OBK_AssessmentDeclaration.FirstOrDefault(e => e.Id == new Guid(modelId));
        }

        public IEnumerable<OBK_RS_Products> GetRsProductsAndSeries(Guid modelId)
        {
            return AppContext.OBK_RS_Products.Where(e => e.ContractId == modelId)
                .Include(x => x.OBK_Procunts_Series)
                .ToList();
        }

        public ReesrtObk GetSearchReestr(string regNumber, string tradeName, string manufacturer, string mnn)
        {
            var reestr = AppContext.sr_register.FirstOrDefault(x => x.reg_number == regNumber);
            if (reestr != null)
            {
                return AppContext.ReesrtObks.FirstOrDefault(x => x.register_id == reestr.id);
            }
            return null;
        }

        public IEnumerable<Dictionary> GetOrganizationForm()
        {
            return AppContext.Dictionaries.Where(x => x.Type == "OpfType").ToList();
        }

        /// <summary>
        /// Поиск органищация по id
        /// </summary>
        public OBK_Declarant GetDeclarantById(Guid? id)
        {
            return AppContext.OBK_Declarant.FirstOrDefault(e => e.Id == id);
        }
        /// <summary>
        /// Поиск контактов органищация по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OBK_DeclarantContact GetDeclarantContactById(Guid? id)
        {
            return AppContext.OBK_DeclarantContact.FirstOrDefault(e => e.Id == id);
        }
        
        /// <summary>
        /// загрузка списка заявлений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isRegisterProject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> GetSafetyAssessmentDeclarationList(ModelRequest request, bool isRegisterProject,
            int? type)
        {
            try
            {
                //Database query
                var employeeId = UserHelper.GetCurrentEmployee().Id;
                var org = UserHelper.GetCurrentEmployee();
                var v = type != null
                    ? AppContext.OBK_AssessmentDeclarationView.Where(m => m.OwnerId == employeeId)
                        .OrderByDescending(m => m.SortDate)
                        .AsQueryable()
                    : AppContext.OBK_AssessmentDeclarationView.Where(m => m.OwnerId == employeeId).AsQueryable();
                //search
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    v =
                        v.Where(
                            a => a.Number.Contains(request.SearchValue) || a.StausName.Contains(request.SearchValue));
                }

                //sort
                if (!(string.IsNullOrEmpty(request.SortColumn) && string.IsNullOrEmpty(request.SortColumnDir)))
                {
                    v = v.OrderBy(request.SortColumn + " " + request.SortColumnDir);
                }


                int recordsTotal = await v.CountAsync();
                var expertiseViews = v.Skip(request.Skip).Take(request.PageSize);
                return
                    new
                    {
                        draw = request.Draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        Data = await expertiseViews.ToListAsync()
                    };
            }

            catch (Exception e)
            {
                return new {IsError = true, Message = e.Message};
            }

        }

        /// <summary>
        /// Cохранение изменений
        /// </summary>
        /// <param name="code"></param>
        /// <param name="modelId">id</param>
        /// <param name="userId">id пользователя</param>
        /// <param name="recordId">id записи</param>
        /// <param name="fieldName">наименование поля</param>
        /// <param name="fieldValue">значение</param>
        /// <param name="fieldDisplay">значение</param>
        /// <returns></returns>
        public SubUpdateField UpdateModel(string code, int typeId, string modelId, string userId, long? recordId,
            string fieldName, string fieldValue, string fieldDisplay)
        {
            bool isNew = false;
            var model = GetById(modelId);
            if (model == null)
            {
                model = new OBK_AssessmentDeclaration
                {
                    EmployeeId = UserHelper.GetCurrentEmployee().Id,
                    Type_Id = GetObkRefTypes(typeId.ToString()).Id,
                    Id = new Guid(modelId),
                    CreatedDate = DateTime.Now,
                    StatusId = CodeConstManager.STATUS_DRAFT_ID,
                    CertificateDate = DateTime.Now,
                    IsDeleted = false
                };
                isNew = true;
            }

            switch (code)
            {
                case "main":
                {
                    return UpdateMain(isNew, model, fieldName, fieldValue, userId, fieldDisplay);
                }
            }
            return null;
        }

        private SubUpdateField UpdateMain(bool isNew, OBK_AssessmentDeclaration model, string fieldName,
            string fieldValue, string userId, string fieldDisplay)
        {
            var property = model.GetType().GetProperty(fieldName);
            if (property != null)
            {
                var t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                object safeValue;
                if (string.IsNullOrEmpty(fieldValue))
                {
                    fieldValue = null;
                }
                if (t == typeof(Guid))
                {
                    safeValue = fieldValue == null ? null : Convert.ChangeType(new Guid(fieldValue), t);
                }
                else
                {
                    safeValue = fieldValue == null ? null : Convert.ChangeType(fieldValue, t);
                }
                property.SetValue(model, safeValue, null);
            }
            if (isNew)
            {
                AppContext.OBK_AssessmentDeclaration.Add(model);
                AppContext.SaveChanges();
            }
            SaveHistoryField(model.Id, fieldName, fieldValue, new Guid(userId), fieldDisplay);
            var subUpdateField = new SubUpdateField();
            subUpdateField.ModelId = model.ObjectId;

            return subUpdateField;
        }


        /// <summary>
        /// Сохранение истории поле
        /// </summary>
        /// <param name="modelId">ид заявление</param>
        /// <param name="fieldName">изменяемое поле</param>
        /// <param name="fieldValue">значение</param>
        /// <param name="userId">автор</param>
        /// <param name="fieldDisplay"></param>
        protected void SaveHistoryField(Guid modelId, string fieldName, string fieldValue, Guid userId,
            string fieldDisplay)
        {
            var history = new OBK_AssessmentDeclarationFieldHistory
            {
                AssessmentDeclarationId = modelId,
                ControlId = fieldName,
                UserId = userId,
                ValueField = fieldValue,
                DisplayField = fieldDisplay,
                CreateDate = DateTime.Now
            };
            AppContext.OBK_AssessmentDeclarationFieldHistory.Add(history);
            AppContext.SaveChanges();
        }

        /// <summary>
        /// удаление записи
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        public void DeleteReport(string id, Guid guid)
        {
            var model = GetById(id);
            model.IsDeleted = true;
            AppContext.SaveChanges();
        }

        /// <summary>
        /// создание дубликата
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public OBK_AssessmentDeclaration DublicateAssessmentDeclaration(string id, Guid guid)
        {
            var oldModel = GetById(id);
            if (oldModel == null)
            {
                return null;
            }
            var model = new OBK_AssessmentDeclaration
            {
                Id = Guid.NewGuid(),
                EmployeeId = guid,
                StatusId = CodeConstManager.STATUS_DRAFT_ID,
                CreatedDate = DateTime.Now,
                CertificateGMP = oldModel.CertificateGMP,
                CertificateNumber = oldModel.CertificateNumber,
                AssuranceCheck = oldModel.AssuranceCheck,
                OrderCheck = oldModel.OrderCheck,
                StabilityCheck = oldModel.StabilityCheck,
                PaymentCheck = oldModel.PaymentCheck,
                Type_Id = oldModel.Type_Id,
                Contract_Id = oldModel.Contract_Id,
                CertificateDate = oldModel.CertificateDate,
                CertificateGMPCheck = oldModel.CertificateGMPCheck,
                InvoiceRu = oldModel.InvoiceRu,
                InvoiceKz = oldModel.InvoiceKz,
                InvoiceDate = oldModel.InvoiceDate,
                InvoiceContractRu = oldModel.InvoiceContractRu,
                InvoiceContractKz = oldModel.InvoiceContractKz,
                InvoiceAgentLastName = oldModel.InvoiceAgentLastName,
                InvoiceAgentFirstName = oldModel.InvoiceAgentFirstName,
                InvoiceAgentMiddelName = oldModel.InvoiceAgentMiddelName,
                InvoiceAgentPositionName = oldModel.InvoiceAgentPositionName,
                IsDeleted = false,
                DesignDate = oldModel.DesignDate,
                ObkContracts = oldModel.ObkContracts
            };
            
            AppContext.OBK_AssessmentDeclaration.Add(model);
            AppContext.SaveChanges();
            return model;
        }

        public OBK_AssessmentDeclaration GetPreamble(Guid id)
        {
            var context = CreateDatabaseContext(false);
            var preamble = context.OBK_AssessmentDeclaration
                //.Include(e => e.ObkContracts)
                //.Include(e => e)
                //.Include(e => e.EXP_DrugOrganizations)
                //.Include(e => e.EXP_DrugPatent)
                ////                .Include(e => e.EXP_DrugPrice)
                //.Include(e => e.EXP_DrugProtectionDoc)
                //.Include(e => e.EXP_DrugType)
                //.Include(e => e.EXP_DrugUseMethod)
                //                .Include(e => e.EXP_DrugWrapping).
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
            return preamble;
        }
        /// <summary>
        /// генерация ук
        /// </summary>
        /// <returns></returns>
        public string GetAppNumber()
        {
            int year = DateTime.Now.Year;
            var numer = AppContext.OBK_AssessmentDeclaration.Where(e => e.SendDate.Value.Year == year).Max(e => e.Number);
            if (numer == null)
            {
                return year + "000001";
            }
            long numberConvert;
            if (long.TryParse(numer, out numberConvert))
            {
                var newNumber = numberConvert + 1;
                return newNumber.ToString();
            }
            return null;
        }
        /// <summary>
        /// сохранение истории при отправке в ЦОЗ
        /// </summary>
        /// <param name="history"></param>
        /// <param name="getCurrentUserId"></param>
        public void SaveHisotry(OBK_AssessmentDeclarationHistory history, Guid? getCurrentUserId)
        {
            AppContext.OBK_AssessmentDeclarationHistory.Add(history);
            AppContext.SaveChanges();
        }
        /// <summary>
        /// сохранение заявления
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual OBK_AssessmentDeclaration SaveOrUpdate(OBK_AssessmentDeclaration entity, Guid? userId)
        {

            if (entity.Id == Guid.Empty)
            {
                try
                {
                    entity.CreatedDate = DateTime.Now;
                    AppContext.MarkAsAdded(entity);
                    AppContext.Commit(true);
                    return entity;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

            //var suspendedStage = AppContext.EXP_ExpertiseStage.FirstOrDefault(x =>
            //    !x.IsHistory
            //    && x.DeclarationId == entity.Id
            //    && x.IsSuspended
            //);
            //if (suspendedStage != null)
            //{
            //    LogHelper.Log.DebugFormat("Найден приостановленный этап {0}", suspendedStage.Id);
            //    if (suspendedStage.SuspendedStartDate.HasValue)
            //    {
            //        suspendedStage.IsSuspended = false;
            //        var suspendedDays = (DateTime.Now - suspendedStage.SuspendedStartDate.Value).TotalDays;
            //        LogHelper.Log.DebugFormat("Всего дней приостановки {0}", suspendedDays);
            //        if (suspendedStage.EndDate.HasValue)
            //        {
            //            suspendedStage.EndDate = suspendedStage.EndDate.Value.AddDays(suspendedDays);
            //        }
            //        else
            //        {
            //            LogHelper.Log.DebugFormat("У этапа {0} не указана дата завершения исполнения EndDate", suspendedStage.Id);
            //        }
            //        AppContext.Commit(true);
            //    }
            //    else
            //    {
            //        LogHelper.Log.DebugFormat("У этапа {0} почему-то не указана дата начала приостановки", suspendedStage.Id);
            //    }
            //}
            //else
            //{
            //    LogHelper.Log.Debug("Заявление не содержит приостановленных этапов");
            //}

            var attachedEntity = AppContext.Set<OBK_AssessmentDeclaration>().Find(entity.Id);
            AppContext.Entry(attachedEntity).CurrentValues.SetValues(entity);
            AppContext.Commit(true);
            //Отправка заявления на этап ЦОЗ
            if (entity.StatusId != CodeConstManager.STATUS_DRAFT_ID)
            {
                string resultDescription;
                var stageRepository = new AssessmentStageRepository();
                if (!stageRepository.HasStage(entity.Id, CodeConstManager.STAGE_OBK_COZ))
                {
                }
                //stageRepository.ToNextStage(entity.Id, null, new[] { CodeConstManager.STAGE_OBK_COZ }, out resultDescription);
            }
            return entity;
        }

        #region внутренний портал

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="stage"></param>
        /// <param name="userId"></param>
        /// <param name="customFilter"></param>
        /// <returns></returns>
        //public IQueryable<OBK_AssessmentDeclarationRegisterView> SafetyAssessmentRegisterList(string status, int stage, Guid userId, DeclarationRegistryFilter customFilter)
        //{
        //    var query =
        //        AppContext.OBK_AssessmentDeclarationRegisterView;
        //    return query;
        //}
        #endregion
    }
}
