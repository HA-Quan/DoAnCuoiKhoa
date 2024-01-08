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
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var listCategory = new List<Category>();
            foreach (var categoryID in listID)
            {
                var category = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.CategoryID == categoryID).FirstOrDefault();
                if (category == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    category.Status = status;
                    category.ModifiedDate = DateTime.Now;
                    listCategory.Add(category);
                }
            }
            if (check)
            {
                UpdateMultiple(listCategory);
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
            var listCategory = new List<Category>();
            foreach (var categoryID in listID)
            {
                var category = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.CategoryID == categoryID).FirstOrDefault();
                if (category == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    category.DelFalg = EnumType.DeleteFlag.Deleted;
                    category.ModifiedDate = DateTime.Now;
                    listCategory.Add(category);
                }
            }
            if (check)
            {
                UpdateMultiple(listCategory);
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
