using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Models.Enum.EnumType;

namespace Services.Repository
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        int UpdateMultiple(List<Guid> listID, byte status);
        PagingData<ProductView> GetByFilter(bool byManager, Guid categoryID, Guid chipID, Guid memoryID, Guid storageID, bool? cardType,
            Guid displayID, string? trademark, string keyword, int? sort, byte demand, byte priceRange, int pageSize, int pageNumber);

        List<Product> GetTopProduct(int number, byte byType);
        int Delete(List<Guid> listID);
        int DeleteByCategoryID(Guid categoryID);
        int DeleteByChipID(Guid chipID);
        int DeleteByDisplayID(Guid displayID);
        int DeleteByMemoryID(Guid memoryID);
        int DeleteByStorageID(Guid storageID);
        Task<string> AddImage(IFormFile file);
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly IGiftByProductRepository _giftByProductRepository;
        private readonly IRateRepository _rateRepository;
        private readonly IConfiguration _configuration;
        public ProductRepository(RepositoryContext repositoryContext,
            IGiftByProductRepository giftByProductRepository, IRateRepository rateRepository, IConfiguration configuration) : base(repositoryContext)
        {
            _giftByProductRepository = giftByProductRepository;
            _rateRepository = rateRepository;
            _configuration = configuration;
        }
        public CriteriaModel GetByCriteriaPrice(List<Product> products)
        {
            var record = new CriteriaModel();
            record.Title = "Price";
            if (products.Where(product => GetPrice(product.Price, product.Discount) < 10000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID =((byte) EnumType.SortByPrice.Less10M).ToString(),
                    Name = "Dưới 10 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) < 10000000).Count()
                });
            }

            if (products.Where(product => GetPrice(product.Price, product.Discount) >= 10000000 &&
            GetPrice(product.Price, product.Discount) < 15000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID = ((byte)EnumType.SortByPrice.From10To15).ToString(),
                    Name = "10 triệu - 15 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) >= 10000000 &&
            GetPrice(product.Price, product.Discount) < 15000000).Count()
                });
            }

            if (products.Where(product => GetPrice(product.Price, product.Discount) >= 15000000 &&
            GetPrice(product.Price, product.Discount) < 20000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID = ((byte)EnumType.SortByPrice.From15To20).ToString(),
                    Name = "15 triệu - 20 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) >= 15000000 &&
            GetPrice(product.Price, product.Discount) < 20000000).Count()
                });
            }

            if (products.Where(product => GetPrice(product.Price, product.Discount) >= 20000000 &&
            GetPrice(product.Price, product.Discount) < 30000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID = ((byte)EnumType.SortByPrice.From20To30).ToString(),
                    Name = "20 triệu - 30 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) >= 20000000 &&
            GetPrice(product.Price, product.Discount) < 30000000).Count()
                });
            }

            if (products.Where(product => GetPrice(product.Price, product.Discount) >= 10000000 &&
            GetPrice(product.Price, product.Discount) < 15000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID = ((byte)EnumType.SortByPrice.From30To50).ToString(),
                    Name = "30 triệu - 50 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) >= 30000000 &&
            GetPrice(product.Price, product.Discount) < 50000000).Count()
                });
            }

            if (products.Where(product => GetPrice(product.Price, product.Discount) >= 50000000).Count() > 0)
            {
                record.SelectModels.Add(new SelectModel()
                {
                    ID = ((byte)EnumType.SortByPrice.Over50).ToString(),
                    Name = "Trên 50 triệu",
                    Count = products.Where(product => GetPrice(product.Price, product.Discount) >= 50000000).Count()
                });
            }
            return record;
        }
        public List<CriteriaModel> GetCriteria(List<Product> products)
        {
            var result = new List<CriteriaModel>();
            result.Add(GetByCriteriaPrice(products));
            var criteriaTrademark = new CriteriaModel()
            {
                Title = "Thương hiệu",
                SelectModels = (from pro in products
                                join cate in _repositoryContext.Categorys on pro.CategoryID equals cate.CategoryID
                                group cate by cate.Trademark into trade
                                select new SelectModel()
                                {
                                    Name = trade.Key,
                                    Count = trade.Count()
                                }).ToList()
            };
            result.Add(criteriaTrademark);

            var criteriaChip = new CriteriaModel()
            {
                Title = "CPU",
                SelectModels = (from pro in products
                                join chip in _repositoryContext.Chips on pro.ChipID equals chip.ChipID
                                group chip by chip.ChipID into c
                                select new SelectModel()
                                {
                                    ID = c.Key.ToString(),
                                    Name = c.Select(r => r.ChipType).FirstOrDefault(),
                                    Count = c.Count()
                                }).ToList()
            };
            result.Add(criteriaChip);

            var criteriaMemory = new CriteriaModel()
            {
                Title = "RAM",
                SelectModels = (from pro in products
                                join memory in _repositoryContext.Memorys on pro.MemoryID equals memory.MemoryID
                                group memory by memory.MemoryID into m
                                select new SelectModel()
                                {
                                    ID = m.Key.ToString(),
                                    Name = m.Select(r => r.MemoryType).FirstOrDefault(),
                                    Count = m.Count()
                                }).ToList()
            };
            result.Add(criteriaMemory);

            var criteriaStorage = new CriteriaModel()
            {
                Title = "Dung lượng ổ cứng",
                SelectModels = (from pro in products
                                join storage in _repositoryContext.Storages on pro.StorageID equals storage.StorageID
                                group storage by storage.StorageID into s
                                select new SelectModel()
                                {
                                    ID = s.Key.ToString(),
                                    Name = s.Select(r => r.StorageType).FirstOrDefault(),
                                    Count = s.Count()
                                }).ToList()
            };
            result.Add(criteriaStorage);

            var criteriaDisplay = new CriteriaModel()
            {
                Title = "CPU",
                SelectModels = (from pro in products
                                join display in _repositoryContext.Displays on pro.ChipID equals display.DisplayID
                                group display by display.DisplayID into d
                                select new SelectModel()
                                {
                                    ID = d.Key.ToString(),
                                    Name = d.Select(r => r.DisplayType).FirstOrDefault(),
                                    Count = d.Count()
                                }).ToList()
            };
            result.Add(criteriaDisplay);

            var criteriaCard = new CriteriaModel()
            {
                Title = "Card đồ họa",
                SelectModels = (from pro in products
                                group pro by pro.CardType into card
                                select new SelectModel()
                                {
                                    Name = card.Key == true ? "Card rời" : "Card Onboard",
                                    Count = card.Count()
                                }).ToList()
            };
            result.Add(criteriaCard);

            var criteriaDemand = new CriteriaModel()
            {
                Title = "Dòng máy",
            };

            if(products.Where(p => CheckDemand(p.DemandTypeString,(byte) EnumType.DemandType.Office)).Count() > 0)
            {
                criteriaDemand.SelectModels.Add(new SelectModel()
                {
                    ID = EnumType.DemandType.Office.ToString(),
                    Name = "Học tập - văn phòng",
                    Count = products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.Office)).Count()
                });
            }

            if (products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.Gaming)).Count() > 0)
            {
                criteriaDemand.SelectModels.Add(new SelectModel()
                {
                    ID = EnumType.DemandType.Gaming.ToString(),
                    Name = "Laptop Gaming",
                    Count = products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.Gaming)).Count()
                });
            }

            if (products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.Graphics)).Count() > 0)
            {
                criteriaDemand.SelectModels.Add(new SelectModel()
                {
                    ID = EnumType.DemandType.Graphics.ToString(),
                    Name = "Đồ họa - kĩ thuật",
                    Count = products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.Graphics)).Count()
                });
            }

            if (products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.ThinAndLight)).Count() > 0)
            {
                criteriaDemand.SelectModels.Add(new SelectModel()
                {
                    ID = EnumType.DemandType.ThinAndLight.ToString(),
                    Name = "Cao cấp - sang trọng",
                    Count = products.Where(p => CheckDemand(p.DemandTypeString, (byte)EnumType.DemandType.ThinAndLight)).Count()
                });
            }

            result.Add(criteriaDemand);

            return result;

        } 
        public PagingData<ProductView> GetByFilter(bool byManager, Guid categoryID, Guid chipID, Guid memoryID, Guid storageID, bool? cardType,
            Guid displayID, string? trademark, string keyword, int? sort, byte demand, byte priceRange, int pageSize, int pageNumber)
        {
            var respone = new PagingData<ProductView>();
            var result = new List<ProductView>();
            var listProduct = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ProductName.Contains(keyword)).ToList();
            if (!byManager)
            {
                listProduct = listProduct.Where(p => p.Status != (byte)EnumType.StatusProduct.StopSelling).ToList();
            }
            if (categoryID != Guid.Empty)
            {
                listProduct = listProduct.Where(p => CheckCategory(p.CategoryID, categoryID)).ToList();
            }

            if (demand != (byte)EnumType.DemandType.All)
            {
                listProduct = listProduct.Where(p => CheckDemand(p.DemandTypeString, demand)).ToList();
            }

            if (priceRange != (byte)EnumType.SortByPrice.All)
            {
                listProduct = FilterByPriceRange(listProduct, priceRange);
            }

            if(trademark != null)
            {
                listProduct = listProduct.Where(p => _repositoryContext.Categorys.
                            Any(c => (c.CategoryID == p.CategoryID && c.Trademark == trademark))).ToList();
            }

            if (chipID != Guid.Empty)
            {
                listProduct = listProduct.Where(s => s.ChipID == chipID).ToList();
            }
            if (memoryID != Guid.Empty)
            {
                listProduct = listProduct.Where(s => s.MemoryID == memoryID).ToList();
            }
            if (storageID != Guid.Empty)
            {
                listProduct = listProduct.Where(s => s.StorageID == storageID).ToList();
            }
            if (cardType != null)
            {
                listProduct = listProduct.Where(s => s.CardType == cardType).ToList();
            }
            if (displayID != Guid.Empty)
            {
                listProduct = listProduct.Where(s => s.DisplayID == displayID).ToList();
            }
            
            respone.ListCriteriaModel = GetCriteria(listProduct);

            listProduct = FilterSort(listProduct, sort);
            var maxResult = listProduct.Count();

            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            listProduct = listProduct.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            foreach (var product in listProduct)
            {
                var record = new ProductView()
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    CategoryName = product.CategoryName,
                    MainImage = product.MainImage,
                    Discount = product.Discount,
                    Condition = product.Condition,
                    ChipDetail = product.ChipDetail,
                    MemoryDetail = product.MemoryDetail,
                    CardDetail = product.CardDetail,
                    DisplayDetail = product.DisplayDetail,
                    StorageDetail = product.StorageDetail,
                    Price = product.Price,
                    Quantity = product.Inventory,
                    NumberSell = product.Quantity - product.Inventory,
                    Status = product.Status,
                };
                result.Add(record);
            }
            respone.Data = result;
            return respone;
        }
        public bool CheckCategory(Guid categoryProduct, Guid categoryFilter)
        {
            try
            {
                while (_repositoryContext.Categorys.Where(c => c.CategoryID == categoryProduct).Any() && categoryProduct != Guid.Empty)
                {
                    if (categoryFilter == categoryProduct)
                    {
                        return true;
                    }
                    else
                    {
                        categoryProduct = _repositoryContext.Categorys.Where(c => c.CategoryID == categoryProduct).FirstOrDefault().ParentID;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool CheckDemand(string demandProduct, byte demandFilter)
        {
            try
            {
                var listDemand = demandProduct.Split(',').ToList().ConvertAll(e => Convert.ToByte(e));
                return listDemand.Contains(demandFilter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public long GetPrice(long priceOld, byte saleoff)
        {
            return Convert.ToInt32(priceOld * (100 - saleoff) / 1000000) * 10000;
        }
        public List<Product> FilterByPriceRange(List<Product> products, byte priceFilter)
        {
            if (priceFilter == (byte)EnumType.SortByPrice.Less10M)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) < 10000000).ToList();
            }

            if (priceFilter == (byte)EnumType.SortByPrice.From10To15)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) >= 10000000
                && GetPrice(product.Price, product.Discount) < 15000000).ToList();
            }

            if (priceFilter == (byte)EnumType.SortByPrice.From15To20)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) >= 15000000
                && GetPrice(product.Price, product.Discount) < 20000000).ToList();
            }

            if (priceFilter == (byte)EnumType.SortByPrice.From20To30)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) >= 20000000
                && GetPrice(product.Price, product.Discount) < 30000000).ToList();

            }

            if (priceFilter == (byte)EnumType.SortByPrice.From30To50)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) >= 30000000
                && GetPrice(product.Price, product.Discount) < 50000000).ToList();
            }

            if (priceFilter == (byte)EnumType.SortByPrice.Over50)
            {
                return products.Where(product => GetPrice(product.Price, product.Discount) >= 50000000).ToList();
            }

            return products;

        }
        public List<Product> FilterSort(List<Product> products, int? sort)
        {
            switch (sort)
            {
                case (int)EnumType.Sort.TimeAsc:
                    return products.OrderBy(s => s.CreatedDate).ToList();

                case (int)EnumType.Sort.TimeDesc:
                    return products.OrderByDescending(s => s.CreatedDate).ToList();

                case (int)EnumType.Sort.PriceAsc:
                    return products.OrderBy(s => GetPrice(s.Price, s.Discount)).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.PriceDesc:
                    return products.OrderByDescending(s => GetPrice(s.Price, s.Discount)).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.ViewAsc:
                    return products.OrderBy(s => s.NumberView).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.ViewDesc:
                    return products.OrderByDescending(s => s.NumberView).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.RateAsc:
                    return products.OrderBy(s => (from r in _repositoryContext.Rates
                                                  where r.ProductID == s.ProductID
                                                  select Convert.ToInt32(r.NumStar)).Average()).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.RateDesc:
                    return products.OrderByDescending(s => (from r in _repositoryContext.Rates
                                                            where r.ProductID == s.ProductID
                                                            select Convert.ToInt32(r.NumStar)).Average()).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.NameAsc:
                    return products.OrderBy(s => s.ProductName).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.NameDesc:
                    return products.OrderByDescending(s => s.ProductName).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.StatusAsc:
                    return products.OrderBy(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.StatusDesc:
                    return products.OrderByDescending(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.QuantityAsc:
                    return products.OrderBy(s => s.Inventory).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.QuantityDesc:
                    return products.OrderByDescending(s => s.Inventory).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.NumberSellAsc:
                    return products.OrderBy(s => (s.Quantity-s.Inventory)).ThenByDescending(s => s.ModifiedDate).ToList();

                case (int)EnumType.Sort.NumberSellDesc:
                    return products.OrderByDescending(s => (s.Quantity - s.Inventory)).ThenByDescending(s => s.ModifiedDate).ToList();

            }
            return products.OrderByDescending(s => s.ModifiedDate).ToList();
        }
        public List<Product> GetTopProduct(int number, byte byType)
        {
            if (byType == (byte)EnumType.ByType.NumberSell)
            {
                return _repositoryContext.Products.Where(p=> p.DelFalg == EnumType.DeleteFlag.Using).OrderByDescending(p => (p.Quantity-p.Inventory)).ThenByDescending(p => p.ModifiedDate).Take(number).ToList();
            }
            else if (byType == (byte)EnumType.ByType.NumberView)
            {
                return _repositoryContext.Products.Where(p => p.DelFalg == EnumType.DeleteFlag.Using).OrderByDescending(p => p.NumberView).ThenByDescending(p => p.ModifiedDate).Take(number).ToList();
            }
            return null;

        }

        public int UpdateMultiple(List<Guid> listID, byte status)
        {
            bool check = true;
            var records = new List<Product>();
            foreach (var id in listID)
            {
                var record = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ProductID == id).FirstOrDefault();
                if (record == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    record.Status = status;
                    record.ModifiedDate = DateTime.Now;
                    records.Add(record);
                }
            }
            if (check)
            {
                UpdateMultiple(records);
                Save();
                return listID.Count;
            }
            else
            {
                return 0;
            }
        }

        //public ApiReponse Delete(List<Guid> listID)
        //{
        //    using (var transaction = _repositoryContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            bool check = true;
        //            foreach (var productID in listID)
        //            {
        //                var product = FindByCondition(p => p.ProductID == productID).FirstOrDefault();
        //                if (product == null)
        //                {
        //                    check = false;
        //                    break;
        //                }
        //                else
        //                {
        //                    Delete(product);
        //                    _repositoryContext.SaveChanges();
        //                }
        //            }
        //            if (check)
        //            {
        //                transaction.Commit();
        //                return new ApiReponse()
        //                {
        //                    Success = true,
        //                    Data = listID.Count()
        //                };
        //            }
        //            else
        //            {
        //                transaction.Rollback();
        //                return new ApiReponse()
        //                {
        //                    Success = false,
        //                    Data = HandleError.GenerateErrorResultValidate()
        //                };
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            transaction.Rollback();
        //            return new ApiReponse()
        //            {
        //                Success = false,
        //                Data = HandleError.GenerateErrorResultDatabase()
        //            };
        //        }
        //    }
        //}
        public int Delete(List<Guid> listProductID)
        {
            var listProduct = new List<Product>();
            foreach (var productID in listProductID)
            {
                Product? product = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ProductID == productID).FirstOrDefault();
                if (product == null)
                {
                    return 0;
                }
                else
                {
                    //var listOrderDetail = _repositoryContext.OrderDetails.
                    //    Where(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ProductID == productID).ToList();
                    //foreach (var orderDetail in listOrderDetail)
                    //{
                    //    orderDetail.DelFalg = EnumType.DeleteFlag.Deleted;
                    //}
                    //_repositoryContext.OrderDetails.UpdateRange(listOrderDetail);
                    //var listImport = _repositoryContext.ImportProducts.
                    //    Where(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ProductID == productID).ToList();
                    //foreach (var import in listImport)
                    //{
                    //    import.DelFalg = EnumType.DeleteFlag.Deleted;
                    //}
                    //_repositoryContext.ImportProducts.UpdateRange(listImport);
                    //Save();

                    _giftByProductRepository.DeleteByProductID(productID);
                    _rateRepository.DeleteByProductID(productID);

                    product.DelFalg = EnumType.DeleteFlag.Deleted;
                    product.ModifiedDate = DateTime.Now;
                    listProduct.Add(product);
                }
            }
            UpdateMultiple(listProduct);
            Save();
            return 1;
        }

        public int DeleteByCategoryID(Guid categoryID)
        {
            var listProductID = (from p in _repositoryContext.Products
                                 where p.CategoryID == categoryID
                                 select p.ProductID).ToList();
            return Delete(listProductID);
        }
        public int DeleteByChipID(Guid chipID)
        {
            var listProductId = (from product in _repositoryContext.Products
                                 where product.ChipID == chipID
                                 select product.ProductID).ToList();
            return Delete(listProductId);
        }
        public int DeleteByDisplayID(Guid displayID)
        {
            var listProductId = (from product in _repositoryContext.Products
                                 where product.DisplayID == displayID
                                 select product.ProductID).ToList();
            return Delete(listProductId);
        }
        public int DeleteByMemoryID(Guid memoryID)
        {
            var listProductId = (from product in _repositoryContext.Products
                                 where product.MemoryID == memoryID
                                 select product.ProductID).ToList();
            return Delete(listProductId);
        }
        public int DeleteByStorageID(Guid storageID)
        {
            var listProductId = (from product in _repositoryContext.Products
                                 where product.StorageID == storageID
                                 select product.ProductID).ToList();
            return Delete(listProductId);
        }
        //public int DeleteByAccountID(Guid accountID)
        //{
        //    var listProductID = (from p in _repositoryContext.Products
        //                         where p.CreatedBy == accountID
        //                         select p.ProductID).ToList();
        //    var listProduct = new List<Product>();
        //    foreach (var productID in listProductID)
        //    {
        //        Product? product = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.ProductID == productID).FirstOrDefault();
        //        if (product == null)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            _giftByProductRepository.DeleteByProductID(productID);
        //            _rateRepository.DeleteByProductID(productID);
        //            if (_subProductRepository.DeleteByProductID(productID) == 0)
        //            {
        //                return 0;
        //            }
        //            product.DelFalg = EnumType.DeleteFlag.Deleted;
        //            listProduct.Add(product);
        //        }
        //    }
        //    UpdateMultiple(listProduct);
        //    Save();
        //    return 1;
        //}
        public async Task<string> AddImage(IFormFile file)
        {
            CommonService CS = new CommonService(_configuration);
            MinioClient minio = CS.CreateMinio();
            try
            {
                var bucketName = _configuration["MinIO:BucketName"];
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                using (var data = file.OpenReadStream())
                {
                    var objectName = Guid.NewGuid().ToString() + '_' + file.FileName;
                    var contentType = file.ContentType;
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(data)
                        .WithObjectSize(data.Length)
                        .WithContentType(contentType);
                    await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                    return objectName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
