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
    
    public partial class I1c_primary_ObkApplicationPaymentState
    {
        public System.Guid Id { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<System.DateTime> PaymentDatetime { get; set; }
        public Nullable<System.Guid> refContractId { get; set; }
    }
}
