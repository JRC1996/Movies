using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class UsersRole
{
    public int IdUsersRoles { get; set; }

    public int IdUser { get; set; }

    public int IdRole { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
