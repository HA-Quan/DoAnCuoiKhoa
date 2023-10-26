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

namespace Services.Service
{
    public interface IImportProductService : IServiceBase<ImportProduct>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid importProductID);
        ApiReponse Save(ImportProduct importProduct, Guid importProductID);
    }

    internal class ImportProductService : BaseService<ImportProduct>, IImportProductService
    {
        private readonly IImportProductRepository _importProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly RepositoryContext _repositoryContext;
        public ImportProductService(IImportProductRepository importProductRepository, IProductRepository productRepository,
            RepositoryContext repositoryContext) : base(importProductRepository)
        {
            _importProductRepository = importProductRepository;
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
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ImportID == importProductID).FirstOrDefault();
                if (record == null)
                {
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultNotFoundByID()
                    };
                }
                return new ApiReponse()
                {
                    Success = true,
                    Data = record
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
        
        public ApiReponse Save(ImportProduct importProduct, Guid importProductID)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                bool check = true;
                try
                {
                    if (importProductID != Guid.Empty)
                    {
                        if (_repositoryBase.FindByCondition(p =>p.DelFalg == EnumType.DeleteFlag.Using && p.ImportID == importProductID).Any())
                        {
                            _repositoryBase.Update(importProduct);
                            _importProductRepository.Save();
                            return new ApiReponse()
                            {
                                Success = true,
                                Data = importProduct.ImportID
                            };
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
                        var product = _productRepository.FindByCondition(s => s.DelFalg == EnumType.DeleteFlag.Using 
                        && s.ProductID == importProduct.ProductID).FirstOrDefault();
                        if (product == null)
                        {
                            check = false;
                        }
                        else
                        {
                            product.Quantity += importProduct.Amount;
                            product.Inventory += importProduct.Amount;
                            _productRepository.Update(product);
                        }
                        importProduct.ImportID = Guid.NewGuid();
                        _repositoryBase.Create(importProduct);
                        _importProductRepository.Save();
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
       
    }

}
