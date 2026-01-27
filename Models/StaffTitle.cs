using System;
using System.Collections.Generic;

namespace SQLScaffolding.Models;

public partial class StaffTitle
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
