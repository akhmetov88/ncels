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
    
    public partial class TmcReportData
    {
        public System.Guid Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatedEmployeeId { get; set; }
        public string CreatedEmployeeName { get; set; }
        public Nullable<System.Guid> TmcId { get; set; }
        public string TmcName { get; set; }
        public string TmcCode { get; set; }
        public Nullable<decimal> GeneralCount { get; set; }
        public Nullable<decimal> GeneralCountFact { get; set; }
        public Nullable<decimal> GeneralConvertFact { get; set; }
        public Nullable<decimal> IssuedCount { get; set; }
        public Nullable<decimal> UseCount { get; set; }
        public Nullable<decimal> UseFactCount { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string MeasureName { get; set; }
        public string MeasureConvertName { get; set; }
        public string UseReasonText { get; set; }
        public Nullable<System.Guid> PositionId { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> SubDepartmentId { get; set; }
        public string SubDepartmentName { get; set; }
        public Nullable<System.Guid> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.Guid> CenterId { get; set; }
        public string CenterName { get; set; }
        public Nullable<System.Guid> ExpertiseStatementId { get; set; }
        public string ExpertiseStatementNumber { get; set; }
        public string ExpertiseStatementTypeStr { get; set; }
        public System.Guid refTmcReport { get; set; }
    }
}
