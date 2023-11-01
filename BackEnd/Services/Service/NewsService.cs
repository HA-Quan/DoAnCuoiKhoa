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
    public interface INewsService : IServiceBase<News>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid newsID);
        ApiReponse Save(News news, Guid newsID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class NewsService : BaseService<News>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly RepositoryContext _repositoryContext;
        public NewsService(INewsRepository newsRepository, RepositoryContext repositoryContext) : base(newsRepository)
        {
            _newsRepository = newsRepository;
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
        public ApiReponse GetById(Guid newsID)
        {
            try
            {
                var record = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.NewsID == newsID).FirstOrDefault();
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

        public ApiReponse Save(News news, Guid newsID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (newsID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.NewsID == newsID).Any() && news.NewsID == newsID)
                    {
                        news.ModifiedDate = DateTime.Now;
                        Update(news);
                        result.Success = true;
                        result.Data = news.NewsID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    news.NewsID = Guid.NewGuid();
                    Create(news);
                    result.Success = true;
                    result.Data = news.NewsID;
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
                    var result = _newsRepository.UpdateMultiple(listID, status);
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
                    var result = _newsRepository.Delete(listID);
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
