using Component.Search.UI.Presenters;
using Component.Search.UI.Service;
using Component.Search.UI.Service.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Component.Search.UI.Controllers
{
    public class DetailController : Controller
    {
        private readonly SearchAPIService _searchAPIService;
        private readonly IDetailPresenter _detailPresenter;
        
        public DetailController(SearchAPIService searchAPIService, IDetailPresenter detailPresenter)
        {
            _searchAPIService = searchAPIService;
            _detailPresenter = detailPresenter;
        }

        [HttpGet("detail/{itemTypeId}/{itemId}")]
        public async Task<ActionResult> Index(string itemTypeId, string itemId)
        {
            var apiResult = await _searchAPIService.GetDetailAsync(new GetDataItemRequestModel()
            {
                ItemTypeId = itemTypeId,
                ItemId = itemId
            });
            var viewModel = await _detailPresenter.HandleIndex(apiResult);
            return View(viewModel.ViewName, viewModel);
        }
    }
}
