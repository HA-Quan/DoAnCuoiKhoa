using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Repository;
using Services.Service.Base;
using Services.Service.Common;
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
        ApiReponse GetById(Guid rateID);
        ApiReponse GetByFilter(byte status, int pageSize, int pageNumber);
        ApiReponse GetCountRateNotReply();
        ApiReponse ReplyComment(ReplyModel replyModel);
        Task<ApiReponse> Save(RateModel rateModel, Guid rateID);
        ApiReponse Delete(Guid rateID);
    }

    internal class RateService : BaseService<Rate>, IRateService
    {
        private readonly IRateRepository _rateRepository;
        private readonly RepositoryContext _repositoryContext;
        private readonly IConfiguration _configuration;
        public RateService(IRateRepository rateRepository, RepositoryContext repositoryContext,
            IConfiguration configuration) : base(rateRepository)
        {
            _rateRepository = rateRepository;
            _repositoryContext = repositoryContext;
            _configuration = configuration;
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
        public ApiReponse GetById(Guid rateID) {
            try {
                var rate = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.RateID.Equals(rateID)).FirstOrDefault();
                if (rate != null) {
                    return new ApiReponse() {
                        Success = true,
                        Data = rate
                    };
                } else {
                    return new ApiReponse() {
                        Success = false,
                        Data = HandleError.GenerateErrorResultNotFoundByID()
                    };
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return new ApiReponse() {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }

        public ApiReponse GetByFilter(byte status, int pageSize, int pageNumber)
        {
            try
            {
                return new ApiReponse()
                {
                    Success = true,
                    Data = _rateRepository.GetByFilter(status, pageSize, pageNumber)
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

        public ApiReponse GetCountRateNotReply() {
            try {
                return new ApiReponse() {
                    Success = true,
                    Data = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && string.IsNullOrEmpty(r.CommentReply)).Count()
                };
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return new ApiReponse() {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }
        public ApiReponse ReplyComment(ReplyModel replyModel) {
            try {
                var result = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using 
                    && r.RateID == replyModel.RateID).FirstOrDefault();
                if (result != null) {
                    result.CommentReply = replyModel.ReplyComment;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedBy = replyModel.ModifiedBy;
                    Update(result);
                    return new ApiReponse() {
                        Success = true,
                        Data = result.RateID
                    };
                } else {
                    return new ApiReponse() {
                        Success = false,
                        Data = HandleError.GenerateErrorDeleteFail()
                    };
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return new ApiReponse() {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
        }

        public async Task<ApiReponse> Save(RateModel rateModel, Guid rateID) {
            ApiReponse result = new ApiReponse();
            using (var transaction = _repositoryContext.Database.BeginTransaction()) {
                bool check = true;
                var rate = rateModel.Rate;
                var listImage = rateModel.ListImages;
                try {
                    if (rateID != Guid.Empty) {
                        if (_repositoryContext.Rates.Any(p => p.DelFalg == EnumType.DeleteFlag.Using && p.RateID == rateID)
                            && rate.RateID == rateID) {

                            if (listImage != null) {
                                var list = new List<string>();
                                string nameImage = null;
                                for (var i = 0; i < listImage.Files.Count; i++) {
                                    nameImage = await ImageService.AddImage(listImage.Files[i], _configuration);
                                    if (nameImage != null) {
                                        list.Add(nameImage);
                                    } else {
                                        check = false;
                                        break;
                                    }
                                }
                                if (check) {
                                    rate.ListImg = string.Join(",", list);
                                }

                            }
                            rate.ModifiedDate = DateTime.Now;
                            _repositoryBase.Update(rate);
                            _rateRepository.Save();
                            if (check) {
                                result.Success = true;
                                result.Data = rate.RateID;
                                transaction.Commit();
                            } else {
                                result.Success = false;
                                result.Data = Guid.Empty;
                                transaction.Rollback();
                            }
                        }
                    } else {
                        if (listImage != null) {
                            var list = new List<string>();
                            string nameImage = null;
                            for (var i = 0; i < listImage.Files.Count; i++) {
                                nameImage = await ImageService.AddImage(listImage.Files[i], _configuration);
                                if (nameImage != null) {
                                    list.Add(nameImage);
                                } else {
                                    check = false;
                                    break;
                                }
                            }
                            if (check) {
                                rate.ListImg = string.Join(",", list);
                            }

                        }

                        _repositoryBase.Create(rate);
                        _rateRepository.Save();

                        if (check) {
                            result.Success = true;
                            result.Data = rate.RateID;
                            transaction.Commit();
                        } else {
                            result.Success = false;
                            result.Data = Guid.Empty;
                            transaction.Rollback();
                        }

                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    result.Success = false;
                    result.Data = HandleError.GenerateErrorResultException();
                }
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
