using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class Permission
{
    public int IdPermission { get; set; }

    public string PermissionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = new List<RolesPermission>();
}
