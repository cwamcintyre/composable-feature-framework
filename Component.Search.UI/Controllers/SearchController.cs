using Component.Search.UI.Presenters;
using Component.Search.UI.Service;
using Component.Search.UI.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Component.Search.UI.Controllers
{
    public class SearchController : Controller
    {
        public const int ItemsPerPage = 10;
        public const string SessionSearchKey = "searchDetails";

        private readonly SearchAPIService _searchAPIService;
        private readonly ISearchPresenter _searchPresenter;

        public SearchController(SearchAPIService searchAPIService, ISearchPresenter searchPresenter)
        {
            _searchAPIService = searchAPIService;
            _searchPresenter = searchPresenter;
        }

        [HttpGet("search/{searchTypeId}")]
        public async Task<ActionResult> Index(string searchTypeId)
        {
            var apiResult = await _searchAPIService.GetSearchIndexTypeAsync(searchTypeId);
            var viewModel = await _searchPresenter.HandleIndex(apiResult);
            return View(viewModel.ViewName, viewModel);
        }

        /// <summary>
        /// gets the results of the search from the search index page. Then sets the search details in the session.
        /// </summary>
        /// <returns>Page with search results</returns>
        [HttpPost("search/results")]
        public async Task<ActionResult> ResultsFromSearchIndex()
        {
            // read form
            var queries = Request.Form
                .Where(kvp => kvp.Key.StartsWith("queries-"))
                .ToDictionary(
                    kvp => kvp.Key.Substring("queries-".Length),
                    kvp => kvp.Value.ToString()
                );

            var searchDetail = new SearchDetail()
            {
                PageNumber = Convert.ToInt32(Request.Form["pageNumber"]),
                Queries = queries,
                ItemsPerPage = ItemsPerPage
            };

            HttpContext.Session.SetString(SessionSearchKey, JsonConvert.SerializeObject(searchDetail));

            var apiResult = await _searchAPIService.SearchAsync(new GetDataRequestModel()
            {
                SearchDataTypeId = Request.Form["searchTypeId"],
                SearchDetail = searchDetail
            });
            
            var viewModel = await _searchPresenter.HandleResults(searchDetail, apiResult);
            return View(viewModel.ViewName, viewModel);
        }

        [HttpGet("search/{searchIndexTypeId}/results/{page?}")]
        public async Task<ActionResult> ResultsFromPagination(string searchIndexTypeId, int page = 1)
        {
            var searchDetail = JsonConvert.DeserializeObject<SearchDetail>(HttpContext.Session.GetString(SessionSearchKey));
            searchDetail.PageNumber = page;

            var apiResult = await _searchAPIService.SearchAsync(new GetDataRequestModel()
            {
                SearchDataTypeId = searchIndexTypeId,
                SearchDetail = searchDetail
            });

            var viewModel = await _searchPresenter.HandleResults(searchDetail, apiResult);
            return View(viewModel.ViewName, viewModel);
        }
    }
}