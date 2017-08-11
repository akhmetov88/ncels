using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Models.OBK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Ncels.Database.Repository.OBK
{
    public class OBKContractRepository : ARepository
    {
        public OBKContractRepository()
        {
            AppContext = CreateDatabaseContext();
        }

        public OBKContractRepository(bool isProxy)
        {
            AppContext = CreateDatabaseContext(isProxy);
        }

        public OBKContractRepository(ncelsEntities context) : base(context) { }

        public IEnumerable<OBK_ProductInfo> GetSearchReestr(int regType, string regNumber, string tradeName, string manufacturer, string mnn)
        {
            if (regNumber == null && tradeName == null && manufacturer == null && mnn == null)
                return null;
            //var reestr = AppContext.sr_register.Where(
            //    x =>
            //    (x.reg_type_id == regType) &&
            //    ((string.IsNullOrEmpty(regNumber)) || x.reg_number == regNumber) &&
            //    ((string.IsNullOrEmpty(tradeName)) || x.name == tradeName) &&
            //    ((string.IsNullOrEmpty(manufacturer)) || x.C_producer_name == manufacturer)
            //    ).Select(x => new OBK_ProductInfo { Id = x.id, RegNumber = x.reg_number, Name = x.name, RegTypeName = x.sr_reg_types.name, RegDate = x.reg_date, ExpireDate = x.expiration_date, ProducerName = x.C_producer_name, CountryName = x.C_country_name });

            var reestr = from register in AppContext.sr_register
                         join d in AppContext.sr_register_drugs on register.id equals d.id into register_drugs
                         from drugs in register_drugs.DefaultIfEmpty()
                         join i in AppContext.sr_international_names on drugs.int_name_id equals i.id into registerdrugsintnames
                         from intnames in registerdrugsintnames.DefaultIfEmpty()
                         join obkp in AppContext.obk_products on register.id equals obkp.register_id into obkProducts
                         from obkProduct in obkProducts.DefaultIfEmpty()
                         join obkpc in AppContext.obk_product_cost on obkProduct.id equals obkpc.id into obkProductsCosts
                         from obkProductCost in obkProductsCosts.DefaultIfEmpty()
                         join obkc in AppContext.obk_currencies on obkProductCost.currency_id equals obkc.id into obkCurrencies
                         from obkCurrency in obkCurrencies.DefaultIfEmpty()
                         where register.reg_type_id == regType &&
                         (string.IsNullOrEmpty(regNumber) || register.reg_number == regNumber) &&
                         (string.IsNullOrEmpty(tradeName) || register.name == tradeName) &&
                         (string.IsNullOrEmpty(manufacturer) || register.C_producer_name == manufacturer) &&
                         (string.IsNullOrEmpty(mnn) || intnames.name_rus == mnn)
                         select new OBK_ProductInfo
                         {
                             Id = register.id,
                             RegNumber = register.reg_number,
                             Name = register.name,
                             NameKz = register.name_kz,
                             RegTypeName = register.sr_reg_types.name,
                             RegDate = register.reg_date,
                             ExpireDate = register.expiration_date,
                             ProducerName = register.C_producer_name,
                             ProducerNameKz = register.C_producer_name_kz,
                             CountryName = register.C_country_name,
                             CountryNameKz = register.C_country_name_kz,
                             TnvedCode = obkProduct.tnved_code,
                             KpvedCode = obkProduct.kpved_code,
                             Price = obkProductCost.cost,
                             Currency = obkCurrency.currency_name
                         };

            if (reestr != null)
            {
                return reestr.ToList();
            }
            return null;
        }
    }
}
