using System;
using System.Collections.Generic;

namespace Labb_3__Anropa_databasen.Models;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? TeacherId { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Staff? Teacher { get; set; }
}
