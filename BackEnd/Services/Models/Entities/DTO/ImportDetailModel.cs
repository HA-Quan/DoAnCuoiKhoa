using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ImportDetailModel
    {
        public Guid ImportDetailID { get; set; }
        public Guid ImportID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public byte Condition { get; set; }
        public long PriceImport { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
