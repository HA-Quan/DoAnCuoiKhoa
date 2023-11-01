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
    }
}
