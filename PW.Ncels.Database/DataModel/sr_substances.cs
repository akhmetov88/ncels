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
    
    public partial class sr_substances
    {
        public sr_substances()
        {
            this.EXP_DrugSubstance = new HashSet<EXP_DrugSubstance>();
            this.sr_register_substances = new HashSet<sr_register_substances>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string name_kz { get; set; }
        public string name_eng { get; set; }
        public bool animal_sign { get; set; }
        public Nullable<int> category_id { get; set; }
        public string category_pos { get; set; }
        public bool block_sign { get; set; }
    
        public virtual ICollection<EXP_DrugSubstance> EXP_DrugSubstance { get; set; }
        public virtual sr_categories sr_categories { get; set; }
        public virtual ICollection<sr_register_substances> sr_register_substances { get; set; }
    }
}
