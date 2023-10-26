using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ProductView
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string? MainImage { get; set; }

        public byte? Discount { get; set; } = 0;

        public byte Condition { get; set; } = 0;
        public string ChipDetail { get; set; }
        public string MemoryDetail { get; set; }
        public string DisplayDetail { get; set; }
        public string CardDetail { get; set; }
        public string StorageDetail { get; set; }
        public int Quantity { get; set; }
        public int NumberSell { get; set; }
        public long Price { get; set; }
        public byte Status { get; set; }
    }
}
