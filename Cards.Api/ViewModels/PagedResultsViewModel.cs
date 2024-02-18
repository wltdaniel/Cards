namespace Cards.Api.ViewModels
{
  
        public class PagedResultsViewModel<T>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public int TotalCount { get; set; }
            public IEnumerable<T>? Results { get; set; }
        }
    
}
