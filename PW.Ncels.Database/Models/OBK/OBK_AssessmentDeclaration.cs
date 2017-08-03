using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PW.Ncels.Database.Repository;

namespace PW.Ncels.Database.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OBK_AssessmentDeclaration : IEntity
    {

        public string ObjectId
        {
            get
            {
                if (Id == Guid.Empty)
                {
                    return null;
                }
                return Id.ToString();
            }
        }

        public string NameRu { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CurrencyId { get; set; }
        public List<OBK_Contract> ObkContracts { get; set; }
        public List<OBK_RS_Products> ObkRsProductses { get; set; }
        public List<OBK_Procunts_Series> ObkProcuntsSeries { get; set; }
    }
}
