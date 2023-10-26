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

namespace Services.Service
{
    public interface IOrderDetailService : IServiceBase<OrderDetail>
    {
        //OrderDetail GetById(Guid orderDetailID);
        //ApiReponse Save(OrderDetail orderDetail, Guid orderDetailID);
        //ApiReponse CreateMultiple(List<OrderDetail> listOrderDetails);
        //ApiReponse UpdateMultiple(List<OrderDetail> listOrderDetails, Guid orderProductID);
        //ApiReponse Delete(List<Guid> listID);
    }

    internal class OrderDetailService : BaseService<OrderDetail>, IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository) : base(orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        //    public OrderDetail GetById(Guid orderDetailID)
        //    {
        //        OrderDetail? result = FindByCondition(a => a.DetailID == orderDetailID).FirstOrDefault();
        //        return result;
        //    }

        //    public ApiReponse Save(OrderDetail orderDetail, Guid orderDetailID)
        //    {
        //        ApiReponse result = new ApiReponse();
        //        try
        //        {
        //            if (orderDetailID != Guid.Empty)
        //            {
        //                if (_repositoryBase.FindByCondition(p => p.DetailID == orderDetailID).Any())
        //                {
        //                    _repositoryBase.Update(orderDetail);
        //                    _orderDetailRepository.Save();
        //                    result.Success = true;
        //                    result.Data = orderDetail.DetailID;
        //                }
        //                else
        //                {
        //                    result.Success = false;
        //                    result.Data = Guid.Empty;
        //                }

        //            }
        //            else
        //            {
        //                orderDetail.DetailID = Guid.NewGuid();
        //                _repositoryBase.Create(orderDetail);
        //                _orderDetailRepository.Save();
        //                result.Success = true;
        //                result.Data = orderDetail.DetailID;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            result.Success = false;
        //            result.Data = HandleError.GenerateErrorResultException();
        //        }
        //        return result;
        //    }
        //    public ApiReponse Delete(List<Guid> listID)
        //    {
        //        return _orderDetailRepository.Delete(listID);
        //    }
        //    public ApiReponse CreateMultiple(List<OrderDetail> listOrderDetails)
        //    {
        //        return _orderDetailRepository.CreateMultiple(listOrderDetails);
        //    }
        //    public ApiReponse UpdateMultiple(List<OrderDetail> listOrderDetails, Guid orderProductID)
        //    {
        //        return  _orderDetailRepository.UpdateMultiple(listOrderDetails, orderProductID);
        //    }
    }

}
