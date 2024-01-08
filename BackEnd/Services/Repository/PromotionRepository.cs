using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Models.Enum.EnumType;

namespace Services.Repository
{
    public interface IPromotionRepository : IRepositoryBase<Promotion>
    {
        PagingData<Promotion> GetByFilter(DateTime timeStart, DateTime timeEnd, string keyword, int? sort,
            bool? status, int pageSize, int pageNumber);
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        public PromotionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public PagingData<Promotion> GetByFilter(DateTime timeStart, DateTime timeEnd, string keyword, int? sort,
            bool? status, int pageSize, int pageNumber) {
            if (timeStart != DateTime.MinValue) {
                timeStart = timeStart.AddHours(-12);
            }
            if (timeEnd != DateTime.MaxValue) {
                timeEnd = timeEnd.AddHours(12);
            }
            var respone = new PagingData<Promotion>();
            var result = new List<Promotion>();
            result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.DayStart >= timeStart && a.DayExpired <= timeEnd && a.PromotionCode.Contains(keyword)).ToList();
            
            if (status != null) {
                result = result.Where(o => o.Status == status).ToList();
            }
            switch (sort) {
                case (int)EnumType.SortPromotion.CodeAsc:
                    result = result.OrderBy(s => s.PromotionCode).ToList();
                    break;

                case (int)EnumType.SortPromotion.CodeDesc:
                    result = result.OrderByDescending(s => s.PromotionCode).ToList();
                    break;

                case (int)EnumType.SortPromotion.DiscountAsc:
                    result = result.OrderBy(s => s.Discount).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.DiscountDesc:
                    result = result.OrderByDescending(s => s.Discount).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.DayStartAsc:
                    result = result.OrderBy(s => s.DayStart).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.DayStartDesc:
                    result = result.OrderByDescending(s => s.DayStart).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.DayExpiredAsc:
                    result = result.OrderBy(s => s.DayExpired).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.DayExpiredDesc:
                    result = result.OrderByDescending(s => s.DayExpired).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.ProvisoAsc:
                    result = result.OrderBy(s => s.Proviso).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.ProvisoDesc:
                    result = result.OrderByDescending(s => s.Proviso).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;
                case (int)EnumType.SortPromotion.QuantityAsc:
                    result = result.OrderBy(s => s.Quantity).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.QuantityDesc:
                    result = result.OrderByDescending(s => s.Quantity).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;
                case (int)EnumType.SortPromotion.NumUsedAsc:
                    result = result.OrderBy(s => s.NumUsed).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.NumUsedDesc:
                    result = result.OrderByDescending(s => s.NumUsed).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;
                case (int)EnumType.SortPromotion.TimeAsc:
                    result = result.OrderBy(s => s.CreatedDate).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.SortPromotion.TimeDesc:
                    result = result.OrderByDescending(s => s.CreatedDate).ThenByDescending(s => s.ModifiedDate).ToList();
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
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var records = new List<Promotion>();
            foreach (var id in listID)
            {
                var record = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.PromotionID == id).FirstOrDefault();
                if (record == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    record.Status = status;
                    record.ModifiedDate = DateTime.Now;
                    records.Add(record);
                }
            }
            if (check)
            {
                UpdateMultiple(records);
                Save();
                return listID.Count;
            }
            else
            {
                return 0;
            }
        }

        public int Delete(List<Guid> listID)
        {
            bool check = true;
            var records = new List<Promotion>();
            foreach (var id in listID)
            {
                var record = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.PromotionID == id).FirstOrDefault();
                if (record == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    record.DelFalg = EnumType.DeleteFlag.Deleted;
                    record.ModifiedDate = DateTime.Now;
                    records.Add(record);
                }
            }
            if (check)
            {
                UpdateMultiple(records);
                Save();
                return listID.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
