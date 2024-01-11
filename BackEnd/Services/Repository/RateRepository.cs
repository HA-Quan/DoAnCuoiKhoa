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
    public interface IRateRepository : IRepositoryBase<Rate>
    {
        PagingData<RateView> GetByFilter(byte status, int pageSize, int pageNumber);
        void DeleteByProductID(Guid productID);
        void DeleteByAccountID(Guid accountID);
    }
    public class RateRepository : RepositoryBase<Rate>, IRateRepository
    {
        public RateRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public PagingData<RateView> GetByFilter(byte status, int pageSize, int pageNumber) {
            var respone = new PagingData<RateView>();
            var result = new List<Rate>();
            result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && 
                _repositoryContext.Products.Any(p => p.ProductID == a.ProductID && p.DelFalg == EnumType.DeleteFlag.Using)).ToList();
            if (status == (byte)EnumType.StatusRate.NotReply) {
                result = result.Where(o => string.IsNullOrEmpty(o.CommentReply)).ToList();
            }
            else {
                result = result.Where(o => !string.IsNullOrEmpty(o.CommentReply)).ToList();
            }

            result = result.OrderByDescending(s => s.CreatedBy).ToList();
            
            var maxResult = result.Count;
            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;

            respone.Data = ConfigRateToView(result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            return respone;

        }
        public List<RateView> ConfigRateToView(List<Rate> rates) {

            var result = new List<RateView>();
            foreach (var item in rates) {
                var product = _repositoryContext.Products.Where(o => o.ProductID == item.ProductID).FirstOrDefault();
                result.Add(new RateView {
                    ProductID = item.ProductID,
                    RateID = item.RateID,
                    ProductName = product != null ? product.ProductName : string.Empty,
                    ProductImage = product != null ? product.MainImage : string.Empty,
                    Condition = product.Condition,
                    RateBy = _repositoryContext.Accounts.Where(a => a.AccountID == item.AccountID).FirstOrDefault().Username,
                    Comment = item.Comment,
                    CommentImage = !string.IsNullOrEmpty(item.ListImg) ? item.ListImg.Split(",").ToList() : null,
                    ReplyComment = item.CommentReply,
                    RateDate = item.CreatedDate,
                    NumStar = item.NumStar,
                    RateCount = _repositoryContext.Rates.Where(r => r.DelFalg == EnumType.DeleteFlag.Using &&
                    r.ProductID == item.ProductID).Count(),
                    RateAverage = GetRateAvg(item.ProductID)

                });
            }
            return result;

        }
        public float GetRateAvg(Guid productId) {
            var listRate = _repositoryContext.Rates.Where(r => r.DelFalg == EnumType.DeleteFlag.Using &&
                    r.ProductID == productId).ToList();
            var sum = 0;
            foreach ( var item in listRate) {
                sum += item.NumStar; 
            }
            return (float)sum/listRate.Count;
        }
        public void DeleteByProductID(Guid productID)
        {
            var listRate = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.ProductID == productID).ToList();
            foreach (var rate in listRate)
            {
                rate.DelFalg = EnumType.DeleteFlag.Deleted;
                rate.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(listRate);
            Save();
        }
        public void DeleteByAccountID(Guid accountID)
        {
            var listRate = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.CreatedBy == accountID).ToList();
            foreach (var rate in listRate)
            {
                rate.DelFalg = EnumType.DeleteFlag.Deleted;
                rate.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(listRate);
            Save();
        }
    }
}
