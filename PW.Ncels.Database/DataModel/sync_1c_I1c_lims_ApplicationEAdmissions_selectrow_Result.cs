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
    
    public partial class sync_1c_I1c_lims_ApplicationEAdmissions_selectrow_Result
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> TmcCode { get; set; }
        public string TmcNumber { get; set; }
        public Nullable<decimal> QuntityVolume { get; set; }
        public string QuntityVolumeStr { get; set; }
        public string Unit { get; set; }
        public Nullable<System.Guid> UnitCode { get; set; }
        public string Producer { get; set; }
        public Nullable<System.DateTime> ProducerExpirationDate { get; set; }
        public string PartyNumber { get; set; }
        public string ShelfNumber { get; set; }
        public string CupboardNumber { get; set; }
        public Nullable<System.Guid> WarehouseNumber { get; set; }
        public Nullable<System.DateTime> DateOfReceiving { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> ExportDatetime { get; set; }
        public Nullable<System.DateTime> ImportDatetime { get; set; }
        public string PowerOfAttorneyNumber_1C { get; set; }
        public Nullable<System.DateTime> PowerOfAttorneyDatetime_1C { get; set; }
        public int sync_row_is_tombstone { get; set; }
        public byte[] sync_row_timestamp { get; set; }
        public Nullable<long> sync_update_peer_timestamp { get; set; }
        public Nullable<int> sync_update_peer_key { get; set; }
        public Nullable<long> sync_create_peer_timestamp { get; set; }
        public Nullable<int> sync_create_peer_key { get; set; }
    }
}