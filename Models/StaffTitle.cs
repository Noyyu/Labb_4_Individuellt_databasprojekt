using System;
using System.Collections.Generic;

namespace Labb_3__Anropa_databasen.Models;

public partial class StaffTitle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
