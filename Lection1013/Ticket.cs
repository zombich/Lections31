﻿using System;
using System.Collections.Generic;

namespace Lection1013;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int SessionId { get; set; }

    public int VisitorId { get; set; }

    public byte Row { get; set; }

    public byte Seat { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual Visitor Visitor { get; set; } = null!;
}
