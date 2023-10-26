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
    public interface IChipRepository : IRepositoryBase<Chip>
    {
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class ChipRepository : RepositoryBase<Chip>, IChipRepository
    {
        public ChipRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        public int UpdateMultiple(List<Guid> listID, bool status)
        {
            bool check = true;
            var listChip = new List<Chip>();
            foreach (var chipId in listID)
            {
                var chip = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ChipID == chipId).FirstOrDefault();
                if (chip == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    chip.Status = status;
                    listChip.Add(chip);
                }
            }
            if (check)
            {
                UpdateMultiple(listChip);
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
            var listChip = new List<Chip>();
            foreach (var chipID in listID)
            {
                var chip = FindByCondition(p => p.DelFalg == EnumType.DeleteFlag.Using && p.ChipID == chipID).FirstOrDefault();
                if (chip == null)
                {
                    check = false;
                    break;
                }
                else
                {
                    chip.DelFalg = EnumType.DeleteFlag.Deleted;
                    listChip.Add(chip);
                }
            }
            if (check)
            {
                UpdateMultiple(listChip);
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
