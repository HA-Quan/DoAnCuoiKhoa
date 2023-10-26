﻿using Services.Helpers;
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
    public interface IPromotionRepository : IRepositoryBase<Promotion>
    {
        int UpdateMultiple(List<Guid> listID, bool status);
        int Delete(List<Guid> listID);
    }
    public class PromotionRepository : RepositoryBase<Promotion>, IPromotionRepository
    {
        public PromotionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
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
