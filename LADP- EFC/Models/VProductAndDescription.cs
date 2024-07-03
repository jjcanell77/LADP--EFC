using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class VProductAndDescription
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string ProductModel { get; set; } = null!;

    public string Culture { get; set; } = null!;

    public string Description { get; set; } = null!;
}
