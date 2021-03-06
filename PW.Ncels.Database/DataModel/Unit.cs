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
    
    public partial class Unit
    {
        public Unit()
        {
            this.Employees = new HashSet<Employee>();
            this.Employees1 = new HashSet<Employee>();
            this.Units1 = new HashSet<Unit>();
            this.DIC_Storages = new HashSet<DIC_Storages>();
            this.OBK_Contract = new HashSet<OBK_Contract>();
        }
    
        public System.Guid Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Path { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public Nullable<System.Guid> EmployeeId { get; set; }
        public int PositionState { get; set; }
        public string ManagerId { get; set; }
        public string SecretaryId { get; set; }
        public string BossId { get; set; }
        public string ChancelleryId { get; set; }
        public string UnitTypeDictionaryId { get; set; }
        public string UnitTypeDictionaryValue { get; set; }
        public int Type { get; set; }
        public string ManagerValue { get; set; }
        public string SecretaryValue { get; set; }
        public string BossValue { get; set; }
        public string ChancelleryValue { get; set; }
        public string DisplayName { get; set; }
        public string CuratorId { get; set; }
        public string CuratorValue { get; set; }
        public int Rank { get; set; }
        public string Email { get; set; }
        public int PositionType { get; set; }
        public int PositionStaff { get; set; }
        public string NameKz { get; set; }
        public string LegalAddress { get; set; }
        public string ActualAddress { get; set; }
        public string Phone { get; set; }
        public Nullable<System.Guid> CountryId { get; set; }
        public string Iin { get; set; }
        public string Bin { get; set; }
        public string ApplicationNumber { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Employee> Employees1 { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Unit> Units1 { get; set; }
        public virtual Unit Parent { get; set; }
        public virtual ICollection<DIC_Storages> DIC_Storages { get; set; }
        public virtual ICollection<OBK_Contract> OBK_Contract { get; set; }
    }
}
