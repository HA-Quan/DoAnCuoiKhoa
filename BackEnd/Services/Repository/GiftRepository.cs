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
    public interface IGiftRepository : IRepositoryBase<Gift>
    {
        PagingData<Gift> GetByFilter(string keyword, int? sort, bool? status, int pageSize, int pageNumber);
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class GiftRepository : RepositoryBase<Gift>, IGiftRepository
    {
        private readonly IGiftByProductRepository _giftByProductRepository;
        public GiftRepository(RepositoryContext repositoryContext, IGiftByProductRepository giftByProductRepository) : base(repositoryContext)
        {
            _giftByProductRepository = giftByProductRepository;
        }
        public PagingData<Gift> GetByFilter(string keyword, int? sort, bool? status, int pageSize, int pageNumber)
        {
            var respone = new PagingData<Gift>();
            var result = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.GiftCode.ToLower().Contains(keyword)).ToList();
            
            if (status != null)
            {
                result = result.Where(p => p.Status == status).ToList();
            }

            switch (sort)
            {
                case (int)EnumType.Sort.TimeAsc:
                    result = result.OrderBy(s => s.CreatedDate).ToList();
                    break;

                case (int)EnumType.Sort.TimeDesc:
                    result = result.OrderByDescending(s => s.CreatedDate).ToList();
                    break;

                case (int)EnumType.Sort.StatusAsc:
                    result = result.OrderBy(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.StatusDesc:
                    result = result.OrderByDescending(s => s.Status).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameAsc:
                    result = result.OrderBy(s => s.GiftCode).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameDesc:
                    result = result.OrderByDescending(s => s.GiftCode).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;
                
                default:
                    result = result.OrderByDescending(s => s.ModifiedDate).ToList();
                    break;
            }
            var maxResult = result.Count();

            respone.TotalCount = maxResult;
            var maxPageNumber = (maxResult % pageSize == 0) ? (maxResult / pageSize) : (maxResult / pageSize + 1);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            respone.Data = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
            return respone;
        }
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var records = new List<Gift>();
            foreach (var id in listID)
            {
                var record = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.GiftID == id).FirstOrDefault();
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
            var records = new List<Gift>();
            foreach (var id in listID)
            {
                var record = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.GiftID == id).FirstOrDefault();
                if (record == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    _giftByProductRepository.DeleteByGiftID(id);
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
