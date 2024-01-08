using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository
{
    public interface IOrderDetailRepository : IRepositoryBase<OrderDetail>
    {
        int CreateMultiple(List<OrderDetail> listOrderDetails);
        int UpdateMultiple(List<OrderDetail> listOrderDetails, Guid orderID);

        void DeleteByOrderID(Guid orderID);
        bool CancelByOrderID(Guid orderID);

    }
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public int CreateMultiple(List<OrderDetail> listOrderDetails)
        {
            foreach (var ordDetail in listOrderDetails)
            {
                var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                g.ProductID == ordDetail.ProductID && g.Inventory >= ordDetail.Amount).FirstOrDefault();
                if (product == null)
                {
                    return 0;
                }
                else
                {
                    var listExportProduct = new List<ExportProduct>();
                    var listProductInWarehouse = _repositoryContext.Warehouses.Where(g => g.ProductID == ordDetail.ProductID && !g.IsSold
                     && g.DelFalg == EnumType.DeleteFlag.Using).OrderBy(g => g.CreatedDate).Take(ordDetail.Amount).ToList();
                    _repositoryContext.OrderDetails.Add(ordDetail);
                    Save();
                    foreach (var item in listProductInWarehouse)
                    {
                        listExportProduct.Add(new ExportProduct()
                        {
                            DetailID = ordDetail.DetailID,
                            WarehouseID = item.WarehouseID,
                            CreatedBy = ordDetail.CreatedBy
                        });
                        item.IsSold = true;
                    }
                    _repositoryContext.ExportProducts.AddRange(listExportProduct);
                    _repositoryContext.Warehouses.UpdateRange(listProductInWarehouse);
                    product.Inventory -= ordDetail.Amount;
                    if (product.Inventory == 0)
                    {
                        product.Status = (int)EnumType.StatusProduct.PrepairCome;
                    }
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedBy = ordDetail.CreatedBy;
                    _repositoryContext.Products.Update(product);
                }
            }
            //_repositoryContext.OrderDetails.AddRange(listOrderDetails);
            Save();
            return listOrderDetails.Count;
        }
        public int UpdateMultiple(List<OrderDetail> listOrderDetailInput, Guid orderID)
        {
            var listOrderDetailOnDB = FindByCondition(o => o.OrderID == orderID && o.DelFalg == EnumType.DeleteFlag.Using).ToList();

            var listIdInput = listOrderDetailInput.Select(o => o.DetailID).ToList();
            var listIdOnDB = listOrderDetailOnDB.Select(o => o.DetailID).ToList();
            var listIdNeedDelete = listIdOnDB.Except(listIdInput).ToList();
            var listIdNeedAdd = listIdInput.Except(listIdOnDB).ToList();
            
            var orderDetailNeedDelete = listOrderDetailOnDB.Where(o => listIdNeedDelete.Contains(o.DetailID)).ToList();
            var orderDetailNeedAdd = listOrderDetailInput.Where(o => listIdNeedAdd.Contains(o.DetailID)).ToList();
            var orderDetailNeedUpdate = listOrderDetailInput.Where(o => !listIdNeedAdd.Contains(o.DetailID)).ToList();

            foreach (var orderDetail in orderDetailNeedDelete)
            {
                orderDetail.DelFalg = EnumType.DeleteFlag.Deleted;
                orderDetail.ModifiedDate = DateTime.Now;

                var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                g.ProductID == orderDetail.ProductID).FirstOrDefault();
                if (product == null)
                {
                    return 0;
                }
                else
                {
                    product.Inventory += orderDetail.Amount;
                    product.ModifiedDate = DateTime.Now;
                    _repositoryContext.Products.Update(product);

                    var listExportCanDelete = _repositoryContext.ExportProducts.AsQueryable().Where(e => e.DelFalg == EnumType.DeleteFlag.Using &&
                    e.DetailID == orderDetail.DetailID).ToList();
                    var listProductInWareHouse = new List<Warehouse>();
                    foreach(var item in listExportCanDelete)
                    {
                        item.DelFalg = EnumType.DeleteFlag.Deleted;
                        item.ModifiedDate = DateTime.Now;

                        var record = _repositoryContext.Warehouses.AsQueryable().Where(w => w.DelFalg == EnumType.DeleteFlag.Using &&
                        w.WarehouseID == item.WarehouseID).FirstOrDefault();
                        if(record == null)
                        {
                            return 0;
                        }
                        else
                        {
                            record.IsSold = false;
                            listProductInWareHouse.Add(record);
                        }
                    }
                    _repositoryContext.Warehouses.UpdateRange(listProductInWareHouse);
                    _repositoryContext.ExportProducts.UpdateRange(listExportCanDelete);
                    _repositoryContext.SaveChanges();
                }

            }

            foreach (var item in orderDetailNeedUpdate)
            {
                item.ModifiedDate = DateTime.Now;
                var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
               g.ProductID == item.ProductID).FirstOrDefault();
                var orderOld = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.DetailID == item.DetailID).FirstOrDefault();
                if (product == null || orderOld == null)
                {
                    return 0;
                }
                else
                {
                    var changeNumber = item.Amount - orderOld.Amount;
                    product.Inventory = product.Inventory + changeNumber;
                    if (product.Inventory == 0)
                    {
                        product.Status = (int)EnumType.StatusProduct.PrepairCome;
                    }
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedBy = item.ModifiedBy;
                    _repositoryContext.Products.Update(product);

                    if (changeNumber < 0)
                    {
                        var listExportCanDelete = _repositoryContext.ExportProducts.Where(p => p.DetailID == item.DetailID
                        && p.DelFalg == EnumType.DeleteFlag.Using).AsQueryable().OrderByDescending(p=> p.CreatedDate).Take(changeNumber).ToList();
                        
                        var listProductInWareHouse = new List<Warehouse>();
                        foreach (var itemExport in listExportCanDelete)
                        {
                            itemExport.DelFalg = EnumType.DeleteFlag.Deleted;
                            itemExport.ModifiedDate = DateTime.Now;

                            var record = _repositoryContext.Warehouses.AsQueryable().Where(w => w.DelFalg == EnumType.DeleteFlag.Using &&
                            w.WarehouseID == itemExport.WarehouseID).FirstOrDefault();
                            if (record == null)
                            {
                                return 0;
                            }
                            else
                            {
                                record.IsSold = false;
                                listProductInWareHouse.Add(record);
                            }
                        }
                        _repositoryContext.Warehouses.UpdateRange(listProductInWareHouse);
                        _repositoryContext.ExportProducts.UpdateRange(listExportCanDelete);
                        _repositoryContext.SaveChanges();
                    }
                    else
                    {
                        var listExportCanAdd = new List<ExportProduct>();
                        var listProductInWarehouse = _repositoryContext.Warehouses.Where(g => g.ProductID == item.ProductID
                     && g.DelFalg == EnumType.DeleteFlag.Using).OrderBy(g => g.CreatedDate).Take(changeNumber).ToList();
                        foreach (var productInWarehouse in listProductInWarehouse)
                        {
                            listExportCanAdd.Add(new ExportProduct()
                            {
                                DetailID = item.DetailID,
                                WarehouseID = productInWarehouse.WarehouseID
                            });
                            productInWarehouse.IsSold = true;
                        }
                        _repositoryContext.Warehouses.UpdateRange(listProductInWarehouse);
                        _repositoryContext.ExportProducts.AddRange(listExportCanAdd);
                        _repositoryContext.SaveChanges();
                    }
                }
            }

            UpdateMultiple(orderDetailNeedDelete);
            UpdateMultiple(orderDetailNeedUpdate);
            Save();
            if (orderDetailNeedAdd.Count > 0)
            {
                var result = CreateMultiple(orderDetailNeedAdd);
                if (result == orderDetailNeedAdd.Count)
                {
                    return orderDetailNeedDelete.Count + orderDetailNeedUpdate.Count + result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return orderDetailNeedDelete.Count + orderDetailNeedUpdate.Count;
            }

        }
        public void DeleteByOrderID(Guid orderID)
        {
            var orderDetails = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.OrderID == orderID).ToList();
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.DelFalg = EnumType.DeleteFlag.Deleted;
                orderDetail.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(orderDetails);
            Save();
        }
        public bool CancelByOrderID(Guid orderID)
        {
            var orderDetails = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.OrderID == orderID).ToList();
            foreach (var orderDetail in orderDetails)
            {
                var product = _repositoryContext.Products.Where(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                g.ProductID == orderDetail.ProductID).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                else
                {
                    product.Inventory += orderDetail.Amount;
                    product.ModifiedDate = DateTime.Now;
                    _repositoryContext.Products.Update(product);

                    var listExportCanDelete = _repositoryContext.ExportProducts.AsQueryable().Where(e => e.DelFalg == EnumType.DeleteFlag.Using &&
                    e.DetailID == orderDetail.DetailID).ToList();
                    var listProductInWareHouse = new List<Warehouse>();
                    foreach (var item in listExportCanDelete)
                    {
                        item.DelFalg = EnumType.DeleteFlag.Deleted;
                        item.ModifiedDate = DateTime.Now;

                        var record = _repositoryContext.Warehouses.AsQueryable().Where(w => w.DelFalg == EnumType.DeleteFlag.Using &&
                        w.WarehouseID == item.WarehouseID).FirstOrDefault();
                        if (record == null)
                        {
                            return false;
                        }
                        else
                        {
                            record.IsSold = false;
                            listProductInWareHouse.Add(record);
                        }
                    }
                    _repositoryContext.Warehouses.UpdateRange(listProductInWareHouse);
                    _repositoryContext.ExportProducts.UpdateRange(listExportCanDelete);
                    _repositoryContext.SaveChanges();
                }
            }
            Save();
            return true;
        }
    }
}
