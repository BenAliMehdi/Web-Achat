using System;
using System.Collections.Generic;

namespace WebAchat.Models;

public partial class StockMovementItem
{
    public int StockMovementItemId { get; set; }

    public int ProductItemId { get; set; }

    public int StockMovementId { get; set; }

    public virtual ProductItem ProductItem { get; set; } = null!;

    public virtual StockMovement StockMovement { get; set; } = null!;
}
