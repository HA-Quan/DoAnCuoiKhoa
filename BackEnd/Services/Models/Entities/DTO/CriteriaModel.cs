using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class CriteriaModel
    {
        public string Title { get; set; }
        public List<SelectModel> SelectModels { get; set; } = new List<SelectModel>();

    }
}
