using System;
using System.Collections.Generic;

namespace Labb_3__Anropa_databasen.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
