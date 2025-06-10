namespace BlastMine.Models;

public class LoginResponse
{
    public required string Token { get; set; }
    public required User User { get; set; }
    public DateTime ExpiresAt { get; set; }
}
