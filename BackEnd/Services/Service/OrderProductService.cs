using Microsoft.EntityFrameworkCore;
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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static Services.Models.Enum.EnumType;

namespace Services.Service
{
    public interface IOrderProductService : IServiceBase<OrderProduct>
    {
        ApiReponse GetAll();
        ApiReponse GetByAccountID(Guid accountID, DateTime? timeStart, DateTime? timeEnd,
                    bool? deliveryMethod, bool? paymentMethod, byte status, int pageSize, int pageNumber);
        ApiReponse GetById(Guid orderProductID);
        ApiReponse GetOrderDetailByOrderID(Guid orderProductID);
        ApiReponse GetByFilter(DateTime? timeStart, DateTime? timeEnd, string? keyword, int? sort, 
            bool? deliveryMethod, bool? paymentMethod, bool? statusPayment, byte status, int pageSize, int pageNumber);
        ApiReponse Save(OrderModel orderModel);
        ApiReponse Update(Guid orderID, OrderModel orderModel);
        ApiReponse CancelOrder(Guid orderID);
        ApiReponse UpdateStatus(Guid orderID, byte status);
        ApiReponse Delete(List<Guid> listID);
    }

    public class OrderProductService : BaseService<OrderProduct>, IOrderProductService
    {
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly RepositoryContext _repositoryContext;
        public OrderProductService(IOrderProductRepository orderProductRepository, IOrderDetailRepository orderDetailRepository
            , RepositoryContext repositoryContext) : base(orderProductRepository)
        {
            _orderProductRepository = orderProductRepository;
            _orderDetailRepository = orderDetailRepository;
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
        public ApiReponse GetById(Guid orderProductID)
        {
            try
            {
                var record = new OrderView();
                var order = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.OrderID == orderProductID).FirstOrDefault();
                if (order != null)
                {
                    record.Order = order;
                    var orderDetails = _repositoryContext.OrderDetails.Where(o => o.OrderID == orderProductID
                            && o.DelFalg == EnumType.DeleteFlag.Using).ToList();
                    var listOrderDetail = new List<OrderDetailModel>();
                    foreach (var orderDetail in orderDetails)
                    {
                        var product = _repositoryContext.Products.Where(o => o.ProductID == orderDetail.ProductID).FirstOrDefault();
                        var orderDetailModel = new OrderDetailModel()
                        {
                            DetailID = orderDetail.DetailID,
                            OrderID = orderDetail.OrderID,
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            MainImage = product.MainImage,
                            Amount = orderDetail.Amount,
                            Condition = product.Condition,
                            Price = orderDetail.Price,
                            ListGift = (from gift in _repositoryContext.Gifts
                                        join giftBy in _repositoryContext.GiftByProducts on gift.GiftID equals giftBy.GiftID
                                        where giftBy.ProductID == product.ProductID && giftBy.DelFalg == EnumType.DeleteFlag.Using
                                        select gift).ToList(),
                            CreatedBy = orderDetail.CreatedBy,
                            CreatedDate = orderDetail.CreatedDate,
                            ModifiedBy = orderDetail.ModifiedBy,
                            ModifiedDate = orderDetail.ModifiedDate,
                        };
                        listOrderDetail.Add(orderDetailModel);
                    }
                    record.ListOrderDetail = listOrderDetail;
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = record
                    };

                }
                return new ApiReponse()
                {
                    Success = false,
                    Data = HandleError.GenerateErrorResultNotFoundByID()
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
        public ApiReponse GetByAccountID(Guid accountID, DateTime? timeStart, DateTime? timeEnd,
                    bool? deliveryMethod, bool? paymentMethod, byte status, int pageSize, int pageNumber) 
        {
            try {
                if (timeStart == null) {
                    timeStart = DateTime.MinValue;
                }
                if (timeEnd == null) {
                    timeEnd = DateTime.MaxValue;
                }

                return new ApiReponse() {
                    Success = true,
                    Data = _orderProductRepository.GetByAccountID(accountID, (DateTime)timeStart, (DateTime)timeEnd,
                    deliveryMethod, paymentMethod, status, pageSize, pageNumber)
                };
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return new ApiReponse() {
                    Success = false,
                    Data = HandleError.GenerateErrorResultDatabase()
                };

            }
        }
        public ApiReponse GetByAccountID(Guid accountID, bool isDelivered)
        {
            try
            {
                var records = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.CreatedBy == accountID).ToList();
                if (isDelivered)
                {
                    records = records.Where(r => r.Status == (byte)EnumType.StatusOrder.Delivered).ToList();
                }
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
        public ApiReponse GetOrderDetailByOrderID(Guid orderProductID)
        {
            try
            {
                //var result = _orderDetailRepository.FindByCondition(o =>o.DelFalg == EnumType.DeleteFlag.Using &&
                //o.OrderID == orderProductID).ToList();
                var result = _orderDetailRepository.FindByCondition(o => o.OrderID == orderProductID).ToList();
                return new ApiReponse()
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ApiReponse()
                {
                    Success = false,
                    Data= HandleError.GenerateErrorResultException()
                };
            }
        }
        public ApiReponse GetByFilter(DateTime? timeStart, DateTime? timeEnd, string? keyword, int? sort,
            bool? deliveryMethod, bool? paymentMethod, bool? statusPayment, byte status, int pageSize, int pageNumber)
        {
            try
            {
                if(timeStart == null)
                {
                    timeStart = DateTime.MinValue;
                }
                if (timeEnd == null)
                {
                    timeEnd = DateTime.MaxValue;
                }

                keyword = keyword == null ? string.Empty : keyword.Trim().ToLower();

                return new ApiReponse()
                {
                    Success = true,
                    Data = _orderProductRepository.GetByFilter((DateTime)timeStart, (DateTime)timeEnd, keyword, sort,
                    deliveryMethod, paymentMethod, statusPayment, status, pageSize, pageNumber)
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

        public ApiReponse Save(OrderModel orderModel)
        {
            ApiReponse result = new ApiReponse();
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var check = true;
                    OrderProduct op = orderModel.Order;
                    if(op.PromotionCode.Length > 0)
                    {
                        var promotion = _repositoryContext.Promotions.FirstOrDefault(a => a.DelFalg == EnumType.DeleteFlag.Using &&
                            a.Status == EnumType.Status.Using && a.PromotionCode == op.PromotionCode);
                        if (promotion == null)
                        {
                            check = false;
                        }
                        else
                        {
                            if (promotion.NumUsed < promotion.Quantity && promotion.DayStart < DateTime.Now && promotion.DayExpired > DateTime.Now)
                            {
                                promotion.NumUsed += 1;
                                promotion.ModifiedDate = DateTime.Now;
                                _repositoryContext.Promotions.Update(promotion);
                                _repositoryContext.SaveChanges();
                            }
                            else
                            {
                                check = false;
                            }
                        }
                    }
                    
                    _repositoryBase.Create(op);
                    _orderProductRepository.Save();
                    foreach (var ordDetail in orderModel.ListOrderDetail)
                    {
                        ordDetail.OrderID = op.OrderID;
                        ordDetail.CreatedBy = op.CreatedBy;
                        ordDetail.CreatedDate = op.CreatedDate;
                    }
                    if (_orderDetailRepository.CreateMultiple(orderModel.ListOrderDetail.ToList()) == 0)
                    {
                        check = false;
                    }
                    if (!check)
                    {
                        transaction.Rollback();
                        result.Success = false;
                        result.Data = Guid.Empty;
                    }
                    else
                    {
                        result.Success = true;
                        result.Data = op.OrderID;
                        transaction.Commit();
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
        public ApiReponse Update(Guid orderID, OrderModel orderModel)
        {
            ApiReponse result = new ApiReponse();
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                var check = true;
                try
                {
                    if (FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using &&
                        p.OrderID == orderID).Any() && orderModel.Order.OrderID == orderID)
                    {
                        OrderProduct op = orderModel.Order;
                        if (op.PromotionCode.Length > 0)
                        {
                            var promotion = _repositoryContext.Promotions.FirstOrDefault(a => a.DelFalg == EnumType.DeleteFlag.Using &&
                                a.Status == EnumType.Status.Using && a.PromotionCode == op.PromotionCode);
                            if (promotion == null)
                            {
                                check = false;
                            }
                            else
                            {
                                if (promotion.NumUsed < promotion.Quantity && promotion.DayStart < DateTime.Now && promotion.DayExpired > DateTime.Now)
                                {
                                    promotion.NumUsed += 1;
                                    promotion.ModifiedDate = DateTime.Now;
                                    _repositoryContext.Promotions.Update(promotion);
                                    _repositoryContext.SaveChanges();
                                }
                                else
                                {
                                    check = false;
                                }
                            }
                        }
                        op.ModifiedDate = DateTime.Now;
                        _repositoryBase.Update(op);
                        _orderProductRepository.Save();
                        List<OrderDetail> listOrderDetail = new List<OrderDetail>();
                        foreach (var ordDetail in orderModel.ListOrderDetail)
                        {
                            var item = _repositoryContext.OrderDetails.AsNoTracking().FirstOrDefault(g => g.DelFalg == EnumType.DeleteFlag.Using &&
                            g.DetailID == ordDetail.DetailID);
                            if (item == null)
                            {
                                ordDetail.DetailID = Guid.NewGuid();
                                ordDetail.CreatedBy =(Guid) op.ModifiedBy;
                                ordDetail.CreatedDate = DateTime.Now;
                                ordDetail.OrderID = op.OrderID;
                            }
                            else if(item != ordDetail)
                            {
                                ordDetail.ModifiedBy = op.ModifiedBy;
                                ordDetail.ModifiedDate = DateTime.Now;
                            }
                            listOrderDetail.Add(ordDetail);
                        }
                        if (_orderDetailRepository.UpdateMultiple(listOrderDetail, orderID) == 0)
                        {
                            check = false;
                        }
                        if (check)
                        {
                            result.Success = true;
                            result.Data = op.OrderID;
                            transaction.Commit();
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        result.Success = false;
                        result.Data = Guid.Empty;
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
        public ApiReponse UpdateStatus(Guid orderID, byte status)
        {
            using (var transaction = _repositoryContext.Database.BeginTransaction())
            {
                try
                {
                    var order = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.OrderID == orderID).FirstOrDefault();
                    if (order != null)
                    {
                        //if ((order.Status != (byte)EnumType.StatusOrder.NotApproved && status == (byte)EnumType.StatusOrder.Cancelled)
                        //    || (order.Status == (byte)EnumType.StatusOrder.Cancelled))
                        //{
                        //    return new ApiReponse()
                        //    {
                        //        Success = false,
                        //        Data = Guid.Empty,
                        //    };
                        //}
                        order.Status = status;
                        order.ModifiedDate = DateTime.Now;
                        Update(order);
                        if (status == (byte)EnumType.StatusOrder.Cancelled)
                        {
                            if (_orderDetailRepository.CancelByOrderID(orderID))
                            {
                                transaction.Commit();
                                return new ApiReponse()
                                {
                                    Success = true,
                                    Data = orderID
                                };
                            }
                            else
                            {
                                transaction.Rollback();
                                return new ApiReponse()
                                {
                                    Success = false,
                                    Data = Guid.Empty
                                };
                            }
                        }
                        else
                        {
                            transaction.Commit();
                            return new ApiReponse()
                            {
                                Success = true,
                                Data = orderID
                            };
                        }
                    }
                    else
                    {
                        return new ApiReponse()
                        {
                            Success = false,
                            Data = Guid.Empty
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
        public ApiReponse CancelOrder(Guid orderID) {
            using (var transaction = _repositoryContext.Database.BeginTransaction()) {
                try {
                    var order = FindByCondition(o => o.DelFalg == EnumType.DeleteFlag.Using && o.OrderID == orderID).FirstOrDefault();
                    if (order != null) {
                        if(order.Status == (byte) EnumType.StatusOrder.NotApproved) {
                            order.Status = (byte)EnumType.StatusOrder.Cancelled;
                            order.ModifiedDate = DateTime.Now;
                            Update(order);
                            if (_orderDetailRepository.CancelByOrderID(orderID)) {
                                transaction.Commit();
                                return new ApiReponse() {
                                    Success = true,
                                    Data = orderID
                                };
                            } else {
                                transaction.Rollback();
                                return new ApiReponse() {
                                    Success = false,
                                    Data = Guid.Empty
                                };
                            }
                        } else {
                            return new ApiReponse() {
                                Success = false,
                                Data = "Đơn hàng đã được duyệt nên không thể hủy, vui lòng liên hệ với shop để biết thêm thông tin chi tiết"
                            };
                        }

                    } else {
                        return new ApiReponse() {
                            Success = false,
                            Data = Guid.Empty
                        };
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    return new ApiReponse() {
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
                    var listOrder = new List<OrderProduct>();
                    bool check = true;
                    foreach (var orderID in listID)
                    {
                        var order = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.OrderID == orderID).FirstOrDefault();
                        if (order == null)
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            _orderDetailRepository.DeleteByOrderID(orderID);
                            order.DelFalg = EnumType.DeleteFlag.Deleted;
                            order.ModifiedDate = DateTime.Now;
                            listOrder.Add(order);
                        }
                    }
                    if (check)
                    {
                        _orderProductRepository.UpdateMultiple(listOrder);
                        _orderProductRepository.Save();
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
    }

}
