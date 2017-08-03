using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.Expertise;

namespace PW.Ncels.Database.Repository.OBK
{
    /// <summary>
    /// 
    /// </summary>
    public class SafetyAssessmentRepository : ARepository
    {

        public SafetyAssessmentRepository() {
            AppContext = CreateDatabaseContext();
        }

        public SafetyAssessmentRepository(bool isProxy) {
            AppContext = CreateDatabaseContext(isProxy);
        }

        public SafetyAssessmentRepository(ncelsEntities context) : base(context) { }

        /// <summary>
        /// Получение списка контрактов
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public IQueryable<OBK_Contract> GetContractsByStatuses(Guid ownerId)
        {
            return AppContext.OBK_Contract.Where(e => e.OwnerId == ownerId);
        }

        /// <summary>
        /// Показать список c проставлением дополнительной информации по владельцу
        /// </summary>
        /// <param name="ownerId">владелец</param>
        /// <returns></returns>
        public IEnumerable<OBK_Contract> GetActiveContractListWithInfo(Guid ownerId)
        {
            var list = GetContractsByStatuses(ownerId).OrderBy(e => e.StartDate);

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

        /// <summary>
        /// Список стран
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dictionary> GetCounties() {
            return AppContext.Dictionaries.Where(o => o.Type == "Country").ToList();
        }

        /// <summary>
        /// валюта
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dictionary> GetObkCurrencies() {
            return AppContext.Dictionaries.Where(o => o.Type == "Currency").ToList();
        }

        public OBK_AssessmentDeclaration GetById(string modelId) {
            return AppContext.OBK_AssessmentDeclaration.FirstOrDefault(e => e.Id == new Guid(modelId));
        }

        public IQueryable<OBK_RS_Products> GetRsProducts(string modelId)
        {
            return AppContext.OBK_RS_Products.Where(e => e.OBK_AssessmentDeclarationId == new Guid(modelId));
        }

        public ReesrtObk GetSearchReestr(string regNumber, string tradeName, string manufacturer, string mnn)
        {
            var reestr = AppContext.sr_register.FirstOrDefault(x => x.reg_number == regNumber);
            if (reestr != null) {
                return AppContext.ReesrtObks.FirstOrDefault(x => x.register_id == reestr.id);
            }
            return null;
        }

        public IQueryable<OBK_RS_Products> GetAddToTable(string modelId)
        {
           var result = AppContext.OBK_RS_Products.Where(x => x.OBK_AssessmentDeclarationId == new Guid(modelId));
            return result;
        }

        /// <summary>
        /// Поиск договора по id
        /// </summary>
        public OBK_Organization GetOrganizationById(int id)
        {
            return AppContext.OBK_Organization.FirstOrDefault(e => e.Id == id);
        }

        public object GetSaveObkRsProducts(string modelId, string nameRu, string nameKz, string producerNameRu, string producerNameKz, string countryNameRu, string countryNameKz,
            string tnvedCode, string kpvedCode, string price, string currency)
        {
            bool isNew = false;
            var model = GetById(modelId);
            if (model == null)
            {
                model = new OBK_AssessmentDeclaration
                {
                    OwnerId = UserHelper.GetCurrentEmployee().Id,
                    Type_Id = 1,
                    Id = new Guid(modelId),
                    CreatedDate = DateTime.Now,
                    StatusId = CodeConstManager.STATUS_DRAFT_ID,
                    Contract_Id = UserHelper.GetCurrentEmployee().Id,
                    CertificateDate = DateTime.Now
                };
                isNew = true;
            }

            if (isNew) {
                var products = new OBK_RS_Products {
                    OBK_AssessmentDeclarationId = model.Id,
                    NameRu = nameRu,
                    NameKz = nameKz,
                    ProducerNameRu = producerNameRu,
                    ProducerNameKz = producerNameKz,
                    CountryNameRu = countryNameRu,
                    CountryNameKZ = countryNameKz,
                    TnvedCode = tnvedCode,
                    KpvedCode = kpvedCode,
                    Price = price
                };
                AppContext.OBK_AssessmentDeclaration.Add(model);
                AppContext.OBK_RS_Products.Add(products);
                AppContext.SaveChanges();
                return new {Success = true};
            }
            else {
                var products = new OBK_RS_Products {
                    OBK_AssessmentDeclarationId = model.Id,
                    NameRu = nameRu,
                    NameKz = nameKz,
                    ProducerNameRu = producerNameRu,
                    ProducerNameKz = producerNameKz,
                    CountryNameRu = countryNameRu,
                    CountryNameKZ = countryNameKz,
                    TnvedCode = tnvedCode,
                    KpvedCode = kpvedCode,
                    Price = price
                };
                AppContext.OBK_RS_Products.Add(products);
                AppContext.SaveChanges();
                return new { Success = true };
            }
            return new { Success = false };
        }

        /// <summary>
        /// загрузка списка заявлений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isRegisterProject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> GetSafetyAssessmentDeclarationList(ModelRequest request, bool isRegisterProject, int? type)
        {
            try
            {
                //Database query
                var employeeId = UserHelper.GetCurrentEmployee().Id;
                var org = UserHelper.GetCurrentEmployee();
                var v = type != null ? AppContext.OBK_AssessmentDeclarationView.Where(m => m.OwnerId == employeeId).OrderByDescending(m => m.SortDate).AsQueryable()
                    : AppContext.OBK_AssessmentDeclarationView.Where(m => m.OwnerId == employeeId).AsQueryable();
                //search
                if (!string.IsNullOrEmpty(request.SearchValue)) {
                    v = v.Where( a => a.Number.Contains(request.SearchValue) || a.StausName.Contains(request.SearchValue));
                }

                //sort
                if (!(string.IsNullOrEmpty(request.SortColumn) && string.IsNullOrEmpty(request.SortColumnDir))) {
                    v = v.OrderBy(request.SortColumn + " " + request.SortColumnDir);
                }


                int recordsTotal = await v.CountAsync();
                var expertiseViews = v.Skip(request.Skip).Take(request.PageSize);
                return
                    new  {
                        draw = request.Draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        Data = await expertiseViews.ToListAsync()
                    };
            }

            catch (Exception e) {
                return new { IsError = true, Message = e.Message };
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
        public SubUpdateField UpdateModel(string code, int typeId, string modelId, string userId, long? recordId, string fieldName, string fieldValue, string fieldDisplay)
        {
            bool isNew = false;
            var model = GetById(modelId);
            if (model == null)
            {
                model = new OBK_AssessmentDeclaration
                {
                    OwnerId = UserHelper.GetCurrentEmployee().Id,
                    Type_Id = typeId,
                    Id = new Guid(modelId),
                    CreatedDate = DateTime.Now,
                    StatusId = CodeConstManager.STATUS_DRAFT_ID,
                    //Contract_Id = UserHelper.GetCurrentEmployee().Id,
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

        private SubUpdateField UpdateMain(bool isNew, OBK_AssessmentDeclaration model, string fieldName, string fieldValue, string userId, string fieldDisplay)
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
        protected void SaveHistoryField(Guid modelId, string fieldName, string fieldValue, Guid userId, string fieldDisplay)
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
    }
}
