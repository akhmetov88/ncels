using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;

namespace PW.Ncels.Database.Repository.OBK
{
    public class AssessmentStageRepository : ARepository
    {
        public AssessmentStageRepository() { }

        public AssessmentStageRepository(ncelsEntities context) : base(context) { }

        /// <summary>
        /// Проверка есть ли у заявления указанный этап
        /// </summary>
        /// <param name="declarationId">id заявления</param>
        /// <param name="stageCode">Код этапа</param>
        /// <returns></returns>
        public bool HasStage(Guid declarationId, int stageCode)
        {
            //return AppContext.OBK_AssessmentStage.Any(e => e.DeclarationId == declarationId &&
            //                                              e.StageId == stageCode);
            return false;
        }

        /// <summary>
        /// Перевод заявления на следующий этап
        /// </summary>
        /// <param name="declaration"></param>
        /// <param name="stageCode"></param>
        //public bool ToNextStage(Guid declarationId, Guid? fromStageId, int[] nextStageIds, out string resultDescription)
        //{
        //    resultDescription = null;
        //    string[] activeStageCodes =
        //    {
        //        OBK_Ref_StageStatus.New, OBK_Ref_StageStatus.InWork,
        //        OBK_Ref_StageStatus.InReWork
        //    };
        //    //var declaration = AppContext.OBK_AssessmentDeclaration.FirstOrDefault(e => e.Id == declarationId);
        //    //if (declaration.EXP_DIC_Type.Code != EXP_DIC_Type.Registration)
        //    //{
        //        //return ToNextStage(declaration, fromStageId, nextStageIds, out resultDescription);
        //    //}
        //    var currentStage = fromStageId != null
        //        ? AppContext.OBK_AssessmentStage.FirstOrDefault(e => e.Id == fromStageId)
        //        : AppContext.OBK_AssessmentStage.FirstOrDefault(
        //            e => e.DeclarationId == declarationId && activeStageCodes.Contains(e.OBK_Ref_StageStatus.Code));
        //    var stageStatusNew = GetStageStatusByCode(OBK_Ref_StageStatus.New);
        //    //закрываем предыдущий этап
        //    //if (currentStage != null && CanCloseStage(currentStage, nextStageIds))
        //    //{
        //    //    currentStage.StatusId = GetStageStatusByCode(EXP_DIC_StageStatus.Completed).Id;
        //    //    currentStage.FactEndDate = DateTime.Now;
        //    //}
        //    var isAnalitic = false;
        //    foreach (var nextStageId in nextStageIds)
        //    {
        //        //if (!CanSendToStep(declarationId, fromStageId, nextStageId, out resultDescription)) return false;
        //        //если имеется уже выполняющийся этап то продолжаем его дальше
        //        if (AppContext.OBK_AssessmentStage.Any(e => e.DeclarationId == declarationId
        //                                                   && e.StageId == nextStageId &&
        //                                                   e.OBK_Ref_StageStatus.Code != OBK_Ref_StageStatus.Completed &&
        //                                                   !e.IsHistory)) continue;
        //        //переделать дату окончания этапа
        //        var daysOnStage = 0;//GetExpStageDaysOnExecution(declaration.TypeId, nextStageId);
        //        var startDate = DateTime.Now;
        //        var newStage = new OBK_AssessmentStage()
        //        {
        //            Id = Guid.NewGuid(),
        //            DeclarationId = declarationId,
        //            StageId = nextStageId,
        //            ParentStageId = currentStage != null ? (Guid?)currentStage.Id : null,
        //            StatusId = stageStatusNew.Id,
        //            StartDate = startDate,
        //            EndDate = daysOnStage != null ? (DateTime?)startDate.AddDays(daysOnStage) : null
        //        };
        //        //todo брать руководителя цоз из настроек
        //        newStage.Employees.Add(GetExecutorByDicStageId(nextStageId));
        //        AppContext.OBK_AssessmentStage.Add(newStage);
        //        //if (nextStageId == CodeConstManager.STAGE_ANALITIC)
        //        //{
        //        //    isAnalitic = true;
        //        //}
        //    }
        //    AppContext.SaveChanges();
        //    //if (isAnalitic)
        //    //{
        //    //    CreateDirectionMaterial(declarationId, GetExecutorByDicStageId(CodeConstManager.STAGE_ANALITIC));
        //    //}
        //    return true;
        //}

        public OBK_Ref_StageStatus GetStageStatusByCode(string code)
        {
            return AppContext.OBK_Ref_StageStatus.AsNoTracking().FirstOrDefault(e => e.Code == code);
        }

        public Employee GetExecutorByDicStageId(int stageId)
        {
            var organization = AppContext.Units.FirstOrDefault(e => e.Id == new Guid("BBF0867E-E3EC-4B02-8B7D-B08FE96A893B"));
            return AppContext.Employees.FirstOrDefault(x => x.Id == new Guid(organization.BossId));
        }
    }
}
