using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Tag : IEntity<int>
{
    public int TagId { get; set; }
    int IEntity<int>.Id => TagId;
    public string TagName { get; set; } = null!;

    public string Note { get; set; } = null!;

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
