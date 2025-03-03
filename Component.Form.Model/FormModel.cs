namespace Component.Form.Model
{
    public class FormModel
    {
        public required string FormId { get; set; }
        public required string StartPage { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int CurrentPage { get; set; }
        public required int TotalPages { get; set; }
        public required List<Page> Pages { get; set; }
        public required Submission Submission { get; set; }
    } 
}
