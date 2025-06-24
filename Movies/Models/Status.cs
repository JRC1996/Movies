using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class Status
{
    public int IdStatus { get; set; }

    public string StatusName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly CreationDate { get; set; }

    public virtual ICollection<UsersMovie> UsersMovies { get; set; } = new List<UsersMovie>();
}
