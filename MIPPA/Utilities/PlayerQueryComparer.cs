using Mippa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIPPA.Utilities
{
    public class PlayerQueryComparer : IEqualityComparer<PlayerQueryViewModel>
    {
        public bool Equals(PlayerQueryViewModel x, PlayerQueryViewModel y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.PlayerId == y.PlayerId;
        }

        public int GetHashCode(PlayerQueryViewModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
