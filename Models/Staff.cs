using System;
using System.Collections.Generic;

namespace SQLScaffolding.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? TitleId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual StaffTitle? Title { get; set; }
}
