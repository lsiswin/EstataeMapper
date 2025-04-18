using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class House
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }//楼盘名称
        [Required]
        public string MainImageUrl { get; set; }//主图
        // 地址结构
        public int SubRegionId { get; set; }      // 所属区县
        [StringLength(200)]
        public string DetailAddress { get; set; } // 详细地址
        public List<Layout> Layouts { get; set; } = new();// 户型
        public int Price { get; set; }//单价
        public List<Tag>? Tags { get; set; } = new();
    }
}
