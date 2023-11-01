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
    public interface IGiftByProductRepository : IRepositoryBase<GiftByProduct>
    {
        int CreateMultiple(List<GiftByProduct> giftByProducts);
        int UpdateMultiple(List<GiftByProduct> giftByProducts, Guid productID);
        void DeleteByProductID(Guid productID);
        void DeleteByGiftID(Guid giftID);
    }
    public class GiftByProductRepository : RepositoryBase<GiftByProduct>, IGiftByProductRepository
    {
        public GiftByProductRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        
        public int CreateMultiple(List<GiftByProduct> giftByProducts)
        {
            foreach (var giftByProduct in giftByProducts)
            {
                if (!_repositoryContext.Gifts.Any(g => g.GiftID == giftByProduct.GiftID && g.DelFalg == EnumType.DeleteFlag.Using))
                {
                    return 0;
                }
                
            }
            _repositoryContext.GiftByProducts.AddRange(giftByProducts);
            Save();
            return giftByProducts.Count;
        }
        public int UpdateMultiple(List<GiftByProduct> giftByProducts, Guid productID)
        {
            var listGiftByProducts = FindByCondition(o => o.ProductID == productID && o.DelFalg == EnumType.DeleteFlag.Using).ToList();
            var giftByProductNeedDelete = listGiftByProducts.Except(giftByProducts).ToList();
            var giftByProductNeedAdd = giftByProducts.Except(listGiftByProducts).ToList();
            foreach (var giftByProduct in giftByProductNeedDelete)
            {
                giftByProduct.DelFalg = EnumType.DeleteFlag.Deleted;
                giftByProduct.ModifiedDate = DateTime.Now;
            }

            foreach (var giftByProduct in giftByProductNeedAdd)
            {
                var gift = _repositoryContext.Gifts.FirstOrDefault(g => g.GiftID == giftByProduct.GiftID
                && g.DelFalg == EnumType.DeleteFlag.Using);
                if (gift == null)
                {
                    return 0;
                }
            }
            UpdateMultiple(giftByProductNeedDelete);
            _repositoryContext.GiftByProducts.AddRange(giftByProductNeedAdd);
            Save();
            return giftByProductNeedDelete.Count + giftByProductNeedAdd.Count;

        }
        public void DeleteByProductID(Guid productID)
        {
            var records = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.ProductID == productID).ToList();
            foreach (var item in records)
            {
                item.DelFalg = EnumType.DeleteFlag.Deleted;
                item.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(records);
            Save();
        }
        public void DeleteByGiftID(Guid giftID)
        {
            var records = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.GiftID == giftID).ToList();
            foreach (var item in records)
            {
                item.DelFalg = EnumType.DeleteFlag.Deleted;
                item.ModifiedDate = DateTime.Now;
            }
            UpdateMultiple(records);
            Save();
        }
    }
}
