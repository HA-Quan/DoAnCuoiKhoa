using Services.Helpers;
using Services.Models.Entities.DTO;
using Services.Repository;
using System.Linq.Expressions;

namespace Services.Service.Base
{
    public interface IServiceBase<T>
    {
        List<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        ApiReponse Create(T entity);
        ApiReponse Update(T entity);
        ApiReponse Delete(T entity);
    }
    public class BaseService<T> : IServiceBase<T> where T : class
    {
        public IRepositoryBase<T> _repositoryBase;
        public BaseService(IRepositoryBase<T> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }
        public List<T> FindAll()
        {
            return _repositoryBase.FindAll().ToList();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _repositoryBase.FindByCondition(expression);
        }
        public ApiReponse Create(T entity)
        {
            try
            {
                _repositoryBase.Create(entity);
                _repositoryBase.Save();
                return new ApiReponse()
                {
                    Success = true,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorAddFail()
                };
            }
            
        }
        public ApiReponse Update(T entity)
        {
            try
            {
                _repositoryBase.Update(entity);
                _repositoryBase.Save();
                return new ApiReponse()
                {
                    Success = true,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorUpdateFail()
                };
            }
        }
        public ApiReponse Delete(T entity)
        {
            try
            {
                _repositoryBase.Delete(entity);
                _repositoryBase.Save();
                return new ApiReponse()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorDeleteFail()
                };
            }
        }
    }
}
