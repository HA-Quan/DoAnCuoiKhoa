using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ImportView
    {
        public ImportProduct? InfoImport { get; set; }
        public List<ImportDetailModel> ListImportDetail { get; set; } = new List<ImportDetailModel>();
    }
}
