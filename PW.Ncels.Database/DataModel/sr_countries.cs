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
    
    public partial class sr_countries
    {
        public sr_countries()
        {
            this.EXP_DrugExportTrade = new HashSet<EXP_DrugExportTrade>();
            this.EXP_DrugSubstance = new HashSet<EXP_DrugSubstance>();
            this.EXP_DrugOtherCountry = new HashSet<EXP_DrugOtherCountry>();
            this.sr_register_producers = new HashSet<sr_register_producers>();
            this.EXP_DrugSubstanceManufacture = new HashSet<EXP_DrugSubstanceManufacture>();
            this.sr_register_names = new HashSet<sr_register_names>();
        }
    
        public long id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string name_kz { get; set; }
        public string full_name_kz { get; set; }
        public string name_en { get; set; }
        public string short_name { get; set; }
        public bool cis_sign { get; set; }
        public bool baltic_sign { get; set; }
        public bool foreign_sign { get; set; }
        public bool europe_sign { get; set; }
        public bool america_sign { get; set; }
        public bool kz_sign { get; set; }
        public bool block_sign { get; set; }
    
        public virtual ICollection<EXP_DrugExportTrade> EXP_DrugExportTrade { get; set; }
        public virtual ICollection<EXP_DrugSubstance> EXP_DrugSubstance { get; set; }
        public virtual ICollection<EXP_DrugOtherCountry> EXP_DrugOtherCountry { get; set; }
        public virtual ICollection<sr_register_producers> sr_register_producers { get; set; }
        public virtual ICollection<EXP_DrugSubstanceManufacture> EXP_DrugSubstanceManufacture { get; set; }
        public virtual ICollection<sr_register_names> sr_register_names { get; set; }
    }
}
