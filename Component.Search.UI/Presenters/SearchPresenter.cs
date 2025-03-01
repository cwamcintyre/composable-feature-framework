using Component.Search.Model;
using Component.Search.UI.Service.Model;

namespace Component.Search.UI.Presenters;

public interface ISearchPresenter
{
    Task<SearchIndexModel> HandleIndex(GetSearchIndexResponseModel responseModel);
    Task<SearchViewModel> HandleResults(SearchDetail searchDetail, GetDataResponseModel responseModel);
}

public class SearchPresenter : ISearchPresenter
{
    public const int PageSize = 10;
    public virtual string IndexViewName { get; set; } = "Index";

    public virtual string ResultViewName { get; set; } = "SearchResult";

    public async Task<SearchIndexModel> HandleIndex(GetSearchIndexResponseModel responseModel)
    {
        var viewModel = new SearchIndexModel
        {
            ViewName = IndexViewName,
            SearchIndexType = responseModel.SearchIndexType
        };

        return viewModel;
    }

    public async Task<SearchViewModel> HandleResults(SearchDetail searchDetail, GetDataResponseModel responseModel)
    {
        var totalPages = (int)Math.Ceiling((double)responseModel.TotalItems / PageSize);
        var currentPage = searchDetail.PageNumber;
        
        var viewModel = new SearchViewModel
        {
            ViewName = ResultViewName,
            SearchDetail = searchDetail,
            SearchTypeModel = responseModel.SearchType,
            Data = responseModel.Data,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };

        if (totalPages > 1)
        {
            if (currentPage > 1)
            {
                viewModel.PreviousPage = currentPage - 1;
            }
            if (currentPage < totalPages)
            {
                viewModel.NextPage = currentPage + 1;
            }

            if (totalPages <= 5)
            {
                viewModel.PaginationItems = Enumerable.Range(1, totalPages).ToList();
            }
            else
            {
                var paginationItems = new List<int> { 1 };
                if (currentPage > 3)
                {
                    paginationItems.Add(-1); // Ellipsis
                }
                var startPage = Math.Max(2, currentPage - 1);
                var endPage = Math.Min(totalPages - 1, currentPage + 1);
                paginationItems.AddRange(Enumerable.Range(startPage, endPage - startPage + 1));
                if (endPage < totalPages - 1)
                {
                    paginationItems.Add(-1); // Ellipsis
                }
                paginationItems.Add(totalPages);
                viewModel.PaginationItems = paginationItems;
            }
        }

        return viewModel;
    }
}

public class SearchIndexModel
{
    public string ViewName { get; set; } = string.Empty;
    public SearchIndexTypeModel SearchIndexType { get; set; }
}

public class SearchViewModel
{
    public string ViewName { get; set; } = string.Empty;
    public SearchTypeModel SearchTypeModel { get; set; } = new SearchTypeModel();
    public List<Dictionary<string, string>> Data { get; set; } = new List<Dictionary<string, string>>();
    public SearchDetail SearchDetail { get; set; } = new SearchDetail();
    public int PreviousPage { get; set; }
    public int NextPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public List<int> PaginationItems { get; set; } = new List<int>();
}