using System;

namespace Interview.Application.Core.Entitities
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }
    }
}
