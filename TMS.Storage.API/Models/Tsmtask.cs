using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tsmtask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string State { get; set; }
    }
}
