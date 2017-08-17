using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Ncels.Database.Models.OBK
{
    public class OBKContractViewModel
    {
        public System.Guid Id { get; set; }
        public int Type { get; set; }
        public int DeclarantId { get; set; }
        public bool IsResident { get; set; }
        public Guid DeclarantOrganizationFormId { get; set; }
        public bool DeclarantBin { get; set; }
        public bool DeclarantNameKz { get; set; }
        public bool DeclarantNameRu { get; set; }
        public bool DeclarantNameEn { get; set; }
        public bool DeclarantCountryId { get; set; }
        public bool ExpertOrganization { get; set; }
        public bool AddressLegalRu { get; set; }
        public bool AddressLegalKz { get; set; }
        public bool AddressFact { get; set; }
        public bool Phone { get; set; }
        public bool Email { get; set; }
        public bool BossLastName { get; set; }
        public bool BossFirstName { get; set; }
        public bool BossMiddleName { get; set; }
        public bool BossPosition { get; set; }
        public bool BossDocType { get; set; }
        public bool IsHasBossDocNumber { get; set; }
        public bool BossDocNumber { get; set; }
        public bool BossDocCreatedDate { get; set; }
        public bool SignLastName { get; set; }
        public bool SignFirstName { get; set; }
        public bool SignMiddleName { get; set; }
        public bool SignPosition { get; set;}
        public bool SignDocType { get; set; }
        public bool IsHasSignDocNumber { get; set; }
        public bool SignDocNumber { get; set; }
        public bool SignDocCreatedDate { get; set; }
        public bool BankIik { get; set; }
        public bool BankBik { get; set; }
        public bool CurrencyId { get; set; }
        public bool BankNameRu { get; set; }
        public bool BankNameKz { get; set; }
    }
}
