using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rates.API.ResourceParameters
{
    public class UsersResourceParameters
    {
        const int MAX_PAGE_SIZE = 20;
        public string LastName { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;

        private int _pageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }

        public string Fields { get; set; }

    }
}
