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
    
    public partial class sync_fr_sr_reg_actions_tracking
    {
        public int id { get; set; }
        public Nullable<int> update_scope_local_id { get; set; }
        public Nullable<int> scope_update_peer_key { get; set; }
        public Nullable<long> scope_update_peer_timestamp { get; set; }
        public int local_update_peer_key { get; set; }
        public byte[] local_update_peer_timestamp { get; set; }
        public Nullable<int> create_scope_local_id { get; set; }
        public Nullable<int> scope_create_peer_key { get; set; }
        public Nullable<long> scope_create_peer_timestamp { get; set; }
        public int local_create_peer_key { get; set; }
        public long local_create_peer_timestamp { get; set; }
        public int sync_row_is_tombstone { get; set; }
        public Nullable<long> restore_timestamp { get; set; }
        public Nullable<System.DateTime> last_change_datetime { get; set; }
    }
}
