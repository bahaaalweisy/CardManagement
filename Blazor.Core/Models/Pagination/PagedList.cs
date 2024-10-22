namespace CardManagement.Core.Models.Pagination
{
    public class PagedList<T> : List<T>
    {
        //
        // Summary:
        //     Gets a value indicating whether has next page.
        public bool HasNextPage { get; }

        //
        // Summary:
        //     Gets a value indicating whether has previous page.
        public bool HasPreviousPage { get; }

        //
        // Summary:
        //     Gets a value indicating whether is first page.
        public bool IsFirstPage { get; }

        //
        // Summary:
        //     Gets a value indicating whether is last page.
        public bool IsLastPage { get; }

        //
        // Summary:
        //     Gets the item end.
        public int ItemEnd { get; }

        //
        // Summary:
        //     Gets the item start.
        public int ItemStart { get; }

        //
        // Summary:
        //     Gets the page count.
        public int PageCount { get; }

        //
        // Summary:
        //     Gets the page index.
        public int PageIndex { get; }

        //
        // Summary:
        //     The page number.
        public int PageNumber => PageIndex + 1;

        //
        // Summary:
        //     Gets the page size.
        public int PageSize { get; }

        //
        // Summary:
        //     Gets the total item count.
        public int TotalItemCount { get; }

        //
        // Summary:
        //     Initializes a new instance of the Adf.Core.PagedList`1 class.
        //
        // Parameters:
        //   source:
        //     The source.
        //
        //   index:
        //     The index.
        //
        //   pageSize:
        //     The page size.
        //
        //   totalCount:
        //     The total count.
        public PagedList(IEnumerable<T> source, int index, int pageSize, int? totalCount = null)
            : this(source.AsQueryable(), index, pageSize, totalCount)
        {
        }

        //
        // Summary:
        //     Initializes a new instance of the Adf.Core.PagedList`1 class.
        //
        // Parameters:
        //   source:
        //     The source.
        //
        //   pageNum:
        //     The page num.
        //
        //   pageSize:
        //     The page size.
        //
        //   totalCount:
        //     The total count.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        public PagedList(IQueryable<T> source, int pageNum, int pageSize, int? totalCount = null)
        {
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException("pageNum", "Value can not be below 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            Type typeFromHandle = typeof(T);
            int num = source.Count();
            PageSize = pageSize;
            PageIndex = pageNum - 1;
            TotalItemCount = totalCount ?? num;
            PageCount = ((TotalItemCount > 0) ? ((int)Math.Ceiling((double)TotalItemCount / (double)PageSize)) : 0);
            HasPreviousPage = PageIndex > 0;
            HasNextPage = PageIndex < PageCount - 1;
            IsFirstPage = PageIndex <= 0;
            IsLastPage = PageIndex >= PageCount - 1;
            ItemStart = PageIndex * PageSize + 1;
            ItemEnd = Math.Min(PageIndex * PageSize + PageSize, TotalItemCount);
            if (TotalItemCount > 0)
            {
                int num2 = (int)Math.Ceiling((double)num / (double)PageSize);
                if (num < TotalItemCount && num2 <= PageIndex)
                {
                    AddRange(source.Skip((num2 - 1) * PageSize).Take(PageSize));
                }
                else
                {
                    AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
                }
            }
        }

        //
        // Summary:
        //     The get paging meta data.
        //
        // Returns:
        //     The Adf.Core.PagingMetaData.
        public PagingMetaData GetPagingMetaData()
        {
            return new PagingMetaData
            {
                HasNextPage = HasNextPage,
                HasPreviousPage = HasPreviousPage,
                IsFirstPage = IsFirstPage,
                IsLastPage = IsLastPage,
                ItemEnd = ItemEnd,
                ItemStart = ItemStart,
                PageCount = PageCount,
                PageIndex = PageIndex,
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalItemCount = TotalItemCount
            };
        }
    }
}
