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
    
    public partial class OBKContractProductsView
    {
        public string NameRu { get; set; }
        public int Id { get; set; }
        public int OBK_RS_ProductsId { get; set; }
        public int ProdSeriesId { get; set; }
        public string ProdSeries { get; set; }
        public string ProdSeriesEndDate { get; set; }
        public string ProdSeriesParty { get; set; }
        public string ProdCountryNameRu { get; set; }
        public string ProdProducerNameRu { get; set; }
        public string ProdShortName { get; set; }
        public Nullable<System.Guid> ContractId { get; set; }
    }
}
