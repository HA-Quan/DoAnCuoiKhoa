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
                    ordDetail.DetailID = Guid.NewGuid();
                    product.Inventory -= ordDetail.Amount;
                    _repositoryContext.Products.Update(product);
                }
            }
            _repositoryContext.OrderDetails.AddRange(listOrderDetails);
            Save();
            return listOrderDetails.Count;
        }
        public int UpdateMultiple(List<OrderDetail> listOrderDetailInput, Guid orderID)
        {
            var listOrderDetailOnDB = FindByCondition(o => o.OrderID == orderID && o.DelFalg == EnumType.DeleteFlag.Using).ToList();

            var listIdInput = listOrderDetailInput.Select(o => o.OrderID).ToList();
            var listIdOnDB = listOrderDetailOnDB.Select(o => o.OrderID).ToList();
            var listIdNeedDelete = listIdOnDB.Except(listIdInput).ToList();
            var listIdNeedAdd = listIdInput.Except(listIdOnDB).ToList();
            
            var orderDetailNeedDelete = listOrderDetailOnDB.Where(o => listIdNeedDelete.Contains(o.DetailID)).ToList();
            var orderDetailNeedAdd = listOrderDetailInput.Where(o => listIdNeedAdd.Contains(o.DetailID)).ToList();
            var orderDetailNeedUpdate = listOrderDetailInput.Where(o => !listIdNeedAdd.Contains(o.DetailID)).ToList();

            foreach (var orderDetail in orderDetailNeedDelete)
            {
                orderDetail.DelFalg = EnumType.DeleteFlag.Deleted;
            }

            foreach (var orderDetail in orderDetailNeedAdd)
            {
                var product = _repositoryContext.Products.FirstOrDefault(g => g.ProductID == orderDetail.ProductID
                && g.DelFalg == EnumType.DeleteFlag.Using);
                if (product == null)
                {
                    return 0;
                }
            }
            UpdateMultiple(orderDetailNeedDelete);
            UpdateMultiple(orderDetailNeedUpdate);
            _repositoryContext.OrderDetails.AddRange(orderDetailNeedAdd);
            Save();
            return orderDetailNeedDelete.Count + orderDetailNeedUpdate.Count + orderDetailNeedAdd.Count;

        }
        public void DeleteByOrderID(Guid orderID)
        {
            var orderDetails = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.OrderID == orderID).ToList();
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.DelFalg = EnumType.DeleteFlag.Deleted;
            }
            UpdateMultiple(orderDetails);
            Save();
        }
    }
}
