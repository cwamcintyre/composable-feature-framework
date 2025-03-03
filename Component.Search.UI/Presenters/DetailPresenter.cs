using System;
using Component.Search.Model;
using Component.Search.UI.Helpers;
using Component.Search.UI.Models;
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
        var enrichedTitle = SearchResultsHelper.EnrichSearchDetailsTitle(responseModel);
        
        var pageModel = new DetailPageViewModel
        {
            Title = enrichedTitle,
            Sections = new List<DetailSectionViewModel>(),
        };        

        foreach (var section in responseModel.DetailTypeModel.DetailsPage.Sections)
        {
            var sectionModel = new DetailSectionViewModel
            {
                Title = section.Title,
                Components = new List<DetailComponentViewModel>(),
            };

            foreach (var component in section.Components)
            {
                var componentModel = new DetailComponent
                {
                    Type = component.Type,
                    Content = component.Content,
                    Fields = component.Fields,
                };

                sectionModel.Components.Add(new DetailComponentViewModel()
                {
                    Component = componentModel,
                    Data = responseModel.Data
                });
            }

            pageModel.Sections.Add(sectionModel);
        }

        var viewModel = new DetailViewModel
        {
            ViewName = IndexViewName,
            PageModel = pageModel,
        };

        return viewModel;
    }
}

public class DetailViewModel
{
    public string ViewName { get; set; }
    public DetailPageViewModel PageModel { get; set; }
}
