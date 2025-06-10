namespace BlastMine.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string HashedPassword { get; set; }
    public required string Email { get; init; }
    public required Role Role { get; set; } = Role.User;
}
