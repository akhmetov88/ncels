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
    
    public partial class OBK_Ref_ContractHistoryStatus
    {
        public OBK_Ref_ContractHistoryStatus()
        {
            this.OBK_ContractHistory = new HashSet<OBK_ContractHistory>();
        }
    
        public System.Guid Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public System.DateTime DateCreate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DateEdit { get; set; }
    
        public virtual ICollection<OBK_ContractHistory> OBK_ContractHistory { get; set; }
    }
}