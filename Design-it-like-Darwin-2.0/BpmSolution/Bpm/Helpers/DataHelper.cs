using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bpm.NotationElements;
using BpmApi.Models;
using System.Collections;

namespace Bpm.Helpers
{
    public class DataHelper
    {
        public static void LoadDummyData()
        {
            #region ActivityHelper
            ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a1",
                    description = "",
                    cashflow = -1,
                    variance = 0.3,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a2",
                    description = "",
                    cashflow = -7,
                    variance = 14.82,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a3",
                    description = "",
                    cashflow = -4,
                    variance = 4.84,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a4",
                    description = "",
                    cashflow = -23,
                    variance = 160.02,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a5",
                    description = "",
                    cashflow = -29,
                    variance = 254.4,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a6",
                    description = "",
                    cashflow = -20,
                    variance = 121,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a7",
                    description = "",
                    cashflow = -4,
                    variance = 4.84,
                    time = 1
                });
            DataHelper.ActivityHelper.Instance().Add(
                new ActivityModel()
                {
                    id = Guid.NewGuid(),
                    name = "a8",
                    description = "",
                    cashflow = -25,
                    variance = 189.06,
                    time = 1
                });
            #endregion

            #region ObjectHelper
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o1",
                    description = "",
                    type = "Information",
                    processInput = true,
                    processOutput = false,
                    price = 0.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o2",
                    description = "",
                    type = "Information",
                    processInput = true,
                    processOutput = false,
                    price = 0.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o3",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = false,
                    price = 0.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o4",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = false,
                    price = 0.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o5",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = true,
                    price = 20.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o6",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = true,
                    price = 25.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o7",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = true,
                    price = 20.0
                });
            DataHelper.ObjectHelper.Instance().Add(
                new ObjectModel()
                {
                    id = Guid.NewGuid(),
                    name = "o8",
                    description = "",
                    type = "Information",
                    processInput = false,
                    processOutput = true,
                    price = 25.0
                });
            #endregion

            #region ActivityInputHelper
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a1",
                objectName = "o1",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a1",
                objectName = "o2",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a2",
                objectName = "o1",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a2",
                objectName = "o2",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a3",
                objectName = "o1",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a3",
                objectName = "o2",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a4",
                objectName = "o1",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a4",
                objectName = "o2",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a5",
                objectName = "o3",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a6",
                objectName = "o3",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a7",
                objectName = "o4",
                isInput = true
            });
            DataHelper.ActivityInputHelper.Instance().Add(new ActivityInputModel()
            {
                activityName = "a8",
                objectName = "o4",
                isInput = true
            });
            #endregion

            #region ActivityOutputHelper
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a1",
                objectName = "o3",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a2",
                objectName = "o3",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a3",
                objectName = "o4",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a4",
                objectName = "o4",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a5",
                objectName = "o5",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a5",
                objectName = "o6",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a6",
                objectName = "o5",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a6",
                objectName = "o6",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a7",
                objectName = "o7",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a7",
                objectName = "o8",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a8",
                objectName = "o7",
                isOutput = true,
            });
            DataHelper.ActivityOutputHelper.Instance().Add(new ActivityOutputModel()
            {
                activityName = "a8",
                objectName = "o8",
                isOutput = true,
            });
            #endregion

            #region ActivityAttributeHelper
            ActivityAttributeHelper.Instance().Add(new ActivityAttributeModel()
            {
                decisionId = "1",
                decisionValue = "1",
                decisionProbability = 0.025
            });
            DataHelper.ActivityAttributeHelper.Instance().Add(new ActivityAttributeModel()
            {
                decisionId = "1",
                decisionValue = "2",
                decisionProbability = 0.175
            });
            DataHelper.ActivityAttributeHelper.Instance().Add(new ActivityAttributeModel()
            {
                decisionId = "1",
                decisionValue = "3",
                decisionProbability = 0.5
            });
            DataHelper.ActivityAttributeHelper.Instance().Add(new ActivityAttributeModel()
            {
                decisionId = "1",
                decisionValue = "4",
                decisionProbability = 0.25
            });
            DataHelper.ActivityAttributeHelper.Instance().Add(new ActivityAttributeModel()
            {
                decisionId = "1",
                decisionValue = "5",
                decisionProbability = 0.05
            });
            #endregion

            #region CoverHelper
            DataHelper.CoverHelper.Instance().Add(new CoverModel()
            {
                activityName = "a7",
                decisionId = "1",
                decisionValue = "5",
                isCovered = false
            });
            DataHelper.CoverHelper.Instance().Add(new CoverModel()
            {
                activityName = "a8",
                decisionId = "1",
                decisionValue = "1",
                isCovered = false
            });
            DataHelper.CoverHelper.Instance().Add(new CoverModel()
            {
                activityName = "a8",
                decisionId = "1",
                decisionValue = "2",
                isCovered = false
            });
            DataHelper.CoverHelper.Instance().Add(new CoverModel()
            {
                activityName = "a8",
                decisionId = "1",
                decisionValue = "3",
                isCovered = false
            });
            DataHelper.CoverHelper.Instance().Add(new CoverModel()
            {
                activityName = "a8",
                decisionId = "1",
                decisionValue = "4",
                isCovered = false
            });
            #endregion
        }

        /// <summary>
        /// Provides data, describing the neccessary input for every activity.
        /// Only objects used as input by the activity is saved. 
        /// Default case is: FALSE
        /// </summary>
        public class ActivityInputHelper
        {
            private static ActivityInputHelper _instance = new ActivityInputHelper();
            public List<ActivityInputModel> Models { get; } = new List<ActivityInputModel>();

            public void Add(ActivityInputModel a)
            {
                Models.Add(a);
            }

            public HashSet<BpmnObject> RequiredInputObjects(BpmnActivity activity)
            {
                // select all input object names for activity
                var y = Models.Where(x => x.isInput && x.activityName.Equals(activity.Name)).Select(x => x.objectName).ToList();

                // lookup all BpmnObjects
                return new HashSet<BpmnObject>(ObjectHelper.Instance().GetAllObjects().Where(x => y.Contains(x.Name)).ToList());
            }

            public static ActivityInputHelper Instance()
            {
                return _instance;
            }

            public void Clear()
            {
                Models.Clear();
            }

            public IEnumerable<ActivityInputModel> GetAll()
            {
                return Models;
            }

            public ActivityInputModel[] GetAllModels()
            {
                return Models.ToArray();
                // TODO
            }
        }

        public class ActivityHelper
        {
            private static ActivityHelper _instance = new ActivityHelper();
            public List<ActivityModel> Models { get; } = new List<ActivityModel>();

            public void Clear()
            {
                Models.Clear();
            }

            public double GetCashflow(string name)
            {
                return Models.Where(x => x.name.Equals(name)).FirstOrDefault().cashflow;
            }

            public double GetVariance(string name)
            {
                return Models.Where(x => x.name.Equals(name)).FirstOrDefault().variance;
            }

            public HashSet<ActivityModel> GetAllModels()
            {
                return new HashSet<ActivityModel>(Models);
            }

            public List<BpmnActivity> GetAll()
            {
                List<BpmnActivity> list = new List<BpmnActivity>();
                list.AddRange(Models.Select(x => new BpmnActivity(-1, null, x.name)));
                return list;
            }

            public static ActivityHelper Instance()
            {
                return _instance;
            }

            public void Add(ActivityModel x)
            {
                Models.Add(x);
            }

            public double GetTime(string name)
            {
                return Models.Where(x => x.name.Equals(name)).FirstOrDefault().time;
            }
        }

        /// <summary>
        /// Provides data, describing the provided output for every activity.
        /// Only objects generated as output by the activity is saved. 
        /// Default case is: FALSE
        /// </summary>
        public class ActivityOutputHelper
        {
            private static ActivityOutputHelper _instance = new ActivityOutputHelper();
            public List<ActivityOutputModel> Models { get; } = new List<ActivityOutputModel>();

            public static ActivityOutputHelper Instance()
            {
                return _instance;
            }

            public void Add(ActivityOutputModel a)
            {
                Models.Add(a);
            }

            public HashSet<BpmnObject> ProvidedOutputObjects(BpmnActivity activity)
            {
                // select all input object names for activity
                var y = Models.Where(x => x.isOutput && x.activityName.Equals(activity.Name)).Select(x => x.objectName).ToList();

                // lookup all BpmnObjects
                return new HashSet<BpmnObject>(ObjectHelper.Instance().GetAllObjects().Where(x => y.Contains(x.Name)).ToList());
            }

            public void Clear()
            {
                Models.Clear();
            }

            public ActivityOutputModel[] GetAllModels()
            {
                return Models.ToArray();
            }
        }

        public class ObjectHelper
        {
            private static ObjectHelper _instance = new ObjectHelper();
            public List<ObjectModel> Models { get; } = new List<ObjectModel>();

            public void Add(ObjectModel o)
            {
                Models.Add(o);
            }

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
                return GetAllObjects().Where(x => x.Name.Equals(name)).FirstOrDefault().Price;
            }

            public static ObjectHelper Instance()
            {
                return _instance;
            }

            public HashSet<ObjectModel> GetAllModels()
            {
                return new HashSet<ObjectModel>(Models);
            }

            public HashSet<BpmnObject> GetAllObjects()
            {
                HashSet<BpmnObject> objects = new HashSet<BpmnObject>();

                foreach (var x in Models)
                {
                    objects.Add(x.ToBpmnObject());
                }

                return objects;
            }

            public void Clear()
            {
                Models.Clear();
            }
        }

        public class ActivityAttributeHelper
        {
            private static ActivityAttributeHelper _instance = new ActivityAttributeHelper();
            public List<ActivityAttributeModel> Models { get; } = new List<ActivityAttributeModel>();

            public void Add(ActivityAttributeModel b)
            {
                Models.Add(b);
            }

            public List<string> GetDecisionValues(string decisionId)
            {
                return Models.Where(x => x.decisionId.Equals(decisionId)).Select(x => x.decisionValue).ToList();
            }

            public double GetDecisionProbability(string decisionId, string decisionValue)
            {
                return Models.Where(x => x.decisionId.Equals(decisionId) && x.decisionValue.Equals(decisionValue)).FirstOrDefault().decisionProbability;
            }

            public HashSet<BpmnProcessAttribute> GetAll()
            {
                HashSet<BpmnProcessAttribute> hashset = new HashSet<BpmnProcessAttribute>();

                foreach (var x in Models)
                {
                    hashset.Add(x.ToBpmnProcessAttribute());
                }

                return hashset;
            }

            public static ActivityAttributeHelper Instance()
            {
                return _instance;
            }

            public void Clear()
            {
                Models.Clear();
            }

            public ActivityAttributeModel[] GetAllModels()
            {
                return Models.ToArray();
            }
        }

        /// <summary>
        /// Provides data, describing the coverage by an activity according to the attribute.
        /// Only not covered attributes and activities are saved. 
        /// Default case is: TRUE
        /// </summary>
        public class CoverHelper
        {
            private static CoverHelper _instance = new CoverHelper();
            public List<CoverModel> Models { get; } = new List<CoverModel>();

            public void Add(CoverModel p)
            {
                Models.Add(p);
            }

            public bool CheckIfActivityCoversAttribute(string decisionId, string attribute, string name)
            {
                var list = Models.Select(x => x.isCovered == false
                        && x.decisionId.Equals(decisionId)
                        && x.decisionValue.Equals(x.decisionValue)
                        && x.activityName.Equals(name));

                if (list.ToList().Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public HashSet<BpmnProcessAttribute> CoveredAttributes(string activityName)
            {
                // all attributes
                var allAttributes = ActivityAttributeHelper.Instance().GetAll();
                var notCovered = Models.Where(x => x.activityName.Equals(activityName)).ToList();

                foreach (var item in notCovered)
                {
                    allAttributes.RemoveWhere(x => x.DecisionId.Equals(item.decisionId) && x.DecisionValue.Equals(item.decisionValue));
                }

                HashSet<BpmnProcessAttribute> set = new HashSet<BpmnProcessAttribute>();
                foreach (var item in allAttributes)
                {
                    set.Add(item);
                }

                // lookup all BpmnObjects
                return set;
            }

            public static CoverHelper Instance()
            {
                return _instance;
            }

            public void Clear()
            {
                Models.Clear();
            }

            public CoverModel[] GetAllModels()
            {
                return Models.ToArray();
            }
        }
    }
}