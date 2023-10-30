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
        ApiReponse GetStatistic(DateTime? timeStart, DateTime? timeEnd, byte typeGet);
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
        public ApiReponse GetStatistic(DateTime timeStart, DateTime timeEnd, byte typeGet)
        {
            try
            {
                var result = new StatisticModel();
                var query = (from order in _repositoryContext.OrderDetails
                             join p in _repositoryContext.Products on order.ProductID equals p.ProductID
                             join cate in _repositoryContext.Categorys on p.CategoryID equals cate.CategoryID
                             where order.CreatedDate >= timeStart && order.CreatedDate < timeEnd
                             select new
                             {
                                 Trade = cate.Trademark,
                                 Price = order.Amount * order.Price,
                             }).ToList();
                var kq = query.GroupBy(x => x.Trade).Select(x => new
                {
                    Name = x.Key,
                    Total = x.Sum(y => y.Price)
                });
                foreach (var item in kq)
                {
                    result.PieChart.Add(new PieChartModel()
                    {
                        Label = item.Name,
                        Value = item.Total
                    });
                }
                var dateStart = timeStart;
                var dateEnd = timeStart;
                if (typeGet == (byte)EnumType.ByType.ByYear)
                {
                    dateEnd = new DateTime(dateEnd.Year + 1, 1, 1);
                    while (dateEnd <= timeEnd)
                    {
                        var areaChart = new AreaChartModel()
                        {
                            Label = dateStart.Year.ToString(),
                            Capital = (from ip in _repositoryContext.ImportProducts
                                       where dateStart <= ip.CreatedDate && dateEnd > ip.CreatedDate
                                       select ip.Price * ip.Amount).Sum(),
                            Revenue = (from order in _repositoryContext.OrderProducts
                                       where dateStart <= order.CreatedDate && dateEnd > order.CreatedDate
                                       select order.Total).Sum()
                        };
                        result.AreaChart.Add(areaChart);
                        dateStart = dateEnd;
                        dateEnd = new DateTime(dateEnd.Year + 1, 1, 1);
                    }
                    return new ApiReponse()
                    {
                        Success = true,
                        Data = result
                    };
                }
                if (typeGet == (byte)EnumType.ByType.ByMonth)
                {
                    dateEnd = new DateTime(dateEnd.Year, dateEnd.Month + 1, 1);
                    while (dateEnd <= timeEnd)
                    {
                        var areaChart = new AreaChartModel()
                        {
                            Label = dateStart.Month.ToString("MMMM"),
                            Capital = (from ip in _repositoryContext.ImportProducts
                                       where dateStart <= ip.CreatedDate && dateEnd > ip.CreatedDate
                                       select ip.Price * ip.Amount).Sum(),
                            Revenue = (from order in _repositoryContext.OrderProducts
                                       where dateStart <= order.CreatedDate && dateEnd > order.CreatedDate
                                       select order.Total).Sum()
                        };
                        result.AreaChart.Add(areaChart);
                        dateStart = dateEnd;
                        dateEnd = new DateTime(dateEnd.Year, dateEnd.Month + 1, 1);

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
