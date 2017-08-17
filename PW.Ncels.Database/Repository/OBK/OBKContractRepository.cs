using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.OBK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        //public OBKContractViewModel SaveContract(OBKContractViewModel contractViewModel)
        //{
        //    OBK_Contract projectContract = AppContext.OBK_Contract.FirstOrDefault(o => o.Id == contractViewModel.Id);
        //    if (projectContract != null)
        //    {
        //        UpdateFields(contractViewModel, projectContract);
        //        AppContext.SaveChanges();
        //    }
        //    else
        //    {
        //        OBK_Contract newContract = new OBK_Contract();
        //        newContract.Id = Guid.NewGuid();
        //        newContract.CreatedDate = DateTime.Now;
        //        newContract.Number = "б/н";
        //        newContract.Status = 1;

        //        AppContext.OBK_Contract.Add(newContract);
        //        AppContext.SaveChanges();
        //    }
        //}

        private void UpdateFields(OBKContractViewModel viewModel, OBK_Contract model)
        {
            //model.Type = viewModel.Type;
            //model.dec
        }

        public OBK_Contract SaveContract(OBK_Contract contract)
        {
            var project = AppContext.OBK_Contract.Any(o => o.Id == contract.Id);
            if (project)
            {
                AppContext.Entry(contract).State = System.Data.Entity.EntityState.Modified;
                AppContext.SaveChanges();
            }
            else
            {
                contract.Id = Guid.NewGuid();
                contract.CreatedDate = DateTime.Now;
                contract.Number = "б/н";
                contract.Status = 1;

                AppContext.OBK_Contract.Add(contract);
                AppContext.SaveChanges();
            }
            return contract;
        }

        public OBK_Declarant GetOrganizationData(Guid guid)
        {
            var declarant = AppContext.OBK_Declarant.FirstOrDefault(x => x.Id == guid);
            return declarant;
        }

        /// <summary>
        /// загрузка списка заявлений
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isRegisterProject"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<object> GetContractList(ModelRequest request, bool isRegisterProject)
        {
            try
            {
                var employeeId = UserHelper.GetCurrentEmployee().Id;
                //var v = AppContext.OBK_Contract.Where(x => x.EmployeeId == employeeId).AsQueryable();
                var v = AppContext.OBK_Contract.AsQueryable();

                //search
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    v =
                        v.Where(
                            a => a.Number.Contains(request.SearchValue));
                }


                //sort
                if (!(string.IsNullOrEmpty(request.SortColumn) && string.IsNullOrEmpty(request.SortColumnDir)))
                {
                    //v = v.OrderBy(request.SortColumn + " " + request.SortColumnDir);
                    v = v.OrderBy(x => x.EmployeeId);
                }

                int recordsTotal = await v.CountAsync();
                var contractViews = v.Skip(request.Skip).Take(request.PageSize).Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.CreatedDate,
                    Status = x.OBK_Ref_Status.NameRu,
                    DeclarantName = x.OBK_Declarant.NameRu,
                    x.StartDate,
                    x.EndDate
                }
                );
                return
                    new
                    {
                        draw = request.Draw,
                        recordsFiltered = recordsTotal,
                        recordsTotal = recordsTotal,
                        Data = await contractViews.ToListAsync()
                    };
            }
            catch (Exception e)
            {
                return new { IsError = true, Message = e.Message };
            }
        }
    }
}
