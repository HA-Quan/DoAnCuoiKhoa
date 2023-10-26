using Services.Models.Entities.DTO;
using Services.Models.Entities;
using Services.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Repository;
using Services.Models.Enum;
using Services.Helpers;

namespace Services.Service
{
    public interface IStatisticService
    {
        ApiReponse GetTop(DateTime? timeStart, DateTime? timeEnd, int number, byte typeGet);
    }
    public class StatisticService : IStatisticService
    {
        private readonly RepositoryContext _repositoryContext;

        public StatisticService(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public ApiReponse GetTop(DateTime? timeStart, DateTime? timeEnd, int number, byte typeGet)
        {
            try
            {
                if (timeStart == null)
                {
                    timeStart = DateTime.MinValue;
                }
                if (timeEnd == null)
                {
                    timeEnd = DateTime.MaxValue;
                }
                if (typeGet == (byte)EnumType.ByType.Member)
                {
                    var topAccount = _repositoryContext.Accounts.Where(a => a.Role == (byte)EnumType.Role.Member).
                        OrderByDescending(a => (from ord in _repositoryContext.OrderProducts
                                                where a.AccountID == ord.CreatedBy && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                                select ord.Total).Sum()).Take(number).ToList();
                    var result =new List<TopAccountModel>();
                    foreach(var acc in topAccount)
                    {
                        var record = new TopAccountModel()
                        {
                            AccountID = acc.AccountID,
                            Username = acc.Username,
                            FullName = acc.FullName,
                            Email = acc.Email,
                            Phone = acc.Phone,
                            TotalBuy = (from ord in _repositoryContext.OrderProducts
                                        where acc.AccountID == ord.CreatedBy && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                        select ord.Total).Sum()
                        };
                        result.Add(record);
                    }
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                if (typeGet == (byte)EnumType.ByType.Staff)
                {
                    var topAccount = _repositoryContext.Accounts.Where(a => a.Role == (byte)EnumType.Role.Staff).
                        OrderByDescending(a => (from ord in _repositoryContext.OrderProducts
                                                where a.AccountID == ord.CreatedBy && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                                select ord.Total).Sum()).Take(number).ToList();
                    var result = new List<TopAccountModel>();
                    foreach (var acc in topAccount)
                    {
                        var record = new TopAccountModel()
                        {
                            AccountID = acc.AccountID,
                            Username = acc.Username,
                            FullName = acc.FullName,
                            Email = acc.Email,
                            Phone = acc.Phone,
                            TotalBuy = (from ord in _repositoryContext.OrderProducts
                                        where acc.AccountID == ord.CreatedBy && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                        select ord.Total).Sum()
                        };
                        result.Add(record);
                    }
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                if (typeGet == (byte)EnumType.ByType.Product)
                {
                    var topProduct = _repositoryContext.Products.
                        OrderByDescending(a => (from ord in _repositoryContext.OrderDetails
                                                where a.ProductID == ord.ProductID && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                                select ord.Price * ord.Amount).Sum()).Take(number).ToList();
                    var result = new List<TopProductModel>();
                    foreach (var pro in topProduct)
                    {
                        var record = new TopProductModel()
                        {
                            ProductID = pro.ProductID,
                            ProductName = pro.ProductName,
                            Condition = pro.Condition,
                            Status = pro.Status,
                            AmountBuy = (from ord in _repositoryContext.OrderDetails
                                         where pro.ProductID == ord.ProductID && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                         select ord.Amount).Sum(),
                            NumberView = pro.NumberView,
                            Total = (from ord in _repositoryContext.OrderDetails
                                     where pro.ProductID == ord.ProductID && ord.CreatedDate >= timeStart && ord.CreatedDate <= timeEnd
                                     select ord.Price * ord.Amount).Sum()
                        };
                        result.Add(record);
                    }
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                return new ApiReponse()
                {
                    Success = false
                };
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
    }
}
