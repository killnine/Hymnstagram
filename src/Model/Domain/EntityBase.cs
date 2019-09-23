using System;

namespace Hymnstagram.Model.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public bool IsDestroyed { get; private set; }

        public bool IsNew
        {
            get { return Guid.Empty == Id; }
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }
    }
}
