using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwaj.BL.Helper
{
    public class PageinationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PageinationHeader(int currentPage,int itemsPerPage,int TotalItems,int totalPages)
        {
            this.CurrentPage = currentPage; 
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = TotalItems;
           this.TotalPages = totalPages;
        }
    }
}
