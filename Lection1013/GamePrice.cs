using System;
using System.Collections.Generic;

namespace Lection1013;

public partial class GamePrice
{
    public int GameId { get; set; }

    public decimal OldPrice { get; set; }

    public DateTime ChangingDate { get; set; }

    public virtual Game Game { get; set; } = null!;
}
