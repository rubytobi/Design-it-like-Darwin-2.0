using Bpm.NotationElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortableGeneticAlgorithm.Interfaces;
using Bpm.Helpers;
using System.Diagnostics;

namespace BpmApi.Models
{
    public class SettingsBpmModel
    {
        public int TimeWeight { get; set; }
        public int CostWeight { get; set; }
        public double Alpha { get; set; }
        public double I { get; set; }
        public double Ifix { get; set; }
        public double Ivar { get; set; }
        public int MaxDepthRandomGenome { get; set; }
        public int N { get; set; }
        public bool OnlyValidSolutions { get; set; }
        public bool OnlyValidSolutionsAtStart { get; set; }
        public double PainFactor { get; set; }
        public int PopulationMultiplicator { get; set; }
        public int T { get; set; }
    }
}