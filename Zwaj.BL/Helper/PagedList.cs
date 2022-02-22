using Microsoft.EntityFrameworkCore;

namespace Zwaj.BL.Helper
{
    public class PagedList<T>:List<T> 
    {
        public PagedList(List<T> items,int count,int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }
        public static async Task<PagedList<T>> GreateAsync(IQueryable<T> ts,int pageNumber,int pageSize)
        {
            var count = await ts.CountAsync();
            var items = await ts.Skip((pageNumber - 1) *pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T> (items, count, pageNumber, pageSize);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
