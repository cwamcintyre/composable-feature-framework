using System;
using Component.Search.UI.Service.Model;

namespace Component.Search.UI.Helpers;

public static class SearchResultsHelper
{
    public const string TotalKey = "{total}";
    
    public static string EnrichSearchDescription(GetDataResponseModel responseModel)
    {
        var description = responseModel.SearchType.SearchPage.Description;

        if (description.Contains(TotalKey))
        {
            description = description.Replace(TotalKey, responseModel.TotalItems.ToString());
        }

        return description;
    }

    public static string EnrichSearchDetailsTitle(GetDataItemResponseModel responseModel) 
    {
        var title = responseModel.DetailTypeModel.DetailsPage.Title;

        // COPILOT: placeholders in the title start with { and end with }. Find any placeholders in the title and replace them with the data found in responseModel
        var placeholders = System.Text.RegularExpressions.Regex.Matches(title, @"\{(.*?)\}");
        foreach (System.Text.RegularExpressions.Match placeholder in placeholders)
        {
            var key = placeholder.Groups[1].Value;
            var value = responseModel.Data.ContainsKey(key) ? responseModel.Data[key] : string.Empty;
            title = title.Replace(placeholder.Value, value);
        }

        return title;
    }
}
