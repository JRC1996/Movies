using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual UsersMovie? UsersMovie { get; set; }
}
