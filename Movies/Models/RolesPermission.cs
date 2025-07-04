using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class RolesPermission
{
    public int IdRolesPermissions { get; set; }

    public int IdRole { get; set; }

    public int IdPermission { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual Permission IdPermissionNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
