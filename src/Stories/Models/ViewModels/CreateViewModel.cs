namespace Stories.Models.StoryViewModels
{
    public class CreateViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string DescriptionMarkdown { get; set; }
        public string Url { get; set; }
        public bool IsAuthor { get; set; }
    }
}
