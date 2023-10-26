using Services.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class TopProductModel
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public byte Condition { get; set; }
        public byte Status { get; set; }
        public int NumberView { get; set; }
        public int AmountBuy { get; set; }
        public long Total {  get; set; }
    }
}
