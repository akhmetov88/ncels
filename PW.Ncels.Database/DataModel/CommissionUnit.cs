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
    
    public partial class CommissionUnit
    {
        public int Id { get; set; }
        public int CommissionId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public int UnitTypeId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual CommissionUnitType CommissionUnitType { get; set; }
        public virtual Commission Commission { get; set; }
    }
}
