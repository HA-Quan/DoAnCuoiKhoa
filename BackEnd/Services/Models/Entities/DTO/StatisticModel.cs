using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class StatisticModel
    {
        public List<AreaChartModel>? AreaChart { get; set; }
        public List<PieChartModel>? PieChart { get; set; }
    }
}
