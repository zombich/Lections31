using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lection1013.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    [StringLength(100)]
    public string? Description { get; set; }
}
