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
    public interface IDisplayService : IServiceBase<Display>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid displayID);
        ApiReponse Save(Display chip, Guid displayID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class DisplayService : BaseService<Display>, IDisplayService
    {
        private readonly IDisplayRepository _displayRepository;
        private readonly IProductRepository _productRepository;
        private readonly RepositoryContext _repositoryContext;
        public DisplayService(IDisplayRepository displayRepository, RepositoryContext repositoryContext, IProductRepository productRepository) : base(displayRepository)
        {
            _displayRepository = displayRepository;
            _repositoryContext = repositoryContext;
            _productRepository = productRepository;
        }
        public ApiReponse GetAll()
        {
            try
            {
                var records = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using).OrderBy(r => r.Specifications).ToList();
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
        public ApiReponse GetById(Guid displayID)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.DisplayID == displayID).FirstOrDefault();
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

        public ApiReponse Save(Display display, Guid displayId)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (displayId != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.DisplayID == displayId && display.DisplayID == displayId).Any())
                    {
                        display.ModifiedDate = DateTime.Now;
                        Update(display);
                        result.Success = true;
                        result.Data = display.DisplayID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    display.DisplayID = Guid.NewGuid();
                    Create(display);
                    result.Success = true;
                    result.Data = display.DisplayID;
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
                    var result = _displayRepository.UpdateMultiple(listID, status);
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
                    foreach (var displayID in listID)
                    {
                        if (_productRepository.DeleteByDisplayID(displayID) == 0)
                        {
                            transaction.Rollback();
                            return new ApiReponse()
                            {
                                Success = false,
                                Data = HandleError.GenerateErrorDeleteFail()
                            };
                        }
                    }
                    var result = _displayRepository.Delete(listID);
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
