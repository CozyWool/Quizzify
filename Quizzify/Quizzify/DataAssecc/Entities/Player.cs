using System;
using System.Collections.Generic;

namespace Quizzify.DataAssecc.Entities;

public class Player
{
    public int PlayerId { get; set; }

    public Guid UserId { get; set; }

    public string Nickname { get; set; }

    public byte[] UserProfilePicture { get; set; }

    public string About { get; set; }

    public virtual User User { get; set; }
}
