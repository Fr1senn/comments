using commentsAPI.Entities.Shared;
using System;
using System.Collections.Generic;

namespace commentsAPI.Entities.Models;

public partial class Comment : BaseEntity
{
    public int UserId { get; set; }

    public Guid PublicId { get; set; }

    public int? ParentCommentId { get; set; }

    public string CommentText { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
