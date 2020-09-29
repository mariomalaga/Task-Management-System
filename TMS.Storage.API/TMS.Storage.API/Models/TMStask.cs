using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace API.Models
{
    public partial class TMStask
    {
        [BindNever]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [BindNever]
        public DateTime? StartDate { get; set; }
        [BindNever]
        public DateTime? FinishDate { get; set; }
        public string State { get; set; }
    }
}
