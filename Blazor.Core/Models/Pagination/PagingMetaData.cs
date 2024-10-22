namespace CardManagement.Core.Models.Pagination
{
    public class PagingMetaData
    {
        //
        // Summary:
        //     Gets or sets a value indicating whether has next page.
        public bool HasNextPage { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether has previous page.
        public bool HasPreviousPage { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether is first page.
        public bool IsFirstPage { get; set; }

        //
        // Summary:
        //     Gets or sets a value indicating whether is last page.
        public bool IsLastPage { get; set; }

        //
        // Summary:
        //     Gets or sets the item end.
        public int ItemEnd { get; set; }

        //
        // Summary:
        //     Gets or sets the item start.
        public int ItemStart { get; set; }

        //
        // Summary:
        //     Gets or sets the page count.
        public int PageCount { get; set; }

        //
        // Summary:
        //     Gets or sets the page index.
        public int PageIndex { get; set; }

        //
        // Summary:
        //     Gets or sets the page number.
        public int PageNumber { get; set; }

        //
        // Summary:
        //     Gets or sets the page size.
        public int PageSize { get; set; }

        //
        // Summary:
        //     Gets or sets the total item count.
        public int TotalItemCount { get; set; }
    }
}
