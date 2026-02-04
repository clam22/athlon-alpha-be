using System;
using System.Collections.Generic;
using System.Text;

namespace athlon_alpha_be.database.Models;
public class User : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}

