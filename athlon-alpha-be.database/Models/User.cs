namespace athlon_alpha_be.database.Models;

public record User : BaseModel
{
    public string CognitoSub { get; set; } = string.Empty;
}

