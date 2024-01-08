using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Repository;
using Services.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface IImportProductService : IServiceBase<ImportProduct>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid importProductID);
        ApiReponse Save(ImportModel importModel, Guid importID);
        ApiReponse GetByFilter(DateTime? timeStart, DateTime? timeEnd, Guid supplier, int? sort, int pageSize, int pageNumber);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class ImportProductService : BaseService<ImportProduct>, IImportProductService
    {
        private readonly IImportProductRepository _importProductRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly RepositoryContext _repositoryContext;
        public ImportProductService(IImportProductRepository importProductRepository, IProductRepository productRepository,
            IImportDetailRepository importDetailRepository, RepositoryContext repositoryContext) : base(importProductRepository)
        {
            _importProductRepository = importProductRepository;
            _importDetailRepository = importDetailRepository;
            _productRepository = productRepository;
            _repositoryContext = repositoryContext;
        }
        public ApiReponse GetAll()
        {
            try
            {
                var records = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using).ToList();
                return new ApiReponse()
                {
                    Success = true,
                    Data = records
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }
        public ApiReponse GetById(Guid importProductID)
        {
            try
            {
                var record = new ImportView();
                var import = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ImportID == importProductID).FirstOrDefault();
                if (import != null)
                {
                    record.InfoImport = import;
                    var importDetails = _repositoryContext.ImportDetails.Where(o => o.ImportID == importProductID
                            && o.DelFalg == EnumType.DeleteFlag.Using).ToList();
                    var listImportDetail = new List<ImportDetailModel>();
                    foreach (var importDetail in importDetails)
                    {
                        var product = _repositoryContext.Products.Where(o => o.ProductID == importDetail.ProductID).FirstOrDefault();
                        var importDetailModel = new ImportDetailModel()
                        {
                            ImportDetailID = importDetail.ImportDetailID,
                            ImportID = importDetail.ImportID,
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            MainImage = product.MainImage,
                            Quantity = importDetail.Quantity,
                            Condition = product.Condition,
                            PriceImport = importDetail.PriceImport,
                            CreatedBy = importDetail.CreatedBy,
                            CreatedDate = importDetail.CreatedDate,
                            ModifiedBy = importDetail.ModifiedBy,
                            ModifiedDate = importDetail.ModifiedDate,
                        };
                        listImportDetail.Add(importDetailModel);
                    }
                    record.ListImportDetail = listImportDetail;
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = record
                    };
                }
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultNotFoundByID()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }

        public ApiReponse GetByFilter(DateTime? timeStart, DateTime? timeEnd, Guid supplier, int? sort, int pageSize, int pageNumber)
        {
            try
            {
                if (timeStart == null)
                {
                    timeStart = DateTime.MinValue;
                }
                if (timeEnd == null)
                {
                    timeEnd = DateTime.MaxValue;
                }

                return new ApiReponse()
                {
                    Success = true,
                    Data = _importProductRepository.GetByFilter((DateTime)timeStart, (DateTime)timeEnd, supplier, sort, pageSize, pageNumber)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }

        public ApiReponse Save(ImportModel importModel, Guid importID)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                bool check = true;
                ImportProduct importProduct = importModel.InfoImport;
                try
                {
                    if (importID != Guid.Empty)
                    {
                        if (FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                        p.ImportID == importID).Any() && importProduct.ImportID == importID)
                        {
                            if (!_repositoryContext.Suppliers.Any(s => s.DelFalg == EnumType.DeleteFlag.Using
                                && s.SupplierID == importProduct.SupplierID))
                            {
                                check = false;
                            }
                            importProduct.ModifiedDate = DateTime.Now;
                            Update(importProduct);
                            List<ImportDetail> importDetails = new List<ImportDetail>();
                            foreach (var importDetail in importModel.ListImportDetail)
                            {
                                var item = _repositoryContext.ImportDetails.AsNoTracking().FirstOrDefault(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                                g.ImportDetailID == importDetail.ImportDetailID);
                                if (item == null)
                                {
                                    importDetail.ImportDetailID = Guid.NewGuid();
                                    importDetail.CreatedBy = importProduct.CreatedBy;
                                    importDetail.ImportID = importProduct.ImportID;
                                }
                                else if (item != importDetail)
                                {
                                    importDetail.ModifiedBy = importProduct.ModifiedBy;
                                    importDetail.ModifiedDate = DateTime.Now;
                                }
                                importDetails.Add(importDetail);
                            }
                            if (_importDetailRepository.UpdateMultiple(importDetails, importID) == 0)
                            {
                                check = false;
                            }

                            if (check)
                            {
                                transaction.Commit();
                                return new ApiReponse()
                                {
                                    Success = true,
                                    Data = importProduct.ImportID
                                };
                            }
                            else
                            {
                                transaction.Rollback();
                                return new ApiReponse()
                                {
                                    Success = false,
                                    Data = Guid.Empty
                                };
                            }
                        }
                        else
                        {
                            return new ApiReponse()
                            {
                                Success = false,
                                Data = Guid.Empty
                            };
                        }
                    }
                    else
                    {
                        if(!_repositoryContext.Suppliers.Any(s=> s.DelFalg == EnumType.DeleteFlag.Using 
                        && s.SupplierID == importProduct.SupplierID))
                        {
                            check = false; 
                        }
                        _repositoryBase.Create(importProduct);
                        _importProductRepository.Save();
                        foreach(var item in importModel.ListImportDetail)
                        {
                            item.ImportID = importProduct.ImportID;
                            item.CreatedBy = importProduct.CreatedBy;
                        }
                        var saveImportDetail = _importDetailRepository.CreateMultiple(importModel.ListImportDetail);
                        if(saveImportDetail != importModel.ListImportDetail.Count)
                        {
                            check = false;
                        }
                        if (check)
                        {
                            transaction.Commit();
                            return new ApiReponse()
                            {
                                Success = true,
                                Data = importProduct.ImportID
                            };
                        }
                        else
                        {
                            transaction.Rollback();
                            return new ApiReponse()
                            {
                                Success = false,
                                Data = HandleError.GenerateErrorResultValidate()
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultException()
                    };
                }
            }
        }

        public ApiReponse Delete(List<Guid> listID)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var listImport = new List<ImportProduct>();
                    bool check = true;
                    foreach (var importID in listID)
                    {
                        var import = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ImportID == importID).FirstOrDefault();
                        if (import == null)
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            _importDetailRepository.DeleteByImportID(importID);
                            import.DelFalg = EnumType.DeleteFlag.Deleted;
                            import.ModifiedDate = DateTime.Now;
                            listImport.Add(import);
                        }
                    }
                    if (check)
                    {
                        _importProductRepository.UpdateMultiple(listImport);
                        _importProductRepository.Save();
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = listID.Count()
                        };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = HandleError.GenerateErrorDeleteFail()
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultException()
                    };
                }
            }
        }
    }

}
