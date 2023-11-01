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
    public interface IStorageRepository : IRepositoryBase<Storage>
    {
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class StorageRepository : RepositoryBase<Storage>, IStorageRepository
    {
        public StorageRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var listStorage = new List<Storage>();
            foreach (var id in listID)
            {
                var storage = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.StorageID == id).FirstOrDefault();
                if (storage == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    storage.Status = status;
                    storage.ModifiedDate = DateTime.Now;
                    listStorage.Add(storage);
                }
            }
            if (check)
            {
                UpdateMultiple(listStorage);
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
            var listStorage = new List<Storage>();
            foreach (var id in listID)
            {
                var storage = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.StorageID == id).FirstOrDefault();
                if (storage == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    storage.DelFalg = EnumType.DeleteFlag.Deleted;
                    storage.ModifiedDate = DateTime.Now;
                    listStorage.Add(storage);
                }
            }
            if (check)
            {
                UpdateMultiple(listStorage);
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
