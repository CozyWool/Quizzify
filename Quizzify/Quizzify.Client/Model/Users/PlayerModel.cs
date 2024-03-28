namespace Quizzify.Client.Model.Users;

public class PlayerModel
{
    public int PlayerId { get; set; }
    public Guid UserId { get; set; }
    public string Nickname { get; set; }
    public byte[] UserProfilePicture { get; set; }
    public string About { get; set; }
}