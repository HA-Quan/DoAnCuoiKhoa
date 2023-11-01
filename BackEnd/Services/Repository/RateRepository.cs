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
        void DeleteByProductID(Guid productID);
        void DeleteByAccountID(Guid accountID);
    }
    public class RateRepository : RepositoryBase<Rate>, IRateRepository
    {
        public RateRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
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
