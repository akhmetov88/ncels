using PW.Ncels.Database.Constants;
using PW.Ncels.Database.DataModel;
using PW.Ncels.Database.Helpers;
using PW.Ncels.Database.Models;
using PW.Ncels.Database.Models.OBK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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

        public IEnumerable<OBK_ProductInfo> GetSearchReestr(int regType, string regNumber, string tradeName, bool drugEndDateExpired)
        {
            if (regNumber == null && tradeName == null)
                return null;

            var reestr = from register in AppContext.sr_register
                         let obkProduct = AppContext.obk_products
                                            .Where(p => p.register_id == register.id)
                                            .OrderBy(p => p.id)
                                            .FirstOrDefault()
                         join srrmt in AppContext.sr_register_mt on register.id equals srrmt.id into srregistermtTable
                         from srregistermt in srregistermtTable.DefaultIfEmpty()
                         join srregprod in AppContext.sr_register_producers on register.id equals srregprod.register_id into srregisterproducersTable
                         from srregisterproducer in srregisterproducersTable.DefaultIfEmpty()
                         join srprod in AppContext.sr_producers on srregisterproducer.producer_id equals srprod.id into srproducersTable
                         from srproducers in srproducersTable.DefaultIfEmpty()
                         join srcn in AppContext.sr_countries on srregisterproducer.country_id equals srcn.id into srcountriesTable
                         from srcountries in srcountriesTable.DefaultIfEmpty()
                         where register.reg_type_id == regType &&
                         (string.IsNullOrEmpty(regNumber) || register.reg_number.ToLower().Contains(regNumber.ToLower())) &&
                         (string.IsNullOrEmpty(tradeName) || register.name.ToLower().Contains(tradeName.ToLower())) &&
                         (
                            (!drugEndDateExpired && register.expiration_date != null && register.expiration_date >= DateTime.Now) ||
                            drugEndDateExpired
                         )
                         &&
                         (srregisterproducer.producer_type_id == 1 || srregisterproducer.producer_type_id == null)
                         select new OBK_ProductInfo
                         {
                             ProductId = register.id,
                             RegNumber = register.reg_number,
                             Name = register.name,
                             NameKz = register.name_kz,
                             RegTypeId = register.sr_reg_types.id,
                             RegTypeName = register.sr_reg_types.name,
                             RegDate = register.reg_date,
                             ExpireDate = register.expiration_date,
                             ProducerName = srproducers.name,
                             ProducerNameKz = srproducers.name_kz,
                             CountryName = srcountries.name,
                             CountryNameKz = srcountries.name_kz,
                             TnvedCode = obkProduct.tnved_code,
                             KpvedCode = obkProduct.kpved_code,
                             DegreeRiskId = srregistermt.degree_risk_id
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
                contract.Number = CodeConstManager.OBK_CONTRACT_NO_NUMBER;
                contract.Status = 1;

                AppContext.OBK_Contract.Add(contract);
                AppContext.SaveChanges();
            }
            return contract;
        }

        public OBKContractViewModel SaveContract2(Guid guid, OBKContractViewModel contractViewModel)
        {
            var ret = new OBKContractViewModel();

            var obkContract = AppContext.OBK_Contract.Where(o => o.Id == contractViewModel.Id).FirstOrDefault();
            if (obkContract != null)
            {
                FillContract(contractViewModel, obkContract);
                ret.Id = contractViewModel.Id;
            }
            else
            {
                OBK_Contract contract = new OBK_Contract();
                contract.Id = guid;
                contract.CreatedDate = DateTime.Now;
                contract.Number = CodeConstManager.OBK_CONTRACT_NO_NUMBER;
                contract.Status = 1;

                var employeeId = UserHelper.GetCurrentEmployee().Id;
                contract.EmployeeId = employeeId;

                FillContract(contractViewModel, contract);

                AppContext.OBK_Contract.Add(contract);
                AppContext.SaveChanges();
                ret.Id = contract.Id;
            }
            return ret;
        }

        private void FillContract(OBKContractViewModel contractViewModel, OBK_Contract obkContract)
        {
            obkContract.Type = contractViewModel.Type != 0 ? contractViewModel.Type : 1;
            obkContract.ExpertOrganization = contractViewModel.ExpertOrganization;
            obkContract.Signer = contractViewModel.Signer;
            if (obkContract.DeclarantContactId != null)
            {
                var contactData = AppContext.OBK_DeclarantContact.Where(x => x.Id == obkContract.DeclarantContactId).FirstOrDefault();
                FillContactData(contractViewModel, contactData, obkContract);
                AppContext.SaveChanges();
            }
            else
            {
                OBK_DeclarantContact contactData = new OBK_DeclarantContact();
                contactData.Id = Guid.NewGuid();
                contactData.CreateDate = DateTime.Now;
                contactData.IsHasBossDocNumber = false;
                contactData.IsHasSignDocNumber = false;
                contactData.SignType = false;
                AppContext.OBK_DeclarantContact.Add(contactData);
                AppContext.SaveChanges();
                obkContract.DeclarantContactId = contactData.Id;
            }
            AppContext.SaveChanges();
        }

        private void FillContactData(OBKContractViewModel contractViewModel, OBK_DeclarantContact contactData, OBK_Contract obkContract)
        {
            contactData.AddressLegalRu = contractViewModel.AddressLegalRu;
            contactData.AddressLegalKz = contractViewModel.AddressLegalKz;
            contactData.AddressFact = contractViewModel.AddressFact;
            contactData.Phone = contractViewModel.Phone;
            contactData.Email = contractViewModel.Email;
            contactData.BossLastName = contractViewModel.BossLastName;
            contactData.BossFirstName = contractViewModel.BossFirstName;
            contactData.BossMiddleName = contractViewModel.BossMiddleName;
            contactData.BossPosition = contractViewModel.BossPosition;
            contactData.BossPositionKz = contractViewModel.BossPositionKz;
            contactData.BossDocType = contractViewModel.BossDocType;
            contactData.IsHasBossDocNumber = contractViewModel.IsHasBossDocNumber;
            contactData.BossDocNumber = contractViewModel.BossDocNumber;
            contactData.BossDocCreatedDate = contractViewModel.BossDocCreatedDate;
            contactData.BossDocEndDate = contractViewModel.BossDocEndDate;
            contactData.BossDocUnlimited = contractViewModel.BossDocUnlimited;
            contactData.SignerIsBoss = contractViewModel.SignerIsBoss;
            contactData.SignLastName = contractViewModel.SignLastName;
            contactData.SignFirstName = contractViewModel.SignFirstName;
            contactData.SignMiddleName = contractViewModel.SignMiddleName;
            contactData.SignPosition = contractViewModel.SignPosition;
            contactData.SignPositionKz = contractViewModel.SignPositionKz;
            contactData.SignDocType = contractViewModel.SignDocType;
            contactData.IsHasSignDocNumber = contractViewModel.IsHasSignDocNumber;
            contactData.SignDocNumber = contractViewModel.SignDocNumber;
            contactData.SignDocCreatedDate = contractViewModel.SignDocCreatedDate;
            contactData.SignDocEndDate = contractViewModel.SignDocEndDate;
            contactData.SignDocUnlimited = contractViewModel.SignDocUnlimited;
            contactData.BankIik = contractViewModel.BankIik;
            contactData.BankBik = contractViewModel.BankBik;
            contactData.CurrencyId = contractViewModel.CurrencyId;
            contactData.BankNameRu = contractViewModel.BankNameRu;
            contactData.BankNameKz = contractViewModel.BankNameKz;
            contactData.DeclarantId = obkContract.DeclarantId;
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
                var v = AppContext.OBK_Contract.Where(x => x.EmployeeId == employeeId).AsQueryable();
                //var v = AppContext.OBK_Contract.AsQueryable();

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
                    v = v.OrderByDescending(x => x.CreatedDate);
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

        public object GetService(Guid serivce)
        {
            var service = AppContext.OBK_Ref_PriceList.Where(x => x.Id == serivce).Select(x => new { x.Id, x.Price, UnitOfMeasurementName = x.Dictionary.Name, UnitOfMeasurementId = x.Dictionary.Id }).FirstOrDefault();
            return service;
        }

        public OBKContractViewModel LoadContract(Guid id)
        {
            var OBKContract = AppContext.OBK_Contract.Where(x => x.Id == id).FirstOrDefault();

            OBKContractViewModel contractViewModel = new OBKContractViewModel();
            contractViewModel.Id = OBKContract.Id;
            contractViewModel.Type = OBKContract.Type;
            contractViewModel.ExpertOrganization = OBKContract.ExpertOrganization;
            contractViewModel.Signer = OBKContract.Signer;
            contractViewModel.Status = OBKContract.Status;

            //if (OBKContract.DeclarantId != null)
            //{
            //    if (OBKContract.OBK_Declarant.IsConfirmed)
            //    {
            //        contractViewModel.DeclarantId = OBKContract.DeclarantId;
            //    }
            //    else
            //    {
            //        contractViewModel.DeclarantId = Guid.Empty;
            //    }
            //    contractViewModel.DeclarantIsResident = OBKContract.OBK_Declarant.IsResident;
            //    contractViewModel.DeclarantOrganizationFormId = OBKContract.OBK_Declarant.OrganizationFormId;
            //    contractViewModel.DeclarantBin = OBKContract.OBK_Declarant.Bin;
            //    contractViewModel.DeclarantNameKz = OBKContract.OBK_Declarant.NameKz;
            //    contractViewModel.DeclarantNameRu = OBKContract.OBK_Declarant.NameRu;
            //    contractViewModel.DeclarantNameEn = OBKContract.OBK_Declarant.NameEn;
            //    contractViewModel.DeclarantCountryId = OBKContract.OBK_Declarant.CountryId;
            //}

            if (OBKContract.DeclarantContactId != null)
            {
                contractViewModel.AddressLegalRu = OBKContract.OBK_DeclarantContact.AddressLegalRu;
                contractViewModel.AddressLegalKz = OBKContract.OBK_DeclarantContact.AddressLegalKz;
                contractViewModel.AddressFact = OBKContract.OBK_DeclarantContact.AddressFact;
                contractViewModel.Phone = OBKContract.OBK_DeclarantContact.Phone;
                contractViewModel.Email = OBKContract.OBK_DeclarantContact.Email;
                contractViewModel.BossLastName = OBKContract.OBK_DeclarantContact.BossLastName;
                contractViewModel.BossFirstName = OBKContract.OBK_DeclarantContact.BossFirstName;
                contractViewModel.BossMiddleName = OBKContract.OBK_DeclarantContact.BossMiddleName;
                contractViewModel.BossPosition = OBKContract.OBK_DeclarantContact.BossPosition;
                contractViewModel.BossPositionKz = OBKContract.OBK_DeclarantContact.BossPositionKz;
                contractViewModel.BossDocType = OBKContract.OBK_DeclarantContact.BossDocType;
                contractViewModel.IsHasBossDocNumber = OBKContract.OBK_DeclarantContact.IsHasBossDocNumber;
                contractViewModel.BossDocNumber = OBKContract.OBK_DeclarantContact.BossDocNumber;
                contractViewModel.BossDocCreatedDate = OBKContract.OBK_DeclarantContact.BossDocCreatedDate;
                contractViewModel.BossDocEndDate = OBKContract.OBK_DeclarantContact.BossDocEndDate;
                contractViewModel.BossDocUnlimited = OBKContract.OBK_DeclarantContact.BossDocUnlimited;
                contractViewModel.SignerIsBoss = OBKContract.OBK_DeclarantContact.SignerIsBoss;
                contractViewModel.SignLastName = OBKContract.OBK_DeclarantContact.SignLastName;
                contractViewModel.SignFirstName = OBKContract.OBK_DeclarantContact.SignFirstName;
                contractViewModel.SignMiddleName = OBKContract.OBK_DeclarantContact.SignMiddleName;
                contractViewModel.SignPosition = OBKContract.OBK_DeclarantContact.SignPosition;
                contractViewModel.SignPositionKz = OBKContract.OBK_DeclarantContact.SignPositionKz;
                contractViewModel.SignDocType = OBKContract.OBK_DeclarantContact.SignDocType;
                contractViewModel.IsHasSignDocNumber = OBKContract.OBK_DeclarantContact.IsHasSignDocNumber;
                contractViewModel.SignDocNumber = OBKContract.OBK_DeclarantContact.SignDocNumber;
                contractViewModel.SignDocCreatedDate = OBKContract.OBK_DeclarantContact.SignDocCreatedDate;
                contractViewModel.SignDocEndDate = OBKContract.OBK_DeclarantContact.SignDocEndDate;
                contractViewModel.SignDocUnlimited = OBKContract.OBK_DeclarantContact.SignDocUnlimited;
                contractViewModel.BankIik = OBKContract.OBK_DeclarantContact.BankIik;
                contractViewModel.BankBik = OBKContract.OBK_DeclarantContact.BankBik;
                contractViewModel.CurrencyId = OBKContract.OBK_DeclarantContact.CurrencyId;
                contractViewModel.BankNameRu = OBKContract.OBK_DeclarantContact.BankNameRu;
                contractViewModel.BankNameKz = OBKContract.OBK_DeclarantContact.BankNameKz;
            }

            return contractViewModel;
        }

        public OBKContractProductViewModel SaveProduct(Guid contractId, OBKContractProductViewModel product)
        {
            var r = new OBKContractProductViewModel();
            r.ProductId = product.ProductId;
            if (product.Id != null)
            {
                OBK_RS_Products productInfo = AppContext.OBK_RS_Products.Where(x => x.Id == product.Id).FirstOrDefault();
                FillProduct(productInfo, product);
                AppContext.SaveChanges();

                DeleteSeries(productInfo.Id);
                SaveSeries(productInfo.Id, product.Series);

                DeleteMtParts(productInfo.Id);
                SaveMtParts(productInfo.Id, product.MtParts);

                r.Id = productInfo.Id;
            }
            else
            {
                OBK_RS_Products productInfo = new OBK_RS_Products();
                productInfo.ContractId = contractId;
                FillProduct(productInfo, product);
                AppContext.OBK_RS_Products.Add(productInfo);
                AppContext.SaveChanges();

                SaveSeries(productInfo.Id, product.Series);
                SaveMtParts(productInfo.Id, product.MtParts);

                r.Id = productInfo.Id;
            }
            return r;
        }

        public bool DeleteProduct(Guid contractId, int productId)
        {
            var product = AppContext.OBK_RS_Products.Where(x => x.Id == productId).FirstOrDefault();
            if (product != null)
            {
                AppContext.OBK_MtPart.RemoveRange(AppContext.OBK_MtPart.Where(x => x.ProductId == productId));
                AppContext.OBK_Procunts_Series.RemoveRange(AppContext.OBK_Procunts_Series.Where(x => x.OBK_RS_ProductsId == productId));
                AppContext.OBK_ContractPrice.RemoveRange(AppContext.OBK_ContractPrice.Where(x => x.ProductId == productId));
                AppContext.OBK_RS_Products.Remove(product);
                AppContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<OBKContractProductViewModel> GetProducts(Guid contractId)
        {
            return AppContext.OBK_RS_Products.Where(x => x.ContractId == contractId).Select(x => new OBKContractProductViewModel
            {
                ProductId = null,
                Id = x.Id,
                RegTypeId = x.RegTypeId,
                DegreeRiskId = x.DegreeRiskId,
                NameRu = x.NameRu,
                NameKz = x.NameKz,
                ProducerNameRu = x.ProducerNameRu,
                ProducerNameKz = x.ProducerNameKz,
                CountryNameRu = x.CountryNameRu,
                CountryNameKz = x.CountryNameKZ,
                KpvedCode = x.KpvedCode,
                TnvedCode = x.TnvedCode,
                Price = x.Price,
                DrugFormBoxCount = x.DrugFormBoxCount,
                DrugFormFullName = x.DrugFormFullName,
                DrugFormFullNameKz = x.DrugFormFullNameKz,
                Series = AppContext.OBK_Procunts_Series.Where(y => y.OBK_RS_ProductsId == x.Id).Select(y => new OBKContractSeriesViewModel
                {
                    Id = y.Id,
                    Series = y.Series,
                    CreateDate = y.SeriesStartdate,
                    ExpireDate = y.SeriesEndDate,
                    Part = y.SeriesParty,
                    UnitId = y.SeriesMeasureId,
                    UnitName = y.sr_measures.short_name
                }).ToList()
                ,
                MtParts = AppContext.OBK_MtPart.Where(y => y.ProductId == x.Id).Select(y => new OBKContractMtPartViewModel
                {
                    Id = y.Id,
                    PartNumber = y.PartNumber,
                    Model = y.Model,
                    Specification = y.Specification,
                    SpecificationKz = y.SpecificationKz,
                    Name = y.Name,
                    NameKz = y.NameKz,
                    ProducerName = y.ProducerName,
                    CountryName = y.CountryName,
                    ProducerNameKz = y.ProducerNameKz,
                    CountryNameKz = y.CountryNameKz
                }).ToList()
            }).ToList();
        }

        public OBKContractServiceViewModel SaveContractPrice(Guid contractId, OBKContractServiceViewModel service)
        {
            var s = new OBKContractServiceViewModel();
            if (service.Id != null)
            {
                OBK_ContractPrice serviceInfo = AppContext.OBK_ContractPrice.Where(x => x.Id == service.Id).FirstOrDefault();
                FillContractPrice(serviceInfo, service);
                AppContext.SaveChanges();
                s.Id = serviceInfo.Id;
            }
            else
            {
                OBK_ContractPrice serviceInfo = new OBK_ContractPrice();
                serviceInfo.Id = Guid.NewGuid();
                serviceInfo.ContractId = contractId;
                FillContractPrice(serviceInfo, service);
                AppContext.OBK_ContractPrice.Add(serviceInfo);
                AppContext.SaveChanges();
                s.Id = serviceInfo.Id;
            }
            return s;
        }

        public bool DeleteContractPrice(Guid contractId, Guid? serviceId)
        {
            var service = AppContext.OBK_ContractPrice.Where(x => x.Id == serviceId).FirstOrDefault();
            if (service != null)
            {
                AppContext.OBK_ContractPrice.Remove(service);
                AppContext.SaveChanges();
                return true;
            }
            return false;
        }

        private void FillContractPrice(OBK_ContractPrice serviceInfo, OBKContractServiceViewModel service)
        {
            serviceInfo.PriceRefId = service.ServiceId;
            serviceInfo.PriceWithoutTax = service.PriceWithoutTax;
            serviceInfo.Count = service.Count;
            serviceInfo.PriceWithoutTax = service.FinalCostWithoutTax;
            serviceInfo.PriceWithTax = service.FinalCostWithTax;
            serviceInfo.ProductId = service.ProductId;
        }

        private void SaveSeries(int productId, List<OBKContractSeriesViewModel> list)
        {
            if (list != null && list.Count > 0)
            {
                foreach (OBKContractSeriesViewModel item in list)
                {
                    OBK_Procunts_Series newSerie = new OBK_Procunts_Series();
                    newSerie.Series = item.Series;
                    newSerie.SeriesStartdate = item.CreateDate;
                    newSerie.SeriesEndDate = item.ExpireDate;
                    newSerie.SeriesParty = item.Part;
                    newSerie.SeriesMeasureId = item.UnitId;
                    newSerie.OBK_RS_ProductsId = productId;
                    AppContext.OBK_Procunts_Series.Add(newSerie);
                    AppContext.SaveChanges();
                }
            }
        }

        private void DeleteSeries(int productId)
        {
            AppContext.OBK_Procunts_Series.RemoveRange(AppContext.OBK_Procunts_Series.Where(x => x.OBK_RS_ProductsId == productId));
            AppContext.SaveChanges();
        }

        private void SaveMtParts(int productId, List<OBKContractMtPartViewModel> list)
        {
            if (list != null && list.Count > 0)
            {
                foreach (OBKContractMtPartViewModel item in list)
                {
                    OBK_MtPart mtPart = new OBK_MtPart();
                    mtPart.Id = Guid.NewGuid();
                    mtPart.ProductId = productId;
                    mtPart.PartNumber = item.PartNumber;
                    mtPart.Model = item.Model;
                    mtPart.Specification = item.Specification;
                    mtPart.SpecificationKz = item.SpecificationKz;
                    mtPart.Name = item.Name;
                    mtPart.NameKz = item.NameKz;
                    mtPart.ProducerName = item.ProducerName;
                    mtPart.CountryName = item.CountryName;
                    mtPart.ProducerNameKz = item.ProducerNameKz;
                    mtPart.CountryNameKz = item.CountryNameKz;

                    AppContext.OBK_MtPart.Add(mtPart);
                    AppContext.SaveChanges();
                }
            }
        }

        private void DeleteMtParts(int productId)
        {
            AppContext.OBK_MtPart.RemoveRange(AppContext.OBK_MtPart.Where(x => x.ProductId == productId));
            AppContext.SaveChanges();
        }

        private void FillProduct(OBK_RS_Products productInfo, OBKContractProductViewModel product)
        {
            productInfo.RegTypeId = product.RegTypeId;
            productInfo.DegreeRiskId = product.DegreeRiskId;
            productInfo.NameRu = product.NameRu;
            productInfo.NameKz = product.NameKz;
            productInfo.ProducerNameRu = product.ProducerNameRu;
            productInfo.ProducerNameKz = product.ProducerNameKz;
            productInfo.CountryNameRu = product.CountryNameRu;
            productInfo.CountryNameKZ = product.CountryNameKz;
            productInfo.KpvedCode = product.KpvedCode;
            productInfo.TnvedCode = product.TnvedCode;
            productInfo.Price = product.Price;
            productInfo.DrugFormBoxCount = product.DrugFormBoxCount;
            productInfo.DrugFormFullName = product.DrugFormFullName;
            productInfo.DrugFormFullNameKz = product.DrugFormFullNameKz;
        }

        public List<OBKContractServiceViewModel> GetContractPrices(Guid contractId)
        {
            var list = AppContext.OBK_ContractPrice.Where(x => x.ContractId == contractId).Select(x => new OBKContractServiceViewModel
            {
                Id = x.Id,
                ServiceName = x.OBK_Ref_PriceList.NameRu,
                ServiceId = x.PriceRefId,
                UnitOfMeasurementId = x.OBK_Ref_PriceList.UnitId,
                UnitOfMeasurementName = x.OBK_Ref_PriceList.Dictionary.Name,
                PriceWithoutTax = x.OBK_Ref_PriceList.Price,
                Count = x.Count,
                FinalCostWithoutTax = x.PriceWithoutTax,
                FinalCostWithTax = x.PriceWithTax,
                ProductId = x.ProductId,
                ProductName = x.OBK_RS_Products.NameRu
            }
            ).ToList();
            return list;
        }

        public OBK_Declarant FindOrganization(string bin)
        {
            var declarant = AppContext.OBK_Declarant.FirstOrDefault(x => x.Bin == bin && x.IsConfirmed && x.IsResident);
            return declarant;
        }

        public OBK_Declarant ContractDeclarantSave(Guid guid, OBKDeclarantViewModel declarantViewModel, out bool declarantAlreadyExist)
        {
            declarantAlreadyExist = false;

            if (declarantViewModel.Id != null)
            {
                OBK_Declarant declarant = AppContext.OBK_Declarant.Where(x => x.Id == declarantViewModel.Id).FirstOrDefault();
                if (declarant != null && !declarant.IsConfirmed)
                {
                    declarant.OrganizationFormId = declarantViewModel.OrganizationFormId;
                    declarant.NameKz = declarantViewModel.NameKz;
                    declarant.NameRu = declarantViewModel.NameRu;
                    declarant.NameEn = declarantViewModel.NameEn;
                    declarant.CountryId = declarantViewModel.CountryId;
                    declarant.IsResident = declarantViewModel.IsResident;
                    if (declarant.IsResident)
                    {
                        declarant.Bin = declarantViewModel.Bin;
                    }
                    else
                    {
                        declarant.Bin = null;
                    }

                    AppContext.SaveChanges();
                    return declarant;
                }
            }
            else
            {
                if (declarantViewModel.IsResident)
                {
                    declarantAlreadyExist = DeclarantExist(declarantViewModel.Id, declarantViewModel.Bin);
                }
                else
                {
                    declarantAlreadyExist = DeclarantExist(declarantViewModel.Id, declarantViewModel.NameRu, declarantViewModel.CountryId);
                }

                if (!declarantAlreadyExist)
                {
                    var contract = AppContext.OBK_Contract.Where(x => x.Id == guid).FirstOrDefault();
                    if (contract != null)
                    {
                        if (contract.DeclarantId != null)
                        {
                            var existingDeclarant = AppContext.OBK_Declarant.Where(x => x.Id == contract.DeclarantId).FirstOrDefault();
                            if (!existingDeclarant.IsConfirmed)
                            {
                                AppContext.OBK_Declarant.Remove(existingDeclarant);
                                contract.DeclarantId = null;
                                AppContext.SaveChanges();
                            }
                        }

                        OBK_Declarant declarant = new OBK_Declarant();
                        declarant.Id = Guid.NewGuid();
                        declarant.IsConfirmed = false;
                        declarant.OrganizationFormId = declarantViewModel.OrganizationFormId;
                        declarant.NameKz = declarantViewModel.NameKz;
                        declarant.NameRu = declarantViewModel.NameRu;
                        declarant.NameEn = declarantViewModel.NameEn;
                        declarant.CountryId = declarantViewModel.CountryId;
                        declarant.IsResident = declarantViewModel.IsResident;
                        if (declarant.IsResident)
                        {
                            declarant.Bin = declarantViewModel.Bin;
                        }
                        else
                        {
                            declarant.Bin = null;
                        }
                        AppContext.OBK_Declarant.Add(declarant);
                        AppContext.SaveChanges();
                        contract.DeclarantId = declarant.Id;
                        var contact = AppContext.OBK_DeclarantContact.Where(x => x.Id == contract.DeclarantContactId).FirstOrDefault();
                        if (contact != null)
                        {
                            contact.DeclarantId = contract.DeclarantId;
                        }
                        AppContext.SaveChanges();
                        return declarant;
                    }
                }
            }
            return null;
        }

        private bool DeclarantExist(Guid? id, string bin)
        {
            var exist = false;
            if (!string.IsNullOrEmpty(bin))
            {
                var declrant = id != null ?
                    AppContext.OBK_Declarant.Where(x => x.IsConfirmed && x.Bin == bin && x.IsResident == true && x.Id != id).FirstOrDefault()
                    :
                    AppContext.OBK_Declarant.Where(x => x.IsConfirmed && x.Bin == bin && x.IsResident == true).FirstOrDefault();
                if (declrant != null)
                {
                    exist = true;
                }
            }
            return exist;
        }

        private bool DeclarantExist(Guid? id, string nameRu, Guid? countryId)
        {
            var exist = false;
            var declarant = id != null ?
                AppContext.OBK_Declarant.Where(x => x.IsConfirmed && x.NameRu == nameRu && x.CountryId == countryId && x.IsResident == false && x.Id != id).FirstOrDefault()
                :
                AppContext.OBK_Declarant.Where(x => x.IsConfirmed && x.NameRu == nameRu && x.CountryId == countryId && x.IsResident == false).FirstOrDefault();
            if (declarant != null)
            {
                exist = true;
            }
            return exist;
        }

        public bool SaveDeclarant(Guid contractId, Guid declarantId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            if (contract != null)
            {
                if (contract.DeclarantId != null)
                {
                    var existingDeclarant = AppContext.OBK_Declarant.Where(x => x.Id == contract.DeclarantId).FirstOrDefault();
                    if (!existingDeclarant.IsConfirmed)
                    {
                        var contacts = AppContext.OBK_DeclarantContact.Where(x => x.DeclarantId == contract.DeclarantId);
                        foreach (var contact in contacts)
                        {
                            contact.DeclarantId = null;
                        }

                        AppContext.OBK_Declarant.Remove(existingDeclarant);
                        contract.DeclarantId = null;
                        AppContext.SaveChanges();
                    }
                }

                contract.DeclarantId = declarantId;

                if (contract.DeclarantContactId != null)
                {
                    var contact = AppContext.OBK_DeclarantContact.Where(x => x.Id == contract.DeclarantContactId).FirstOrDefault();
                    if (contact != null)
                    {
                        contact.DeclarantId = contract.DeclarantId;
                    }
                }

                AppContext.SaveChanges();
            }
            return true;
        }

        public bool DeleteDeclarant(Guid contractId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            if (contract != null)
            {
                bool declarantRemoved = false;
                if (contract.DeclarantId != null)
                {
                    var existingDeclarant = AppContext.OBK_Declarant.Where(x => x.Id == contract.DeclarantId).FirstOrDefault();
                    if (!existingDeclarant.IsConfirmed)
                    {
                        var contacts = AppContext.OBK_DeclarantContact.Where(x => x.DeclarantId == contract.DeclarantId);
                        foreach (var contact in contacts)
                        {
                            contact.DeclarantId = null;
                        }

                        AppContext.OBK_Declarant.Remove(existingDeclarant);
                        contract.DeclarantId = null;
                        AppContext.SaveChanges();
                        declarantRemoved = true;
                    }
                }

                if (!declarantRemoved)
                {
                    contract.DeclarantId = null;
                    var contacts = AppContext.OBK_DeclarantContact.Where(x => x.DeclarantId == contract.DeclarantId);
                    foreach (var contact in contacts)
                    {
                        contact.DeclarantId = null;
                    }

                    AppContext.SaveChanges();
                }
            }
            return true;
        }

        public object GetNamesNonResidents(Guid? countryId)
        {
            var noData = new { Id = Guid.Empty, Name = "Нет данных", NameKz = "Нет данных" };
            if (countryId != null)
            {
                var obkDeclarants = AppContext.OBK_Declarant.Where(x => x.CountryId == countryId && x.IsConfirmed == true && x.IsResident == false).OrderBy(x => x.NameRu).Select(x => new { x.Id, Name = x.NameRu, x.NameKz }).ToList();
                var list = new[] { noData }.ToList().Concat(obkDeclarants);
                return list;
            }
            else
            {
                var list = new[] { noData }.ToList();
                return list;
            }
        }

        public bool ClearContactData(Guid contractId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            if (contract != null)
            {
                if (contract.DeclarantContactId != null)
                {
                    var declarantContact = AppContext.OBK_DeclarantContact.Where(x => x.Id == contract.DeclarantContactId).FirstOrDefault();
                    if (declarantContact != null)
                    {
                        declarantContact.AddressLegalRu = null;
                        declarantContact.AddressLegalKz = null;
                        declarantContact.AddressFact = null;
                        declarantContact.Phone = null;
                        declarantContact.Email = null;
                        declarantContact.BossLastName = null;
                        declarantContact.BossFirstName = null;
                        declarantContact.BossMiddleName = null;
                        declarantContact.BossPosition = null;
                        declarantContact.BossPositionKz = null;
                        declarantContact.BossDocType = null;
                        declarantContact.IsHasBossDocNumber = false;
                        declarantContact.BossDocNumber = null;
                        declarantContact.BossDocCreatedDate = null;
                        declarantContact.BossDocEndDate = null;
                        declarantContact.BossDocUnlimited = false;
                        declarantContact.SignLastName = null;
                        declarantContact.SignFirstName = null;
                        declarantContact.SignMiddleName = null;
                        declarantContact.SignPosition = null;
                        declarantContact.SignPositionKz = null;
                        declarantContact.SignDocType = null;
                        declarantContact.IsHasSignDocNumber = false;
                        declarantContact.SignDocNumber = null;
                        declarantContact.SignDocCreatedDate = null;
                        declarantContact.SignDocEndDate = null;
                        declarantContact.SignDocUnlimited = false;
                        declarantContact.BankIik = null;
                        declarantContact.BankBik = null;
                        declarantContact.CurrencyId = null;
                        declarantContact.BankNameRu = null;
                        declarantContact.BankNameKz = null;
                        declarantContact.DeclarantId = null;
                        AppContext.SaveChanges();
                    }
                }
            }
            return true;
        }

        public OBKDeclarantViewModel GetDeclarant(Guid contractId)
        {
            OBKDeclarantViewModel declarantViewModel = null;
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            if (contract != null && contract.DeclarantId != null)
            {
                var declarant = AppContext.OBK_Declarant.Where(x => x.Id == contract.DeclarantId).FirstOrDefault();
                if (declarant != null)
                {
                    declarantViewModel = new OBKDeclarantViewModel()
                    {
                        Id = declarant.Id,
                        OrganizationFormId = declarant.OrganizationFormId,
                        NameKz = declarant.NameKz,
                        NameRu = declarant.NameRu,
                        NameEn = declarant.NameEn,
                        CountryId = declarant.CountryId,
                        Bin = declarant.Bin,
                        IsResident = declarant.IsResident,
                        IsConfirmed = declarant.IsConfirmed
                    };
                }
            }
            return declarantViewModel;
        }

        public List<OBKDrugFormViewModel> GetDrugForms(int productId)
        {
            var drugForms = AppContext.sr_drug_forms.Where(x => x.register_id == productId).Select(y => new OBKDrugFormViewModel
            {
                Id = y.id,
                RegisterId = y.register_id,
                BoxCount = y.box_count,
                FullName = y.full_name,
                FullNameKz = y.full_name_kz
            }).ToList();
            return drugForms;
        }

        public List<OBKMtPartViewModel> GetMtParts(int productId)
        {
            var meParts = AppContext.sr_register_mt_parts.Where(x => x.register_id == productId).Select(y => new OBKMtPartViewModel
            {
                Id = y.id,
                RegisterId = y.register_id,
                Name = y.name,
                NameKz = y.name_kz,
                PartNumber = y.part_number,
                Model = y.model,
                Specification = y.specification,
                SpecificationKz = y.specification_kz,
                ProducerName = y.producer_name,
                CountryName = y.country_name,
                ProducerNameKz = y.producer_name_kz,
                CountryNameKz = y.country_name_kz,

            }).ToList();
            return meParts;
        }

        public IQueryable<OBK_ContractRegisterView> GetContracts()
        {
            //List<OBKContractViewModel> list = AppContext.OBK_Contract.AsNoTracking().Where(x => x.Status == PW.Ncels.Database.Constants.CodeConstManager.STATUS_OBK_INPROCESSING).Select(x =>
            //new OBKContractViewModel
            //{
            //    Id = x.Id,
            //    Number = x.Number,
            //    CreatedDate = x.CreatedDate,
            //    StartDate = x.StartDate,
            //    EndDate = x.EndDate,
            //    DeclarantNameRu = x.OBK_Declarant.NameRu
            //}).ToList();

            var emp = UserHelper.GetCurrentEmployee();

            var list = AppContext.OBK_ContractRegisterView.Where(x => x.ExecutorId == emp.Id).AsQueryable();

            //var stages = AppContext.OBK_ContractStage.Where(x => x.Employees.Contains(emp)).AsQueryable();


            return list;
        }

        public int SendContractInProcessing(Guid contractId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            contract.Status = Constants.CodeConstManager.STATUS_OBK_INPROCESSING;

            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.New);

            var obkContractStageCoz = new OBK_ContractStage()
            {
                Id = Guid.NewGuid(),
                ContractId = contractId,
                StageId = CodeConstManager.STAGE_OBK_COZ,
                StageStatusId = stageStatus.Id,
                ParentStageId = null,
                ResultId = null
            };

            // Руководитель ЦОЗ
            Guid bossCozGuid = new Guid("3100E850-F7D8-48A4-A5AC-4BF5D50D98D2");
            var bossCozEmployee = AppContext.Employees.Where(x => x.Id == bossCozGuid).FirstOrDefault();

            obkContractStageCoz.Employees.Add(bossCozEmployee);

            AppContext.OBK_ContractStage.Add(obkContractStageCoz);

            AppContext.SaveChanges();

            var obkContractStageUOBK = new OBK_ContractStage()
            {
                Id = Guid.NewGuid(),
                ContractId = contractId,
                StageId = CodeConstManager.STAGE_OBK_UOBK,
                StageStatusId = stageStatus.Id,
                ParentStageId = obkContractStageCoz.Id,
                ResultId = null
            };

            // Руководитель УОБК
            Guid bossUobkGuid = new Guid("14D1A1F0-9501-4232-9C29-E9C394D88784");
            var bossUobkEmployee = AppContext.Employees.Where(x => x.Id == bossUobkGuid).FirstOrDefault();

            obkContractStageUOBK.Employees.Add(bossUobkEmployee);

            AppContext.OBK_ContractStage.Add(obkContractStageUOBK);

            AppContext.SaveChanges();

            return contract.Status;
        }

        public OBK_ContractCom GetComments(Guid modelId, string idControl)
        {
            return
                AppContext.OBK_ContractCom.FirstOrDefault(
                    e => e.ControlId == idControl && modelId == e.ContractId);
        }

        public void SaveComment(string modelId, string idControl, bool isError, string comment, string fieldValue,
            string userId, string fieldDisplay)
        {
            var entityId = new Guid(modelId);
            var model =
                AppContext.OBK_ContractCom.FirstOrDefault(
                    e => e.ControlId == idControl && e.ContractId.Equals(entityId)) ??
                new OBK_ContractCom
                {
                    DateCreate = DateTime.Now,
                    ContractId = entityId,
                    ControlId = idControl,
                };

            model.IsError = isError;
            model.OBK_ContractComRecord.Add(new OBK_ContractComRecord
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Note = comment,
                UserId = new Guid(userId),
                OBK_ContractCom = model,
                ValueField = fieldValue,
                DisplayField = fieldDisplay
            });
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                AppContext.OBK_ContractCom.Add(model);
            }
            AppContext.SaveChanges();
        }

        public OBK_ContractPriceCom GetCommentsPrice(Guid contractPriceId)
        {
            return
                AppContext.OBK_ContractPriceCom.FirstOrDefault(
                    e => contractPriceId == e.ContractPriceId);
        }

        public void SaveCommentPrice(string contractPriceId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            var entityId = new Guid(contractPriceId);
            var model =
                AppContext.OBK_ContractPriceCom.FirstOrDefault(
                    e => e.ContractPriceId.Equals(entityId)) ??
                new OBK_ContractPriceCom
                {
                    DateCreate = DateTime.Now,
                    ContractPriceId = entityId
                };

            model.IsError = isError;
            model.OBK_ContractPriceComRecord.Add(new OBK_ContractPriceComRecord
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Note = comment,
                UserId = new Guid(userId),
                OBK_ContractPriceCom = model,
                ValueField = fieldValue,
                DisplayField = fieldDisplay
            });
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                AppContext.OBK_ContractPriceCom.Add(model);
            }
            AppContext.SaveChanges();
        }

        public OBK_RS_ProductsCom GetCommentsProduct(int productId)
        {
            return
                AppContext.OBK_RS_ProductsCom.FirstOrDefault(
                    e => productId == e.ProductId);
        }

        public void SaveCommentProduct(string productId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            var entityId = int.Parse(productId);
            var model =
                AppContext.OBK_RS_ProductsCom.FirstOrDefault(
                    e => e.ProductId.Equals(entityId)) ??
                new OBK_RS_ProductsCom
                {
                    DateCreate = DateTime.Now,
                    ProductId = entityId
                };

            model.IsError = isError;
            model.OBK_RS_ProductsComRecord.Add(new OBK_RS_ProductsComRecord
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Note = comment,
                UserId = new Guid(userId),
                OBK_RS_ProductsCom = model,
                ValueField = fieldValue,
                DisplayField = fieldDisplay
            });
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                AppContext.OBK_RS_ProductsCom.Add(model);
            }
            AppContext.SaveChanges();
        }

        public OBK_Products_SeriesCom GetCommentsProductsSerie(int productSerieId)
        {
            return
               AppContext.OBK_Products_SeriesCom.FirstOrDefault(
                   e => productSerieId == e.ProductSerieId);
        }

        public void SaveCommentProductsSerie(string productSerieId, bool isError, string comment, string fieldValue, string userId, string fieldDisplay)
        {
            var entityId = int.Parse(productSerieId);
            var model =
                AppContext.OBK_Products_SeriesCom.FirstOrDefault(
                    e => e.ProductSerieId.Equals(entityId)) ??
                new OBK_Products_SeriesCom
                {
                    DateCreate = DateTime.Now,
                    ProductSerieId = entityId
                };

            model.IsError = isError;
            model.OBK_Products_SeriesComRecord.Add(new OBK_Products_SeriesComRecord
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Note = comment,
                UserId = new Guid(userId),
                OBK_Products_SeriesCom = model,
                ValueField = fieldValue,
                DisplayField = fieldDisplay
            });
            if (model.Id == null || model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                AppContext.OBK_Products_SeriesCom.Add(model);
            }
            AppContext.SaveChanges();
        }

        public OBK_Ref_StageStatus GetStageStatusByCode(string code)
        {
            return AppContext.OBK_Ref_StageStatus.AsNoTracking().FirstOrDefault(e => e.Code == code);
        }

        public void SendToWork(Guid stageId, Guid executorId)
        {
            var stage = AppContext.OBK_ContractStage.Where(x => x.Id == stageId).FirstOrDefault();
            var executor = AppContext.Employees.Where(x => x.Id == executorId).FirstOrDefault();
            stage.Employees.Add(executor);
            stage.StageStatusId = GetStageStatusByCode(OBK_Ref_StageStatus.InWork).Id;

            var contract = AppContext.OBK_Contract.Where(x => x.Id == stage.ContractId).FirstOrDefault();
            contract.Status = CodeConstManager.STATUS_OBK_WORK;
            AppContext.SaveChanges();
        }

        public string GetContractTemplatePath(Guid contractId)
        {
            string templateName = null;
            var contract = AppContext.OBK_Contract.FirstOrDefault(e => e.Id == contractId);
            if (contract == null)
                return null;
            switch (contract.Type)
            {
                case 1:
                    templateName = "ObkContractSerial.mrt";
                    break;
                case 2:
                    templateName = "ObkContractParty.mrt";
                    break;
                case 3:
                    templateName = "ObkContractDeclaration.mrt";
                    break;
            }
            return System.Web.HttpContext.Current.Server.MapPath("~/Reports/Mrts/OBK/" + templateName);
        }

        public string GetPriceCount(Guid contractId)
        {
            var count = AppContext.OBK_ContractPrice.Where(e => e.ContractId == contractId).ToList();
            return count.Sum(e => e.PriceWithTax).ToString();
        }

        public IQueryable<object> GetSigners()
        {
            string[] signerCodes = { "ncels_deputyceo", "ncels_ceo" };
            var items = AppContext.Employees.Where(e => signerCodes.Contains(e.Position.Code)).Select(e => new
            {
                e.Id,
                Name = e.Position.ShortName + " " + e.ShortName
            });
            return items;
        }

        public IQueryable<object> GetExpertOrganizations()
        {
            var items = AppContext.Units.Where(x => x.Code == "00").Select(x => new
            {
                Id = x.Id,
                Name = x.ShortName
            });
            return items;
        }

        public bool MeetsRequirements(Guid contractId)
        {
            var employee = UserHelper.GetCurrentEmployee();
            var employeeId = employee.Id;
            var contractViews = AppContext.OBK_ContractRegisterView.Where(x => x.ContractId == contractId && x.ExecutorId == employeeId).ToList();
            foreach (var contractView in contractViews)
            {
                //var stageStatusCompleted = GetStageStatusByCode(OBK_Ref_StageStatus.Completed);
                var stage = AppContext.OBK_ContractStage.Where(x => x.Id == contractView.ContractStageId).FirstOrDefault();
                stage.ResultId = CodeConstManager.OBK_RESULT_ID_MEETS_REQUIREMENTS;
                //stage.StageStatusId = stageStatusCompleted.Id;

                if (stage.StageId == CodeConstManager.STAGE_OBK_UOBK)
                {
                    CreateStageDef(contractId, stage);
                }

                AppContext.SaveChanges();
            }
            return true;
        }

        public bool DoestNotMeetRequirements(Guid contractId)
        {
            var employee = UserHelper.GetCurrentEmployee();
            var employeeId = employee.Id;
            var contractViews = AppContext.OBK_ContractRegisterView.Where(x => x.ContractId == contractId && x.ExecutorId == employeeId).ToList();
            foreach (var contractView in contractViews)
            {
                //var stageStatusCompleted = GetStageStatusByCode(OBK_Ref_StageStatus.Completed);
                var stage = AppContext.OBK_ContractStage.Where(x => x.Id == contractView.ContractStageId).FirstOrDefault();
                stage.ResultId = CodeConstManager.OBK_RESULT_ID_DOES_NOT_MEET_REQUIREMENTS;
                //stage.StageStatusId = stageStatusCompleted.Id;

                if (stage.StageId == CodeConstManager.STAGE_OBK_UOBK)
                {
                    CreateStageDef(contractId, stage);
                }

                AppContext.SaveChanges();
            }
            return true;
        }

        private void CreateStageDef(Guid contractId, OBK_ContractStage parentStage)
        {
            var stageStatusInWork = GetStageStatusByCode(OBK_Ref_StageStatus.InWork);
            OBK_ContractStage stageDef = new OBK_ContractStage()
            {
                Id = Guid.NewGuid(),
                ContractId = contractId,
                StageId = CodeConstManager.STAGE_OBK_DEF,
                StageStatusId = stageStatusInWork.Id,
                ParentStageId = parentStage.Id,
                ResultId = null
            };

            // Руководитель ДЭФ
            var bossDefGuid = new Guid("C9746027-F617-4791-8EEE-1CD80F2EDD5B");
            var bossDefEmployee = AppContext.Employees.Where(x => x.Id == bossDefGuid).FirstOrDefault();

            stageDef.Employees.Add(bossDefEmployee);
            AppContext.OBK_ContractStage.Add(stageDef);
        }

        public bool ReturnToApplicant(Guid contractId)
        {
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            contract.Status = CodeConstManager.STATUS_OBK_ONCORRECTION;

            var stages = AppContext.OBK_ContractStage.Where(x => x.ContractId == contractId).ToList();
            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.OnCorrection);
            foreach (var dtage in stages)
            {
                dtage.StageStatusId = stageStatus.Id;
            }

            AppContext.SaveChanges();

            return true;
        }

        public bool SendToBossForApproval(Guid contractId)
        {
            var stages = AppContext.OBK_ContractStage.Where(x => x.ContractId == contractId).ToList();
            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.OnAgreement);
            foreach (var dtage in stages)
            {
                dtage.StageStatusId = stageStatus.Id;
            }

            AppContext.SaveChanges();

            return true;
        }

        public bool ApproveContract(Guid contractId)
        {
            var stages = AppContext.OBK_ContractStage.Where(x => x.ContractId == contractId).ToList();
            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.RequiresRegistration);
            foreach (var dtage in stages)
            {
                dtage.StageStatusId = stageStatus.Id;
            }

            AppContext.SaveChanges();
            return true;
        }

        public bool RefuseApprovement(Guid contractId, string reason)
        {
            var stages = AppContext.OBK_ContractStage.Where(x => x.ContractId == contractId).ToList();
            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.NotAgreed);
            foreach (var dtage in stages)
            {
                dtage.StageStatusId = stageStatus.Id;
            }

            AddHistoryRefused(contractId, reason);

            AppContext.SaveChanges();

            return true;
        }

        private void AddHistoryRefused(Guid contractId, string reason)
        {
            var currentEmployee = UserHelper.GetCurrentEmployee();

            var status = GetContractHistoryStatusByCode(OBK_Ref_ContractHistoryStatus.Refused);

            var unitName = GetParentUnitName(currentEmployee);

            var history = new OBK_ContractHistory()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                RefuseReason = reason,
                ContractId = contractId,
                EmployeeId = currentEmployee.Id,
                UnitName = unitName,
                StatusId = status.Id,
            };
            AppContext.OBK_ContractHistory.Add(history);
        }

        public OBK_Ref_ContractHistoryStatus GetContractHistoryStatusByCode(string code)
        {
            return AppContext.OBK_Ref_ContractHistoryStatus.Where(x => x.Code == code).FirstOrDefault();
        }

        private string GetParentUnitName(Employee employee)
        {
            string unitName = null;
            if (employee.Units != null && employee.Units.Count > 0)
            {
                foreach (var unit in employee.Units)
                {
                    if (unit.Parent != null)
                    {
                        unitName = unit.Parent.ShortName;
                    }
                    break;
                }
            }
            return unitName;
        }

        public string GetRefuseReason(Guid contractId)
        {
            OBK_Ref_ContractHistoryStatus status = GetContractHistoryStatusByCode(OBK_Ref_ContractHistoryStatus.Refused);
            var history = AppContext.OBK_ContractHistory.Where(x => x.ContractId == contractId && x.StatusId == status.Id).OrderByDescending(x => x.Created).FirstOrDefault();
            if (history != null)
            {
                return history.RefuseReason;
            }
            return null;
        }

        public string RegisterContract(Guid contractId)
        {
            string number = GetLastNumberOfContract();
            var contract = AppContext.OBK_Contract.Where(x => x.Id == contractId).FirstOrDefault();
            contract.Number = number;
            AppContext.SaveChanges();
            return contract.Number;
        }

        private string GetLastNumberOfContract()
        {
            string number = "1";
            //var contract = AppContext.OBK_Contract.Where(x => x.Number != null).OrderByDescending(x => int.Parse(x.Number)).FirstOrDefault();
            var numbers = AppContext.OBK_Contract.Select(x => x.Number).ToList();
            int temp;
            int contractNumber = numbers.Select(n => int.TryParse(n, out temp) ? temp : 0).Max();
            contractNumber++;
            number = contractNumber.ToString();
            return number;
        }

        public bool UploadContract(Guid contractId)
        {
            var stages = AppContext.OBK_ContractStage.Where(x => x.ContractId == contractId).ToList();
            var stageStatus = GetStageStatusByCode(OBK_Ref_StageStatus.Active);
            foreach (var dtage in stages)
            {
                dtage.StageStatusId = stageStatus.Id;
            }
            AppContext.SaveChanges();
            return true;
        }
    }
}
