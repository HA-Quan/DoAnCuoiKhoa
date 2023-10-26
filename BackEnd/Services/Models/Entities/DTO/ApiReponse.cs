using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ApiReponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
    }
}
