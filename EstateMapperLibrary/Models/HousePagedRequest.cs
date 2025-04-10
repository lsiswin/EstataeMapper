using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class HousePagedRequest:PagedRequest
    {
        public string? Search { get; set; }
        public int? RegionId { get; set; }
        public int? Price {  get; set; }

    }
}
