using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class Genre
{
    public int IdGenre { get; set; }

    public string GenreName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
