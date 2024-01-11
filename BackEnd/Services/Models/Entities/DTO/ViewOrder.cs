using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ViewOrder {
        public OrderProduct Order { get; set; }
        public List<OrderItemModel>? ListOrderDetail { get; set; }
    }
}
