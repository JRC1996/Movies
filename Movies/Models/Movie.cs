using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class Movie
{
    public int IdMovie { get; set; }

    public string Name { get; set; } = null!;

    public int IdGenre { get; set; }

    public int IdAgeRating { get; set; }

    public string? ImageUrl { get; set; }

    public int? DurationMinutes { get; set; }

    public string? Resume { get; set; }

    public DateOnly? RelaseDate { get; set; }

    public virtual AgeRating IdAgeRatingNavigation { get; set; } = null!;

    public virtual Genre IdGenreNavigation { get; set; } = null!;

    public virtual UsersMovie? UsersMovie { get; set; }
}
