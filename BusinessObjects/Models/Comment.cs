using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Comment : IEntity<int>
{
    public int CommentId { get; set; }
    int IEntity<int>.Id => CommentId;
    public int NewsArticleId { get; set; }

    public short AccountId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual SystemAccount Account { get; set; } = null!;

    public virtual NewsArticle NewsArticle { get; set; } = null!;
}
