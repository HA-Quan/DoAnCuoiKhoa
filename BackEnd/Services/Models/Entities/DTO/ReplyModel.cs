using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ReplyModel {
        public Guid RateID { get; set; }
        public Guid ModifiedBy { get; set; }
        public string? ReplyComment { get; set; }
    }
}
