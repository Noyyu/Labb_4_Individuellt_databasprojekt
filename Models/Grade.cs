using System;
using System.Collections.Generic;

namespace SQLScaffolding.Models;

public partial class Grade
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int TeacherId { get; set; }

    public DateOnly Date { get; set; }

    public string Grade1 { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Staff Teacher { get; set; } = null!;
}
