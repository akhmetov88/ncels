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
    
    public partial class AccessTask
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public System.Guid ObjectId { get; set; }
        public string PropertyName { get; set; }
    
        public virtual Task Task { get; set; }
    }
}
