using System;
using System.Collections.Generic;

namespace Lection1013;

public partial class VisitorEmail
{
    public int VisitorId { get; set; }

    public string? Email { get; set; }

    public DateTime ChangingDate { get; set; }

    public virtual Visitor Visitor { get; set; } = null!;
}
