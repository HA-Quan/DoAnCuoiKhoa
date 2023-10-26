﻿using Microsoft.IdentityModel.Tokens;
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
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface ICategoryService : IServiceBase<Category>
    {
        ApiReponse GetAllCategory();
        ApiReponse GetById(Guid categoryID);
        ApiReponse Save(Category category, Guid categoryID);
        ApiReponse UpdateMultiple(List<Guid> listID, bool status);
        ApiReponse Delete(List<Guid> listID);
    }

    internal class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly RepositoryContext _repositoryContext;

        public CategoryService(ICategoryRepository categoryRepository,IProductRepository productRepository, RepositoryContext repositoryContext) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _repositoryContext = repositoryContext;
        }

        public ApiReponse GetAllCategory()
        {
            try
            {
                var categories = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using).ToList();
                return new ApiReponse()
                {
                    Success = true,
                    Data = categories
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
        public ApiReponse GetById(Guid categoryID)
        {
            try
            {
                var category = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.CategoryID == categoryID).FirstOrDefault();
                if(category == null)
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
                    Data = category
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
        
        public ApiReponse Save(Category category, Guid categoryID)
        {
            ApiReponse result = new ApiReponse();
            try
            {
                if (categoryID != Guid.Empty)
                {
                    if (_repositoryBase.FindByCondition(p =>p.DelFalg == EnumType.DeleteFlag.Using &&
                    p.CategoryID == categoryID && category.CategoryID == categoryID).Any())
                    {
                        Update(category);
                        result.Success = true;
                        result.Data = category.CategoryID;
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }

                }
                else
                {
                    category.CategoryID = Guid.NewGuid();
                    Create(category);
                    result.Success = true;
                    result.Data = category.CategoryID;
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
                    var result = _categoryRepository.UpdateMultiple(listID, status);
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
                    foreach (var categoryID in listID)
                    {
                        if(_productRepository.DeleteByCategoryID(categoryID) == 0)
                        {
                            transaction.Rollback();
                            return new ApiReponse()
                            {
                                Success = false,
                                Data = HandleError.GenerateErrorDeleteFail()
                            };
                        }
                    }
                    var result = _categoryRepository.Delete(listID);
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
