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
    public interface IRateService : IServiceBase<Rate>
    {
        ApiReponse GetByProductID(Guid productID);
        ApiReponse Save(Rate rate, Guid rateID);
        ApiReponse Delete(Guid rateID);
    }

    internal class RateService : BaseService<Rate>, IRateService
    {
        private readonly IRateRepository _rateRepository;
        public RateService(IRateRepository rateRepository) : base(rateRepository)
        {
            _rateRepository = rateRepository;
        }
        public ApiReponse GetByProductID(Guid productID)
        {
            try
            {
                var records = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ProductID == productID).ToList();
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

        public ApiReponse Save(Rate rate, Guid rateID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (rateID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.RateID == rateID && rate.RateID == rateID).Any())
                    {
                        rate.ModifiedDate = DateTime.Now;
                        Update(rate);
                        result.Success = true;
                        result.Data = rate.RateID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    rate.RateID = Guid.NewGuid();
                    Create(rate);
                    result.Success = true;
                    result.Data = rate.RateID;
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

        public ApiReponse Delete(Guid rateID)
        {
            try
            {
                var result = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using &&  r.RateID == rateID).FirstOrDefault();
                if (result != null)
                {
                    result.DelFalg = EnumType.DeleteFlag.Deleted;
                    result.ModifiedDate = DateTime.Now;
                    Update(result);
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = rateID
                    };
                }
                else
                {
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
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }

    }

}
