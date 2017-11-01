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
    
    public partial class TmcOffStateView
    {
        public System.Guid Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatedEmployeeId { get; set; }
        public int StateType { get; set; }
        public string UseReason { get; set; }
        public decimal UsedCount { get; set; }
        public Nullable<System.Guid> ExpertiseStatementId { get; set; }
        public string ExpertiseStatementNumber { get; set; }
        public string ExpertiseStatementTypeStr { get; set; }
        public decimal RequstedCount { get; set; }
        public decimal ReceivedCount { get; set; }
        public System.Guid TmcId { get; set; }
        public System.DateTime TmcCreateDate { get; set; }
        public System.Guid TmcCreateEmployeeId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Manufacturer { get; set; }
        public string Serial { get; set; }
        public decimal RequestedCount { get; set; }
        public Nullable<System.Guid> MeasureTypeDicId { get; set; }
        public decimal CountFact { get; set; }
        public decimal ConvertedCount { get; set; }
        public Nullable<System.Guid> MeasureTypeConvertDicId { get; set; }
        public decimal ResidueCount { get; set; }
        public Nullable<System.DateTime> ManufactureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.Guid> PackageDicId { get; set; }
        public Nullable<System.Guid> TmcTypeDicId { get; set; }
        public Nullable<System.Guid> OwnerEmployeeId { get; set; }
        public Nullable<System.DateTime> ReceivingDate { get; set; }
        public Nullable<System.DateTime> WriteoffDate { get; set; }
    }
}
