using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class StatisticModel
    {
        public long TotalCapital { get; set; }
        public long TotalRevenue { get; set; }
        public List<AreaChartModel> AreaChart { get; set; } = new List<AreaChartModel>();
        public List<PieChartModel> PieChart { get; set; } = new List<PieChartModel>();
    }
}
