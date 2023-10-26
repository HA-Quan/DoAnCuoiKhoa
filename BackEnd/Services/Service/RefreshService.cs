using Microsoft.Extensions.Configuration;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Repository;
using Services.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public interface IRefreshService
    {
    }

    internal class RefreshService : BaseService<RefreshToken>, IRefreshService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshService(IRefreshTokenRepository refreshTokenRepository) : base(refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        
    }
}
