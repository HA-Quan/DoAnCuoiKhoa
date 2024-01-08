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
    public interface IImportProductRepository : IRepositoryBase<ImportProduct>
    {
        void DeleteByAccountID(Guid accountID);
        void DeleteBySupplierID(Guid supplierID);
        PagingData<ImportProduct> GetByFilter(DateTime timeStart, DateTime timeEnd, Guid supplier, int? sort, int pageSize, int pageNumber);
    }
    public class ImportProductRepository : RepositoryBase<ImportProduct>, IImportProductRepository
    {
        public ImportProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public void DeleteByAccountID(Guid accountID)
        {
            var listImport = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.CreatedBy == accountID).ToList();
            foreach (var item in listImport)
            {
                item.DelFalg = EnumType.DeleteFlag.Deleted;
                item.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(listImport);
            Save();
        }
        public void DeleteBySupplierID(Guid supplierID)
        {
            var listImport = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.SupplierID == supplierID).ToList();
            foreach (var item in listImport)
            {
                item.DelFalg = EnumType.DeleteFlag.Deleted;
                item.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(listImport);
            Save();
        }
        public PagingData<ImportProduct> GetByFilter(DateTime timeStart, DateTime timeEnd, Guid supplier, int? sort, int pageSize, int pageNumber)
        {
            if (timeStart != DateTime.MinValue)
            {
                timeStart = timeStart.AddHours(-12);
            }
            if (timeEnd != DateTime.MaxValue)
            {
                timeEnd = timeEnd.AddHours(12);
            }
            var respone = new PagingData<ImportProduct>();
            var result = new List<ImportProduct>();
            result = FindByCondition(a => a.DelFalg == EnumType.DeleteFlag.Using && a.CreatedDate >= timeStart && a.CreatedDate <= timeEnd).ToList();
            if(supplier!= Guid.Empty)
            {
                result = result.Where(r => r.SupplierID == supplier).ToList();
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
                    result = result.OrderBy(s => s.Price).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.PriceDesc:
                    result = result.OrderByDescending(s => s.Price).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameAsc:
                    result = result.OrderBy(s => s.SupplierName).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.NameDesc:
                    result = result.OrderByDescending(s => s.SupplierName).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.QuantityAsc:
                    result = result.OrderBy(s => s.Amount).ThenByDescending(s => s.ModifiedDate).ToList();
                    break;

                case (int)EnumType.Sort.QuantityDesc:
                    result = result.OrderByDescending(s => s.Amount).ThenByDescending(s => s.ModifiedDate).ToList();
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
    }
}
