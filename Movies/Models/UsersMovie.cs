using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class UsersMovie
{
    public int IdUserMovie { get; set; }

    public int IdUser { get; set; }

    public int IdMovie { get; set; }

    public decimal? Rating { get; set; }

    public string? Status { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Movie IdMovieNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
