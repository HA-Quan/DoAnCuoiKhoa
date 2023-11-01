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
    public interface IMemoryService : IServiceBase<Memory>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid memoryID);
        ApiReponse Save(Memory memory, Guid memoryID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class MemoryService : BaseService<Memory>, IMemoryService
    {
        private readonly IMemoryRepository _memoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly RepositoryContext _repositoryContext;
        public MemoryService(IMemoryRepository memoryRepository, RepositoryContext repositoryContext, IProductRepository productRepository) : base(memoryRepository)
        {
            _memoryRepository = memoryRepository;
            _repositoryContext = repositoryContext;
            _productRepository = productRepository;
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
        public ApiReponse GetById(Guid memoryID)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.MemoryID == memoryID).FirstOrDefault();
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

        public ApiReponse Save(Memory memory, Guid memoryID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (memoryID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.MemoryID == memoryID && memory.MemoryID == memoryID).Any())
                    {
                        memory.ModifiedDate = DateTime.Now;
                        Update(memory);
                        result.Success = true;
                        result.Data = memory.MemoryID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    memory.MemoryID = Guid.NewGuid();
                    Create(memory);
                    result.Success = true;
                    result.Data = memory.MemoryID;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.Success = false;
                result.Data = HandleError.GenerateErrorResultException();
            }
            return result;
        }
        public ApiReponse UpdateMultiple(List<Guid> listID, bool status)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _memoryRepository.UpdateMultiple(listID, status);
                    if (result == listID.Count)
                    {
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = listID
                        };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = HandleError.GenerateErrorUpdateFail()
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
        public ApiReponse Delete(List<Guid> listID)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var memoryID in listID)
                    {
                        if (_productRepository.DeleteByMemoryID(memoryID) == 0)
                        {
                            transaction.Rollback();
                            return new ApiReponse()
                            {
                                Success = false,
                                Data = HandleError.GenerateErrorDeleteFail()
                            };
                        }
                    }
                    var result = _memoryRepository.Delete(listID);
                    if (result == listID.Count)
                    {
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = result
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
