using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class LayoutDto
    {
        public int Id { get; set; }
        public int LayoutName { get; set; }//户型面积大小
        public string? LayoutUrl { get; set; }//户型图URL
        public string? Description { get; set; }//户型描述
    }
}
