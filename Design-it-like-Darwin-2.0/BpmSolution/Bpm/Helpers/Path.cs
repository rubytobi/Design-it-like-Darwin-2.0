using System;
using System.Collections.Generic;
using System.Linq;
using Bpm.NotationElements;

namespace Bpm.Helpers
{
    public class Path
    {
        /// <summary>
        ///     paths contained activities
        /// </summary>
        public List<BpmnActivity> path { get; set; } = new List<BpmnActivity>();

        /// <summary>
        ///     probability of the path
        /// </summary>
        public double Probability { get; set; } = 1;

        /// <summary>
        ///     unique id for the path
        /// </summary>
        public string Id { get; } = Guid.NewGuid().ToString();

        /// <inheritdoc />
        public override string ToString()
        {
            return "[ " + Probability + " | " + string.Join(", ", path.Select(x => x.Name)) + " ]";
        }
    }
}