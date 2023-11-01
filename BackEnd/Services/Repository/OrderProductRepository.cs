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
    public interface IOrderProductRepository : IRepositoryBase<OrderProduct>
    {
        int DeleteByAccountID(Guid accountID);
        PagingData<OrderProduct> GetByFilter(DateTime? timeStart, DateTime? timeEnd, string? keyword, int? sort,
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
        public PagingData<OrderProduct> GetByFilter(DateTime? timeStart, DateTime? timeEnd, string? keyword, int? sort,
            bool? deliveryMethod, bool? paymentMethod, bool? statusPayment, byte status, int pageSize, int pageNumber)
        {
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
