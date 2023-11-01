using CommunityToolkit.HighPerformance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Repository;
using Services.Service.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface IProductService : IServiceBase<Product>
    {
        ApiReponse GetAll();
        ApiReponse GetById(Guid productID);
        ApiReponse GetTopProduct(int number, byte byType);
        ApiReponse GetByFilter(bool byManager, Guid categoryID, Guid chipID, Guid memoryID, Guid storageID, bool? cardType,
            Guid displayID, string trademark, string? keyword, int? sort, byte demand, byte priceRange, int pageSize, int pageNumber);

        Task<ApiReponse> Save(ProductModel product);
        ApiReponse UpdateProduct(ProductModel product, Guid productID);
        ApiReponse UpdateMultiple(List<Guid> listID, byte status);
        ApiReponse Delete(List<Guid> listID);
    }

    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IGiftByProductRepository _giftByProductRepository;
        private readonly RepositoryContext _repositoryContext;
        private readonly IConfiguration _configuration;
        public ProductService(IProductRepository productRepository , RepositoryContext repositoryContext,
            IGiftByProductRepository giftByProductRepository, IConfiguration configuration) : base(productRepository)
        {
            _productRepository = productRepository;
            _repositoryContext = repositoryContext;
            _giftByProductRepository = giftByProductRepository;
            _configuration = configuration;
        }
        public ApiReponse GetAll()
        {
            try
            {
                List<Product> listProduct = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using).ToList();
                return new ApiReponse()
                {
                    Success = true,
                    Data = listProduct
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
        public ApiReponse GetById(Guid productID)
        {
            try
            {
                ProductModel result = new ProductModel();
                var product = FindByCondition(a =>a.DelFalg == EnumType.DeleteFlag.Using && a.ProductID.Equals(productID)).FirstOrDefault();
                if (product != null)
                {
                    result.Product = product;
                    result.ListGifts = (from gift in _repositoryContext.Gifts
                                        join giftBy in _repositoryContext.GiftByProducts on gift.GiftID equals giftBy.GiftID
                                        where giftBy.ProductID == productID && gift.DelFalg == EnumType.DeleteFlag.Using && giftBy.DelFalg == EnumType.DeleteFlag.Using
                                        select gift).ToList();
                    //result.ListImageView = product.ListImageString.Split(',').ToList();
                    //if(result.ListImageView.Count > 0)
                    //{
                    //    foreach(string image in result.ListImageView)
                    //    {
                    //        if (image == null)
                    //        {
                    //            image = string.Empty;
                    //        }
                    //        image += _configuration["MinIO:PrefixLink"];
                    //    }
                    //}
                    
                    product.NumberView += 1;
                    product.ModifiedDate = DateTime.Now;
                    Update(product);
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                else 
                { 
                    return new ApiReponse() 
                    {  
                        Success = false ,
                        Data = HandleError.GenerateErrorResultNotFoundByID()
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
        public ApiReponse GetTopProduct(int number, byte byType)
        {
            try
            {
                var result = _productRepository.GetTopProduct(number, byType);
                if (result != null)
                {
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                    
                }
                else
                {
                    return new ApiReponse()
                    {
                        Success = false,
                        Data = HandleError.GenerateErrorResultValidate()
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
        public ApiReponse GetByFilter(bool byManager, Guid categoryID, Guid chipID, Guid memoryID, Guid storageID, bool? cardType,
            Guid displayID, string? trademark, string? keyword, int? sort, byte demand, byte priceRange, int pageSize, int pageNumber)
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
                    Data = _productRepository.GetByFilter(byManager, categoryID, chipID, memoryID,
                        storageID, cardType, displayID, trademark, keyword, sort, demand, priceRange, pageSize, pageNumber)
                };
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultException()
                };
            }
            
        }

        public async Task<ApiReponse> Save(ProductModel product)
        {
            ApiReponse result = new ApiReponse();
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                bool check = true;
                try
                {
                    Product p = product.Product;
                    if (await _productRepository.AddImage(product.ListImages[0]) != null)
                    {
                        p.MainImage = await _productRepository.AddImage(product.ListImages[0]);
                    }
                    else
                    {
                        check = false;
                    }
                    var listImage = await AddImage(product.ListImages);
                    if (listImage != null)
                    {
                        p.ListImageString = string.Join(",", listImage);
                    }
                    else if (product.ListImages.Count() > 0)
                    {
                        check = false;
                    }
                    _repositoryBase.Create(p);
                    _productRepository.Save();

                    List<GiftByProduct> listGiftByProducts = new List<GiftByProduct>();
                    foreach (var gift in product.ListGifts)
                    {
                        listGiftByProducts.Add(new GiftByProduct()
                        {
                            ID = Guid.NewGuid(),
                            GiftID = gift.GiftID,
                            ProductID = p.ProductID
                        });
                    }
                    if (listGiftByProducts.Count() > 0 && _giftByProductRepository.CreateMultiple(listGiftByProducts) == 0)
                    {
                        check = false;
                    }
                    
                    if (check)
                    {
                        result.Success = true;
                        result.Data = p.ProductID;
                        transaction.Commit();
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                        transaction.Rollback();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    result.Success = false;
                    result.Data = HandleError.GenerateErrorResultException();
                }
            }
            return result;
        }
        public ApiReponse UpdateProduct(ProductModel product, Guid productID)
        {
            ApiReponse result = new ApiReponse();
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                bool check = true;
                try
                {
                    if (_repositoryBase.FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                        p.ProductID == productID).Any() && product.Product.ProductID == productID)
                    {
                        Product p = product.Product;
                        List<GiftByProduct> listGiftByProducts = new List<GiftByProduct>();
                        foreach (var gift in product.ListGifts)
                        {
                            var giftByProduct = _repositoryContext.GiftByProducts.FirstOrDefault(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                            g.ProductID == productID && g.GiftID == gift.GiftID);
                            if (giftByProduct != null)
                            {
                                listGiftByProducts.Add(giftByProduct);
                            }
                            else
                            {
                                listGiftByProducts.Add(new GiftByProduct()
                                {
                                    ID = Guid.NewGuid(),
                                    GiftID = gift.GiftID,
                                    ProductID = productID
                                });
                            }

                        }
                        p.ModifiedDate = DateTime.Now;
                        _repositoryBase.Update(p);
                        _productRepository.Save();
                        if (_giftByProductRepository.UpdateMultiple(listGiftByProducts, productID) == 0)
                        {
                            check = false;
                        }
                        else
                        {
                            result.Success = true;
                            result.Data = p.ProductID;
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Data = Guid.Empty;
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    result.Success = false;
                    result.Data = HandleError.GenerateErrorResultException();
                }
            }
            return result;
        }
        public ApiReponse UpdateMultiple(List<Guid> listID, byte status)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _productRepository.UpdateMultiple(listID, status);
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
                    var listProduct = new List<Product>();
                    bool check = true;
                    foreach (var productID in listID)
                    {
                        Product? product = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ProductID == productID).FirstOrDefault();
                        if (product == null)
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            _giftByProductRepository.DeleteByProductID(productID);
                            product.DelFalg = EnumType.DeleteFlag.Deleted;
                            product.ModifiedDate = DateTime.Now;
                            listProduct.Add(product);
                        }
                    }
                    if (check)
                    {
                        _productRepository.UpdateMultiple(listProduct);
                        _productRepository.Save();
                        transaction.Commit();
                        return new ApiReponse()
                        {
                            Success = true,
                            Data = listID.Count()
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

        public async Task<List<string>> AddImage(IFormFileCollection listImage)
        {
            var list = new List<string>();
            for(var i = 1; i<listImage.Count(); i++)
            {
                var result = await _productRepository.AddImage(listImage[i]);
                if (result == null)
                {
                    return null;
                }
                else
                {
                    list.Add(result);
                }
            }
            return list;
        }
    }

}
