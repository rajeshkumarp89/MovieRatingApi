using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public class BaseEntity : IEntity
    {
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
