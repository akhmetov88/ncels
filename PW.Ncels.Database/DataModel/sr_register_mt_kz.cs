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
    
    public partial class sr_register_mt_kz
    {
        public int id { get; set; }
        public string purpose_kz { get; set; }
        public string use_area_kz { get; set; }
        public string description_kz { get; set; }
    
        public virtual sr_register_mt sr_register_mt { get; set; }
    }
}
