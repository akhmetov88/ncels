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
    
    public partial class Exp_DrugDosageStageForAddView
    {
        public System.Guid Id { get; set; }
        public long DosageId { get; set; }
        public System.Guid DosageStageId { get; set; }
        public string DosageRegNumber { get; set; }
        public System.Guid DeclarationId { get; set; }
        public string DeclarationNumber { get; set; }
        public string DeclarationNameRu { get; set; }
        public System.DateTime DeclarationCreatedDate { get; set; }
        public int StageId { get; set; }
        public string StageNameRu { get; set; }
        public Nullable<int> ResultId { get; set; }
        public string ResultNameRu { get; set; }
        public Nullable<System.Guid> FinalDocStatusId { get; set; }
        public string FinalDocStatusCode { get; set; }
        public string FinalDocStatusDisplayName { get; set; }
        public string ProducerNameRu { get; set; }
        public Nullable<System.Guid> ProducerCountryId { get; set; }
        public Nullable<System.DateTime> ProducerDocDate { get; set; }
        public Nullable<System.DateTime> ProducerDocExpiryDate { get; set; }
        public string ProducerCountryName { get; set; }
        public Nullable<int> DosageFormId { get; set; }
        public string DosageFormName { get; set; }
        public string DeclarationPaysDates { get; set; }
        public Nullable<int> DeclarationAtxId { get; set; }
        public string DeclarationAtxName { get; set; }
        public string DeclarationAtxCode { get; set; }
        public Nullable<int> DeclarationMnnId { get; set; }
        public string DeclarationMnnNameRu { get; set; }
        public Nullable<int> DeclarationSaleTypeId { get; set; }
        public string DeclarationSaleTypeName { get; set; }
        public Nullable<int> NtdId { get; set; }
        public string NtdNameRu { get; set; }
    }
}
