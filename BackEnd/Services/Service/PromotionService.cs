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
    public interface IPromotionService : IServiceBase<Promotion>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid promotionID);
        ApiReponse GetByCode(string promotionCode);
        ApiReponse Save(Promotion promotion, Guid promotionID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class PromotionService : BaseService<Promotion>, IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly RepositoryContext _repositoryContext;
        public PromotionService(IPromotionRepository promotionRepository, RepositoryContext repositoryContext) : base(promotionRepository)
        {
            _promotionRepository = promotionRepository;
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
        public ApiReponse GetById(Guid promotionID)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.PromotionID == promotionID).FirstOrDefault();
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
        public ApiReponse GetByCode(string promotionCode)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && 
                a.Status == EnumType.Status.Using && a.PromotionCode == promotionCode).FirstOrDefault();
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
        public ApiReponse Save(Promotion promotion, Guid promotionID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (promotionID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.PromotionID == promotionID && promotion.PromotionID == promotionID).Any())
                    {
                        promotion.ModifiedDate = DateTime.Now;
                        Update(promotion);
                        result.Success = true;
                        result.Data = promotion.PromotionID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    promotion.PromotionID = Guid.NewGuid();
                    Create(promotion);
                    result.Success = true;
                    result.Data = promotion.PromotionID;
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
                    var result = _promotionRepository.UpdateMultiple(listID, status);
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
                    var result = _promotionRepository.Delete(listID);
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
