using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public List<SubRegion> SubRegions { get; set; } = new();

    }
}
