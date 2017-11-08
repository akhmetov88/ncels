//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PW.Ncels.Database.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class OBK_AssessmentStage
    {
        public OBK_AssessmentStage()
        {
            this.OBK_AssessmentStageSignData = new HashSet<OBK_AssessmentStageSignData>();
            this.OBK_AssessmentStageExecutors = new HashSet<OBK_AssessmentStageExecutors>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid DeclarationId { get; set; }
        public int StageId { get; set; }
        public int StageStatusId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> FactEndDate { get; set; }
        public Nullable<int> ResultId { get; set; }
        public Nullable<System.Guid> ParentStageId { get; set; }
        public bool IsHistory { get; set; }
        public string OtdIds { get; set; }
        public string OtdRemarks { get; set; }
        public bool IsSuspended { get; set; }
        public Nullable<System.DateTime> SuspendedStartDate { get; set; }
    
        public virtual OBK_AssessmentDeclaration OBK_AssessmentDeclaration { get; set; }
        public virtual OBK_Ref_Stage OBK_Ref_Stage { get; set; }
        public virtual OBK_Ref_StageStatus OBK_Ref_StageStatus { get; set; }
        public virtual ICollection<OBK_AssessmentStageSignData> OBK_AssessmentStageSignData { get; set; }
        public virtual ICollection<OBK_AssessmentStageExecutors> OBK_AssessmentStageExecutors { get; set; }
    }
}
