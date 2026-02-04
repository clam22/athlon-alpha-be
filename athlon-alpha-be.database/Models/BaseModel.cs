using System;
using System.Collections.Generic;
using System.Text;

namespace athlon_alpha_be.database.Models;

public abstract class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;
    public void UpdateLastModified()
    {
        LastModified = DateTimeOffset.Now;
    }
}
