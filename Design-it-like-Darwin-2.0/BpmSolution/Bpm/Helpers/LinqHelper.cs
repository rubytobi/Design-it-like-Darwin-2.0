using Bpm.NotationElements;
using System.Collections.Generic;
using static Bpm.Helpers.ProcessHelper.PathSplitter;

namespace Bpm.Helpers
{
    public static class LinqHelper
    {
        public static List<double> GetLast(this List<double> list, int id)
        {
            if (id >= list.Count)
            {
                return new List<double>();
            }
            else
            {
                return list.GetRange(id, list.Count - id);
            }
        }
    }
}