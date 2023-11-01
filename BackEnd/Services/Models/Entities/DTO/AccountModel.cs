using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class AccountModel
    {
        public Account Account { get; set; }
        public IFormFile? Image { get; set; }
    }
}
