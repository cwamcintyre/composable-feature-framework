using System.Collections.Generic;

namespace Component.Form.Model
{
    public class FormModel
    {
        public string FormId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string StartPage { get; set; }
        public List<Page> Pages { get; set; }
        public Submission Submission { get; set; }
    }
}
