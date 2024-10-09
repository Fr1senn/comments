using commentsAPI.Entities.Shared;
using System;
using System.Collections.Generic;

namespace commentsAPI.Entities.Models;

public partial class User : BaseEntity
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? HomePage { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
