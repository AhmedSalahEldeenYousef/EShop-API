﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Core.Shared
{
    public class ProductParams
    {
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get; set; }
        public int MaxPageSize { get; set; } = 6;
        private int _pagesize = 3;
        public int PageSize
        {
            get { return _pagesize; }
            set { _pagesize = value> MaxPageSize? MaxPageSize  : value; }
        }
        public int PageNumber { get; set; } = 1;
    }
}
