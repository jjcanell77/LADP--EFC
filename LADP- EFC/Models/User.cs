using System;
using System.Collections.Generic;

namespace LADP__EFC.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
