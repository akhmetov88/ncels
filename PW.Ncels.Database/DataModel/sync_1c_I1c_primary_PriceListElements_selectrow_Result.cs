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
    
    public partial class sync_1c_I1c_primary_PriceListElements_selectrow_Result
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> PriceListId { get; set; }
        public string PriceListName { get; set; }
        public string PriceListNameKz { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.Guid> refApplication { get; set; }
        public int sync_row_is_tombstone { get; set; }
        public byte[] sync_row_timestamp { get; set; }
        public Nullable<long> sync_update_peer_timestamp { get; set; }
        public Nullable<int> sync_update_peer_key { get; set; }
        public Nullable<long> sync_create_peer_timestamp { get; set; }
        public Nullable<int> sync_create_peer_key { get; set; }
    }
}
