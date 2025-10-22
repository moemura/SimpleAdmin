namespace SimpleAdmin.Models
{
    public class PaginatedList<T>
    {
        public IEnumerable<T>? Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ItemCount { get; set; }
        public int PageCount { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public PaginatedList(IEnumerable<T> data,int pageIndex,int pageSize,int itemCount)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            ItemCount = itemCount;
            PageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            HasNext = PageIndex < PageCount;
            HasPrevious = PageIndex > 1;
        }
        public PaginatedList(){}
    }
}
