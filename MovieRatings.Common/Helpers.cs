using System;
using System.Collections.Generic;
using System.Text;

namespace MovieRatings.Common
{
    public static class Helpers
    {
        public static double RoundRating(double rating)
        {
            return Math.Round(rating * 2, MidpointRounding.AwayFromZero) / 2;
        }
    }
}
