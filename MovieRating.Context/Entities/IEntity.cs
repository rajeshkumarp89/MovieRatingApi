using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Context.Entities
{
    public interface IEntity
    {
        bool IsActive { get; set; }
        DateTime DateCreated { get; set; }
    }
}
