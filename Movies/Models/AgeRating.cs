using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class AgeRating
{
    public int IdAgeRating { get; set; }

    public string RatingName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
