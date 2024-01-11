using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Models.Entities.DTO
{
    public class RateModel
    {
        public Rate Rate { get; set; }
        public IFormCollection? ListImages { get; set; }
        
    }
}
