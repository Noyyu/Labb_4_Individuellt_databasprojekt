using System;
using System.Collections.Generic;

namespace Labb_3__Anropa_databasen.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TitleId { get; set; }

    public string LastName { get; set; } = null!;

    public int YearsOfService { get; set; }

    public decimal Salary { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual StaffTitle Title { get; set; } = null!;
}
