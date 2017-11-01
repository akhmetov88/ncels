using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PW.Ncels.Database.DataModel;

namespace PW.Ncels.Database.Repository.OBK
{
    public class OBKExpDocumentRepository : ARepository
    {
        public virtual OBK_AssessmentStage GetAssessmentStage(Guid id)
        {
            return new AssessmentStageRepository().GetById(id);
        }

        public OBK_AssessmentDeclaration GetAssessmentDeclaration(Guid id)
        {
            return AppContext.OBK_AssessmentDeclaration.FirstOrDefault(e => e.Id == id);
        }

        public OBK_StageExpDocumentResult GetStageExpDocResult(Guid modelId)
        {
            return AppContext.OBK_StageExpDocumentResult.FirstOrDefault(e => e.AssessmetDeclarationId == modelId);
        }

        public void SaveExpDocumentResult(bool expResult, Guid modelId)
        {
            var expDocResult =
                AppContext.OBK_StageExpDocumentResult.FirstOrDefault(e => e.AssessmetDeclarationId == modelId);
            if (expDocResult == null)
            {
                var result = new OBK_StageExpDocumentResult {
                    Id = Guid.NewGuid(),
                    AssessmetDeclarationId = modelId,
                    ExpResult = expResult
                };
                AppContext.OBK_StageExpDocumentResult.Add(result);
                AppContext.SaveChanges();
            }
            else
            {
                var result = new OBK_StageExpDocumentResult {
                    Id = expDocResult.Id,
                    AssessmetDeclarationId = modelId,
                    ExpResult = expResult
                };
                AppContext.OBK_StageExpDocumentResult.AddOrUpdate(result);
                AppContext.SaveChanges();
            }
        }

        public string GetSignData(Guid id)
        {
            return null;
        }
    }
}
