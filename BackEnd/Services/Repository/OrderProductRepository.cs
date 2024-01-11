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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static Services.Models.Enum.EnumType;

namespace Services.Repository
{
    public interface IOrderProductRepository : IRepositoryBase<OrderProduct>
    {
        int DeleteByAccountID(Guid accountID);
        PagingData<ViewOrder> GetByAccountID(Guid accountID, DateTime timeStart, DateTime timeEnd,
                    bool? deliveryMethod, bool? paymentMethod, byte status, int pageSize, int pageNumber);
        PagingData<OrderProduct> GetByFilter(DateTime timeStart, DateTime timeEnd, string? keyword, int? sort,
            bool? deliveryMethod, bool? paymentMethod, bool? statusPayment, byte status, int pageSize, int pageNumber);
        //ApiReponse UpdateMultiple(List<Guid> listID, byte status);
    }
    public class OrderProductRepository : RepositoryBase<OrderProduct>, IOrderProductRepository
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderProductRepository(RepositoryContext repositoryContext, IOrderDetailRepository orderDetailRepository) : base(repositoryContext)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        public int DeleteByAccountID(Guid accountID)
        {
            var listOrder = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.CreatedBy == accountID).ToList();
            foreach (var item in listOrder)
            {
                if (item.Status != (byte)EnumType.StatusOrder.Delivered && item.Status != (byte)EnumType.StatusOrder.Cancelled)
                {
                    return 0;
                }
                else
                {
                    item.DelFalg = EnumType.DeleteFlag.Deleted;
                    item.ModifiedDate = DateTime.Now;
                    _orderDetailRepository.DeleteByOrderID(item.OrderID);
                }

            }
            UpdateMultiple(listOrder);
            Save();
            return 1;
        }
        public PagingData<ViewOrder> GetByAccountID(Guid accountID, DateTime timeStart, DateTime timeEnd,
                    bool? deliveryMethod, bool? paymentMethod, byte status, int pageSize, int pageNumber) 
        {
            if (timeStart != DateTime.MinValue) {
                timeStart = timeStart.AddHours(-12);
            }
            if (timeEnd != DateTime.MaxValue) {
                timeEnd = timeEnd.AddHours(12);
            }
            var respone = new PagingData<ViewOrder>();
            var listOrder = new List<ViewOrder>();
            var listProduct = new List<OrderProduct>();
            var result = new List<OrderProduct>();
            listProduct = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.CreatedBy== accountID 
            && a.CreatedDate >= timeStart && a.CreatedDate <= timeEnd).ToList();
            if (deliveryMethod != null) {
                listProduct = listProduct.Where(o => o.DeliveryMethod == deliveryMethod).ToList();
            }
            if (paymentMethod != null) {
                listProduct = listProduct.Where(o => o.PaymentMethod == paymentMethod).ToList();
            }
            listProduct = listProduct.Where(o => o.Status == status).ToList();
            listProduct = listProduct.OrderByDescending(s => s.CreatedDate).ToList();
          
            var maxResult = listProduct.Count;
            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            result = listProduct.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            foreach (var item in result) {
                listOrder.Add(new ViewOrder {
                    Order = item,
                    ListOrderDetail = ConfigOrderToView(item, accountID),
                });
            }
            respone.Data = listOrder;
            return respone;

        }
        public List<OrderItemModel> ConfigOrderToView(OrderProduct orderProduct, Guid accountID) {

            var result = new List<OrderItemModel>();
            var orderDetails = _repositoryContext.OrderDetails.Where(o => o.OrderID == orderProduct.OrderID
                            && o.DelFalg == EnumType.DeleteFlag.Using).ToList();
            foreach (var item in orderDetails) {
                var product = _repositoryContext.Products.Where(o => o.ProductID == item.ProductID).FirstOrDefault();
                var rate = _repositoryContext.Rates.Where(o => o.ProductID == item.ProductID && o.AccountID == accountID)
                    .FirstOrDefault();
                result.Add(new OrderItemModel {
                    DetailID = item.DetailID,
                    ProductID = product?.DelFalg == EnumType.DeleteFlag.Using ? product.ProductID : Guid.Empty,
                    RateID = rate != null ? rate.RateID : Guid.Empty,
                    ProductName = product != null ? product.ProductName : string.Empty,
                    MainImage = product != null ? product.MainImage : string.Empty,
                    Amount = item.Amount,
                    Condition = product.Condition,
                    Price = item.Price,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                });
            }
            return result;
            
        }
        public PagingData<OrderProduct> GetByFilter(DateTime timeStart, DateTime timeEnd, string? keyword, int? sort,
            bool? deliveryMethod, bool? paymentMethod, bool? statusPayment, byte status, int pageSize, int pageNumber)
        {
            if(timeStart != DateTime.MinValue)
            {
                timeStart = timeStart.AddHours(-12);
            }
            if (timeEnd != DateTime.MaxValue)
            {
                timeEnd = timeEnd.AddHours(12);
            }
            var respone = new PagingData<OrderProduct>();
            var result = new List<OrderProduct>();
            result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.CreatedDate >= timeStart && a.CreatedDate <= timeEnd && a.OrderBy.Contains(keyword)).ToList();
            if(deliveryMethod != null)
            {
                result = result.Where(o => o.DeliveryMethod == deliveryMethod).ToList();
            }
            if (paymentMethod != null)
            {
                result = result.Where(o => o.PaymentMethod == paymentMethod).ToList();
            }
            if (statusPayment != null)
            {
                result = result.Where(o => o.PaymentStatus == statusPayment).ToList();
            }
            if (status != (byte)EnumType.StatusOrder.All)
            {
                result = result.Where(o => o.Status == status).ToList();
            }
            switch (sort)
            {
                case (int)EnumType.Sort.TimeAsc:
                    result = result.OrderBy(s => s.CreatedDate).ToList();
                    break;

                case (int)EnumType.Sort.TimeDesc:
                    result = result.OrderByDescending(s => s.CreatedDate).ToList();
                    break;

                case (int)EnumType.Sort.PriceAsc:
                    result = result.OrderBy(s => s.Total).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.PriceDesc:
                    result = result.OrderByDescending(s => s.Total).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.StatusAsc:
                    result = result.OrderBy(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.StatusDesc:
                    result = result.OrderByDescending(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameAsc:
                    result = result.OrderBy(s => s.FullName).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameDesc:
                    result = result.OrderByDescending(s => s.FullName).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.UsernameAsc:
                    result = result.OrderBy(s => s.OrderBy).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.UsernameDesc:
                    result = result.OrderByDescending(s => s.OrderBy).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;
                default:
                    result = result.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
            }
            var maxResult = result.Count;
            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            respone.Data = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return respone;

        }
        //public ApiReponse UpdateMultiple(List<Guid> listID, byte status)
        //{
        //    using (var transaction = _repositoryContext.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            bool check = true;
        //            foreach (var orderProductID in listID)
        //            {
        //                var orderProduct = FindByCondition(o => o.OrderID == orderProductID).FirstOrDefault();
        //                if (orderProduct == null)
        //                {
        //                    check = false;
        //                    break;
        //                }
        //                else
        //                {
        //                    orderProduct.Status = status;
        //                    Update(orderProduct);
        //                    _repositoryContext.SaveChanges();
        //                }
        //            }
        //            if (check)
        //            {
        //                transaction.Commit();
        //                return new ApiReponse()
        //                {
        //                    Success = true,
        //                    Data = listID
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

    }
}
