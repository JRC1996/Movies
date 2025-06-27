using System;
using System.Collections.Generic;

namespace Movies.Models;

public partial class RefreshToken
{
    public int IdRefreshToken { get; set; }

    public string Token { get; set; } = null!;

    public int IdUser { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? Revoked { get; set; }

    public string CreatedByIp { get; set; } = null!;

    public string RevokedByIp { get; set; } = null!;

    public string? ReplaceByToken { get; set; }

    public string? ReasonRevoked { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
