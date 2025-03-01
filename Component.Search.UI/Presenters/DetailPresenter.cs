using System;
using Component.Search.Model;
using Component.Search.UI.Service.Model;

namespace Component.Search.UI.Presenters;

public interface IDetailPresenter
{
    Task<DetailViewModel> HandleIndex(GetDataItemResponseModel responseModel);
}

public class DetailPresenter : IDetailPresenter
{
    public virtual string IndexViewName { get; set; } = "Index";

    public async Task<DetailViewModel> HandleIndex(GetDataItemResponseModel responseModel)
    {
        var viewModel = new DetailViewModel
        {
            ViewName = IndexViewName,
            DetailTypeModel = responseModel.DetailTypeModel,
            Data = responseModel.Data
        };

        return viewModel;
    }
}

public class DetailViewModel
{
    public string ViewName { get; set; }
    public DetailTypeModel DetailTypeModel { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
