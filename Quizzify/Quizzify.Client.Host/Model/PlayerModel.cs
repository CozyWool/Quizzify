namespace Quizzify.Client.Host.Model;

public class PlayerModel
{
    public int PlayerId { get; set; }

    public Guid UserId { get; set; }

    public string Nickname { get; set; }

    public byte[] UserProfilePicture { get; set; }

    public string About { get; set; }
    public bool IsAnswering { get; set; }
    public string ConnectionId { get; set; }
    public int Points { get; set; }
}