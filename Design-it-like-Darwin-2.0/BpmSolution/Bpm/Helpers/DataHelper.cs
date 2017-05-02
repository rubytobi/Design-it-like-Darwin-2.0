using System;
using System.Collections.Generic;
using System.Linq;
using Bpm.NotationElements;
using BpmApi.Models;

namespace Bpm.Helpers
{
    public class DataHelper
    {
        /// <summary>
        ///     Provides data, describing the neccessary input for every activity.
        ///     Only objects used as input by the activity is saved.
        ///     Default case is: FALSE
        /// </summary>
        public class ActivityInputHelper
        {
            private static readonly ActivityInputHelper _instance = new ActivityInputHelper();
            public HashSet<InputModel> Models { get; } = new HashSet<InputModel>();

            public HashSet<BpmnObject> RequiredInputObjects(BpmnActivity activity)
            {
                // select all input object names for activity
                var y = Models.Where(x => x.activityName.Equals(activity.Name))
                    .Select(x => x.objectName)
                    .ToList();

                // lookup all BpmnObjects
                return new HashSet<BpmnObject>(ObjectHelper.Instance()
                    .GetAllObjects()
                    .Where(x => y.Contains(x.Name))
                    .ToList());
            }

            public static ActivityInputHelper Instance()
            {
                return _instance;
            }
        }

        public class ActivityHelper
        {
            private static readonly ActivityHelper _instance = new ActivityHelper();
            public HashSet<ActivityModel> Models { get; } = new HashSet<ActivityModel>();

            public double GetCashflow(string name)
            {
                return Models.FirstOrDefault(x => x.name.Equals(name)).cashflow;
            }

            public double GetVariance(string name)
            {
                return Models.FirstOrDefault(x => x.name.Equals(name)).variance;
            }

            public double GetTime(string name)
            {
                return Models.FirstOrDefault(x => x.name.Equals(name)).duration;
            }

            public List<BpmnActivity> GetAll()
            {
                var list = new List<BpmnActivity>();
                list.AddRange(Models.Select(x => new BpmnActivity(-1, null, x.name)));
                return list;
            }

            public static ActivityHelper Instance()
            {
                return _instance;
            }

        }

        /// <summary>
        ///     Provides data, describing the provided output for every activity.
        ///     Only objects generated as output by the activity is saved.
        ///     Default case is: FALSE
        /// </summary>
        public class ActivityOutputHelper
        {
            private static readonly ActivityOutputHelper _instance = new ActivityOutputHelper();
            public HashSet<OutputModel> Models { get; } = new HashSet<OutputModel>();

            public static ActivityOutputHelper Instance()
            {
                return _instance;
            }

            public HashSet<BpmnObject> ProvidedOutputObjects(BpmnActivity activity)
            {
                // select all input object names for activity
                var y = Models.Where(x => x.activityName.Equals(activity.Name))
                    .Select(x => x.objectName)
                    .ToList();

                // lookup all BpmnObjects
                return new HashSet<BpmnObject>(ObjectHelper.Instance()
                    .GetAllObjects()
                    .Where(x => y.Contains(x.Name))
                    .ToList());
            }
        }

        public class ObjectHelper
        {
            private static readonly ObjectHelper _instance = new ObjectHelper();
            public HashSet<ObjectModel> Models { get; } = new HashSet<ObjectModel>();

            public HashSet<BpmnObject> GetProcessInput()
            {
                return new HashSet<BpmnObject>(GetAllObjects().Where(x => x.ProcessInput));
            }

            public HashSet<BpmnObject> GetProcessOutput()
            {
                return new HashSet<BpmnObject>(GetAllObjects().Where(x => x.ProcessOutput));
            }

            public double GetOutputPrice(string name)
            {
                return GetAllObjects().FirstOrDefault(x => x.Name.Equals(name)).Price;
            }

            public static ObjectHelper Instance()
            {
                return _instance;
            }

            public HashSet<BpmnObject> GetAllObjects()
            {
                var objects = new HashSet<BpmnObject>();

                foreach (var x in Models)
                    objects.Add(x.ToBpmnObject());

                return objects;
            }
        }

        public class ActivityAttributeHelper
        {
            private static readonly ActivityAttributeHelper _instance = new ActivityAttributeHelper();
            public HashSet<ActivityAttributeModel> Models { get; } = new HashSet<ActivityAttributeModel>();

            public List<string> GetDecisionValues(string decisionId)
            {
                return Models.Where(x => x.decisionId.Equals(decisionId)).Select(x => x.decisionValue).ToList();
            }

            public double GetDecisionProbability(string decisionId, string decisionValue)
            {
                return Models.FirstOrDefault(x => x.decisionId.Equals(decisionId) && x.decisionValue.Equals(decisionValue))
                    .decisionProbability;
            }

            public HashSet<BpmnProcessAttribute> GetAll()
            {
                var hashset = new HashSet<BpmnProcessAttribute>();

                foreach (var x in Models)
                    hashset.Add(x.ToBpmnProcessAttribute());

                return hashset;
            }

            public static ActivityAttributeHelper Instance()
            {
                return _instance;
            }
        }

        /// <summary>
        ///     Provides data, describing the coverage by an activity according to the attribute.
        ///     Only not covered attributes and activities are saved.
        ///     Default case is: TRUE
        /// </summary>
        public class CoverHelper
        {
            private static readonly CoverHelper _instance = new CoverHelper();
            public HashSet<CoverModel> Models { get; } = new HashSet<CoverModel>();

            public bool CheckIfActivityCoversAttribute(string decisionId, string attribute, string name)
            {
                var list = Models.Select(x => x.isCovered == false
                                              && x.decisionId.Equals(decisionId)
                                              && x.decisionValue.Equals(x.decisionValue)
                                              && x.activityName.Equals(name));

                if (list.Any())
                    return true;
                return false;
            }

            public HashSet<BpmnProcessAttribute> CoveredAttributes(string activityName)
            {
                // all attributes
                var allAttributes = ActivityAttributeHelper.Instance().GetAll();
                var notCovered = Models.Where(x => x.activityName.Equals(activityName)).ToList();

                foreach (var item in notCovered)
                    allAttributes.RemoveWhere(x => x.DecisionId.Equals(item.decisionId) &&
                                                   x.DecisionValue.Equals(item.decisionValue));

                var set = new HashSet<BpmnProcessAttribute>();
                foreach (var item in allAttributes)
                    set.Add(item);

                // lookup all BpmnObjects
                return set;
            }

            public static CoverHelper Instance()
            {
                return _instance;
            }
        }
    }
}