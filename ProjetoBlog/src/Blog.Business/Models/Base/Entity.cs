﻿namespace Blog.Business.Models.Base
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

    }
}
