using CommunityToolkit.HighPerformance;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Models.Resource;
using Services.Repository;
using Services.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface IGiftService : IServiceBase<Gift>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid giftID);
        ApiReponse GetByProductId(Guid productID);
        ApiReponse GetByFilter(string? keyword, int? sort, bool? status, int pageSize, int pageNumber);
        ApiReponse Save(Gift gift, Guid giftID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class GiftService : BaseService<Gift>, IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly RepositoryContext _repositoryContext;
        public GiftService(IGiftRepository giftRepository, RepositoryContext repositoryContext) : base(giftRepository)
        {
            _giftRepository = giftRepository;
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
        public ApiReponse GetById(Guid giftID)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.GiftID == giftID).FirstOrDefault();
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
        
        public ApiReponse GetByProductId(Guid productID)
        {
            try
            {
                var record = (from gift in _repositoryContext.Gifts
                              join giftBy in _repositoryContext.GiftByProducts on gift.GiftID equals giftBy.GiftID
                              where giftBy.ProductID == productID && giftBy.DelFalg == EnumType.DeleteFlag.Using
                              select gift).ToList();
                
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
        public ApiReponse GetByFilter(string? keyword, int? sort, bool? status, int pageSize, int pageNumber)
        {
            try
            {
                if (keyword == null)
                {
                    keyword = string.Empty;
                }
                else
                {
                    keyword = keyword.Trim().ToLower();
                }
                return new ApiReponse()
                {
                    Success = true,
                    Data = _giftRepository.GetByFilter(keyword, sort, status, pageSize, pageNumber)
                };

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }

        public ApiReponse Save(Gift gift, Guid giftID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (giftID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.GiftID == giftID && gift.GiftID == giftID).Any())
                    {
                        if (FindByCondition(g => g.GiftCode.ToLower() == gift.GiftCode.ToLower() && g.GiftID != giftID).Any())
                        {
                            result.Success = false;
                            result.Data = new ErrorResult()
                            {
                                ErrorCode = EnumType.ErrorCode.Duplicate,
                                UserMsg = "Mã quà tặng đã tồn tại!",
                                DevMsg = Resource.DevMsgDuplicate,
                                MoreInfo = Resource.MoreInfo
                            };
                        }
                        else
                        {
                            gift.ModifiedDate = DateTime.Now;
                            Update(gift);
                            result.Success = true;
                            result.Data = gift.GiftID;
                        }
                        
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    if(FindByCondition(g => g.GiftCode.ToLower() == gift.GiftCode.ToLower()).Any())
                    {
                        result.Success = false;
                        result.Data = new ErrorResult()
                        {
                            ErrorCode = EnumType.ErrorCode.Duplicate,
                            UserMsg = "Mã quà tặng đã tồn tại!",
                            DevMsg = Resource.DevMsgDuplicate,
                            MoreInfo = Resource.MoreInfo
                        };
                    }
                    else
                    {
                        gift.GiftID = Guid.NewGuid();
                        Create(gift);
                        result.Success = true;
                        result.Data = gift.GiftID;
                    }
                    
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
                    var result = _giftRepository.UpdateMultiple(listID, status);
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
                    var result = _giftRepository.Delete(listID);
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
