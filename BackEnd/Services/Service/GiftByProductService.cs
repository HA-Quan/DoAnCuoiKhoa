using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.Entities;
using Services.Models.Entities.DTO;
using Services.Models.Enum;
using Services.Repository;
using Services.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Service
{
    public interface IGiftByProductService : IServiceBase<GiftByProduct>
    {
        
    }

    internal class GiftByProductService : BaseService<GiftByProduct>, IGiftByProductService
    {
        private readonly IGiftByProductRepository _giftByProductRepository;
        public GiftByProductService(IGiftByProductRepository giftByProductRepository) : base(giftByProductRepository)
        {
            _giftByProductRepository = giftByProductRepository;
        }
        
    }

}
