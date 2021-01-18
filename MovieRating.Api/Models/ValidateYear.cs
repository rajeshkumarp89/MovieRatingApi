using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.Api.Models
{
    public class ValidateYear : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }
            int year = Convert.ToInt32(value);
            if (year >= 1950 && year <= DateTime.Now.Year)
            {
                return true;
            }
            return false;
        }
    }
}
