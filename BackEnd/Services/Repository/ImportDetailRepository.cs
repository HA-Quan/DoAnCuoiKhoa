using Minio.DataModel;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IImportDetailRepository : IRepositoryBase<ImportDetail>
    {
        int CreateMultiple(List<ImportDetail> listImportDetails);
        int UpdateMultiple(List<ImportDetail> listImportDetails, Guid importID);
        void DeleteByImportID(Guid importID);
    }
    public class ImportDetailRepository : RepositoryBase<ImportDetail>, IImportDetailRepository
    {
        public ImportDetailRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public int CreateMultiple(List<ImportDetail> listImportDetails)
        {
            foreach (var item in listImportDetails)
            {
                var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                g.ProductID == item.ProductID).FirstOrDefault();
                if (product == null)
                {
                    return 0;
                }
                else
                {
                    product.Inventory += item.Quantity;
                    product.Quantity += item.Quantity;
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedBy = item.CreatedBy;
                    _repositoryContext.Products.Update(product);
                    _repositoryContext.ImportDetails.Add(item);
                    //var record = new Warehouse()
                    //{
                    //    ImportDetailID = item.ImportDetailID,
                    //    ProductID = item.ProductID,
                    //    PriceImport = item.PriceImport,
                    //    CreatedBy = item.CreatedBy,
                    //};
                    var list = new List<Warehouse>();
                    for(int i=0; i<item.Quantity; i++)
                    {
                        list.Add(new Warehouse()
                        {
                            ImportDetailID = item.ImportDetailID,
                            ProductID = item.ProductID,
                            PriceImport = item.PriceImport,
                            CreatedBy = item.CreatedBy,
                        });
                    }
                    _repositoryContext.Warehouses.AddRange(list);
                }
            }
            //_repositoryContext.ImportDetails.AddRange(listImportDetails);
            Save();
            return listImportDetails.Count;
        }
        public int UpdateMultiple(List<ImportDetail> listImportDetailInput, Guid importID)
        {
            var listImportDetailOnDB = FindByCondition(o => o.ImportID == importID && o.DelFalg == EnumType.DeleteFlag.Using).ToList();

            var listIdInput = listImportDetailInput.Select(o => o.ImportDetailID).ToList();
            var listIdOnDB = listImportDetailOnDB.Select(o => o.ImportDetailID).ToList();
            var listIdNeedDelete = listIdOnDB.Except(listIdInput).ToList();
            var listIdNeedAdd = listIdInput.Except(listIdOnDB).ToList();

            var importDetailNeedDelete = listImportDetailOnDB.Where(o => listIdNeedDelete.Contains(o.ImportDetailID)).ToList();
            var importDetailNeedAdd = listImportDetailInput.Where(o => listIdNeedAdd.Contains(o.ImportDetailID)).ToList();
            var importDetailNeedUpdate = listImportDetailInput.Where(o => !listIdNeedAdd.Contains(o.ImportDetailID)).ToList();

            foreach (var importDetail in importDetailNeedDelete)
            {
                if (_repositoryContext.Warehouses.Where(p => p.ImportDetailID == importDetail.ImportDetailID && p.IsSold == true).Any())
                {
                    return 0;
                }
                else
                {
                    importDetail.DelFalg = EnumType.DeleteFlag.Deleted;
                    importDetail.ModifiedDate = DateTime.Now;
                    var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                        g.ProductID == importDetail.ProductID).FirstOrDefault();
                    if (product == null)
                    {
                        return 0;
                    }
                    else
                    {
                        product.Inventory -= importDetail.Quantity;
                        product.Quantity -= importDetail.Quantity;
                        if(product.Inventory == 0)
                        {
                            product.Status = (int) EnumType.StatusProduct.PrepairCome;
                        }
                        product.ModifiedDate = DateTime.Now;
                        _repositoryContext.Products.Update(product);
                        var listProductInWarehouse = _repositoryContext.Warehouses.Where(p => p.ImportDetailID == importDetail.ImportDetailID
                            && p.DelFalg == EnumType.DeleteFlag.Using).AsQueryable().ToList();
                        //foreach (var productInWarehouse in listProductInWarehouse)
                        //{
                        //    productInWarehouse.DelFalg = EnumType.DeleteFlag.Deleted;
                        //    productInWarehouse.ModifiedDate = DateTime.Now;
                        //}
                        //_repositoryContext.Warehouses.UpdateRange(listProductInWarehouse);
                        _repositoryContext.Warehouses.RemoveRange(listProductInWarehouse);
                    }
                }
            }

            foreach (var importDetail in importDetailNeedUpdate)
            {
                if (_repositoryContext.Warehouses.Where(p => p.ImportDetailID == importDetail.ImportDetailID 
                   && p.DelFalg == EnumType.DeleteFlag.Using  && p.IsSold == true).Any())
                {
                    return 0;
                }
                else
                {
                    importDetail.ModifiedDate = DateTime.Now;
                    var importDetilOld = FindByCondition(i => i.ImportDetailID == importDetail.ImportDetailID 
                                                           && i.DelFalg == EnumType.DeleteFlag.Using).FirstOrDefault();
                    var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                        g.ProductID == importDetail.ProductID).FirstOrDefault();
                    if (product == null || importDetilOld == null)
                    {
                        return 0;
                    }
                    else
                    {
                        var numberChange = importDetail.Quantity - importDetilOld.Quantity;
                        product.Inventory = product.Inventory + numberChange;
                        if (product.Inventory == 0)
                        {
                            product.Status = (int)EnumType.StatusProduct.PrepairCome;
                        }
                        product.Quantity = product.Quantity + numberChange;
                        product.ModifiedDate = DateTime.Now;
                        _repositoryContext.Products.Update(product);

                        if(numberChange < 0)
                        {
                            var listProductInWarehouse = _repositoryContext.Warehouses.Where(p => p.ImportDetailID == importDetail.ImportDetailID
                            && p.DelFalg == EnumType.DeleteFlag.Using).AsQueryable().OrderByDescending(p => p.CreatedDate).Take(0-numberChange).ToList();
                            _repositoryContext.Warehouses.RemoveRange(listProductInWarehouse);

                        }
                        else if(numberChange > 0) 
                        {
                            var listCanAdd = new List<Warehouse>();
                            for (int i = 0; i < numberChange; i++)
                            {
                                listCanAdd.Add(new Warehouse()
                                {
                                    ImportDetailID = importDetail.ImportDetailID,
                                    ProductID = importDetail.ProductID,
                                    PriceImport = importDetail.PriceImport,
                                    CreatedBy =(Guid) importDetail.ModifiedBy
                                });
                            }
                            _repositoryContext.Warehouses.AddRange(listCanAdd);
                        }
                        
                    }
                }
            }
            UpdateMultiple(importDetailNeedDelete);
            UpdateMultiple(importDetailNeedUpdate);
            Save();
            if(importDetailNeedAdd.Count > 0)
            {
                var result = CreateMultiple(importDetailNeedAdd);
                if(result == importDetailNeedAdd.Count)
                {
                    return importDetailNeedDelete.Count + importDetailNeedUpdate.Count + result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return importDetailNeedDelete.Count + importDetailNeedUpdate.Count;
            }
        }
        public void DeleteByImportID(Guid importID)
        {
            var importDetails = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.ImportID == importID).ToList();
            foreach (var importDetail in importDetails)
            {
                importDetail.DelFalg = EnumType.DeleteFlag.Deleted;
                importDetail.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(importDetails);
            Save();
        }
    }
}
