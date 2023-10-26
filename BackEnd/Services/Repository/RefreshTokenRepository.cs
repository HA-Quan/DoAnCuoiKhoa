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
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {

        RefreshToken GetRefreshToken(string key);
        void DeleteRefreshTokenByAccountID(Guid accountID);
    }
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public RefreshToken GetRefreshToken(string key)
        {
            return _repositoryContext.RefreshTokens.FirstOrDefault(x => x.Token.Equals(key)); 
        }
        public void DeleteRefreshTokenByAccountID(Guid accountID)
        {
            var listRefreshToken = FindByCondition(r => r.DelFalg == EnumType.DeleteFlag.Using && r.CreatedBy == accountID).ToList();
            foreach (var item in listRefreshToken)
            {
                item.DelFalg = EnumType.DeleteFlag.Deleted;
            }
            UpdateMultiple(listRefreshToken);
            Save();
        }
    }
}
