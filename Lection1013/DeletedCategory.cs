using System;
using System.Collections.Generic;

namespace Lection1013;

public partial class DeletedCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DeletedDate { get; set; }

    public string Login { get; set; } = null!;
}
