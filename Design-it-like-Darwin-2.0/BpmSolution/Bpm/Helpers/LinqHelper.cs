using System.Collections.Generic;

namespace Bpm.Helpers
{
    public static class LinqHelper
    {
        public static List<double> GetLast(this List<double> list, int id)
        {
            if (id >= list.Count)
                return new List<double>();
            return list.GetRange(id, list.Count - id);
        }
    }
}