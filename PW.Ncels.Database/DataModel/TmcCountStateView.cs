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
    
    public partial class TmcCountStateView
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal CountFact { get; set; }
        public decimal CountConvert { get; set; }
        public decimal IssuedCount { get; set; }
        public Nullable<decimal> UsedCount { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
