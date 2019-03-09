using System.Linq;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Get total credit value from a Movement collection (without credit cards)
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static double TotalCredit (this IEnumerable<Movement> collection)
        {
            return collection.Where(x => x.Type.Equals("C") && x.MovementStatus.Equals(MovementStatus.Launched) && x.Invoice == null).Sum(x => x.TotalValue);
        }

        /// <summary>
        /// Get total debit value from a Movement collection (without credit cards)
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static double TotalDebit(this IEnumerable<Movement> collection)
        {
            return collection.Where(x => x.Type.Equals("D") && x.MovementStatus.Equals(MovementStatus.Launched) && x.Invoice == null).Sum(x => x.TotalValue);
        }
    }
}