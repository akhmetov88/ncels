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
    
    public partial class Holiday
    {
        public long Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeValue { get; set; }
        public string HolidayTypeDictionaryId { get; set; }
        public string HolidayTypeDictionaryValue { get; set; }
        public Nullable<System.DateTime> PeriodStart { get; set; }
        public Nullable<System.DateTime> PeriodEnd { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string DocumentId { get; set; }
        public string DocumentValue { get; set; }
        public string Note { get; set; }
        public int Count { get; set; }
    }
}
