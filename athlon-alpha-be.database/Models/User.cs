namespace athlon_alpha_be.database.Models;

public record User : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CognitoSub { get; set; } = string.Empty;

}

