using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Report.API.Models
{
    public class CSVModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string State { get; set; }
    }
    public class Root
    {
        public List<CSVModel> CSVModel { get; set; }
    }
}
