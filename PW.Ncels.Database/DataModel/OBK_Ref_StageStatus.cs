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
    
    public partial class OBK_Ref_StageStatus
    {
        public OBK_Ref_StageStatus()
        {
            this.OBK_AssessmentStage = new HashSet<OBK_AssessmentStage>();
            this.OBK_ContractStage = new HashSet<OBK_ContractStage>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public System.DateTime DateCreate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<OBK_AssessmentStage> OBK_AssessmentStage { get; set; }
        public virtual ICollection<OBK_ContractStage> OBK_ContractStage { get; set; }
    }
}
