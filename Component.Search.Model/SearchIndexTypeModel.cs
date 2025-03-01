namespace Component.Search.Model
{
    public class SearchIndexTypeModel
    {
        public SearchIndexPage SearchIndexPage { get; set; }
    }

    public class SearchIndexPage
    {
        public string Title { get; set; } = string.Empty;
        public List<IndexField> IndexFields { get; set; } = new List<IndexField>();
    }

    public class IndexField
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
    }
}