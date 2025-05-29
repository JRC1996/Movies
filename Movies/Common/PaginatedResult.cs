namespace Movies.Common
{
    using Microsoft.EntityFrameworkCore;
    public class PaginatedResult<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public IEnumerable<T> Items { get; private set; }


        public PaginatedResult(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;


        public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
          pageIndex = Math.Max(1, pageIndex); 
          pageSize = Math.Max(1, pageSize); 
            
          var count = await source.CountAsync();
          var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
    
          return new PaginatedResult<T>(items, count, pageIndex, pageSize);
        }

    }
}
