namespace FUNewsManagement_RazorPages.Models.Dto
{
    public class NewArticleDto
    {
        public NewsArticle NewsArticle { get; set; }
        public List<int>? TagIds { get; set; }
    }

}
