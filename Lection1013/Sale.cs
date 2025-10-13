using System;
using System.Collections.Generic;

namespace Lection1013;

public partial class Sale
{
    public int SaleId { get; set; }

    public int GameId { get; set; }

    public short KeysAmount { get; set; }

    public DateTime SaleDate { get; set; }

    public virtual Game Game { get; set; } = null!;
}
