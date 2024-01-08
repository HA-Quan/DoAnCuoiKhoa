using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Entities.DTO
{
    public class ImportModel
    {
        public ImportProduct? InfoImport { get; set; }
        public List<ImportDetail> ListImportDetail { get; set; } = new List<ImportDetail>();
    }
}
