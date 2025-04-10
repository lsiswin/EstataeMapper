using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperLibrary.Models
{
    // 分页参数基类（可继承扩展）
    public class PagedRequest
    {
        private int _pageNumber =1;
        private int _pageSize=10;

        /// <summary>
        /// 当前页码（默认1）
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        /// <summary>
        /// 每页数量（默认10，最大100）
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value switch
            {
                > 100 => 100,
                < 1 => 10,
                _ => value
            };
        }
    }
}
