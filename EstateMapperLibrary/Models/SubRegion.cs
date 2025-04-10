using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class SubRegion
    {

        public int SubRegionId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }//区县名称
        public int RegionId { get; set; }//省市Id
        [JsonIgnore]
        public Region Region { get; set; }
        // 反向导航
        [JsonIgnore]
        public List<House> Houses { get; set; }
    }
}
