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
    
    public partial class sr_register_mt_parts
    {
        public int id { get; set; }
        public int register_id { get; set; }
        public string name { get; set; }
        public string name_kz { get; set; }
        public Nullable<int> part_number { get; set; }
        public string model { get; set; }
        public string specification { get; set; }
        public string specification_kz { get; set; }
        public Nullable<int> producer_id { get; set; }
        public Nullable<int> country_id { get; set; }
        public string producer_name { get; set; }
        public string country_name { get; set; }
        public string producer_name_kz { get; set; }
        public string country_name_kz { get; set; }
    
        public virtual sr_producers sr_producers { get; set; }
        public virtual sr_register_mt sr_register_mt { get; set; }
    }
}
