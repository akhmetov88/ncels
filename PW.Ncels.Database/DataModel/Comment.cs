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
    
    public partial class Comment
    {
        public System.Guid Id { get; set; }
        public string Value { get; set; }
        public System.Guid AuthorId { get; set; }
        public string AuthorValue { get; set; }
        public System.Guid refObjectId { get; set; }
        public Nullable<System.Guid> refParentComment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
}
