using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    public class TagDto
    {
        public int Id { get; set; }
        private string tagName;

        public string TagName
        {
            get { return tagName; }
            set { tagName = value;  }
        }

    }
}
