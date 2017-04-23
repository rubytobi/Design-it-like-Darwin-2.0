using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bpm.NotationElements;
using Bpm.NotationElements.Gateways;

namespace Bpm.Helpers
{
    public class ProcessHelper
    {
        public static class PrettyPrint
        {
            /// <summary>
            ///     counts the appearance of same activities in single nodes
            /// </summary>
            /// <param name="gene">gene to check</param>
            /// <param name="count">so far found appearances</param>
            /// <returns></returns>
            public static int CheckForMultipleEqualActivitiesInSameNode(BpmGene gene, int count = 0)
            {
                if (gene is BpmnActivity)
                    return count;

                var list = new List<string>();

                foreach (var child in gene.Children)
                    if (child is BpmnActivity)
                        if (list.Contains(((BpmnActivity) child).Name))
                            count++;
                        else
                            list.Add(((BpmnActivity) child).Name);
                    else
                        count += CheckForMultipleEqualActivitiesInSameNode(child);

                return count;
            }

            /// <summary>
            ///     counts appearances of concatenating same gateways with having multiple children SEQ(SEQ(xxx))
            /// </summary>
            /// <param name="gene"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static int CheckForConcatenatingGatewaysWithSingleChild(BpmGene gene, int count = 0)
            {
                if (gene is BpmnActivity)
                    return count;

                if (gene.Children.Count == 1 && gene.Children[0].GetType() == gene.GetType())
                    return count + 1;

                foreach (var child in gene.Children)
                    count += CheckForConcatenatingGatewaysWithSingleChild(child);

                return count;
            }

            /// <summary>
            ///     counts appearances of empty gateways
            /// </summary>
            /// <param name="gene"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static int CheckForEmptyGateways(BpmGene gene, int count = 0)
            {
                if (gene is BpmnActivity)
                    return count;

                if (gene.Children.Count == 0)
                    return count + 1;

                foreach (var child in gene.Children)
                    count += CheckForEmptyGateways(child);

                return count;
            }

            internal static double Check(BpmGenome genome)
            {
                var sum = 0;

                sum += CheckForConcatenatingGatewaysWithSingleChild(genome.RootGene);
                sum += CheckForEmptyGateways(genome.RootGene);
                sum += CheckForMultipleEqualActivitiesInSameNode(genome.RootGene);
                // TODO
                sum += CheckForSeveralXorsInLine(genome.RootGene, 5);
                sum += CheckForTooMuchActivities(genome.RootGene);

                return sum;
            }


            /// <summary>
            ///     checks for concatenating xors inside each other
            /// </summary>
            /// <param name="gene"></param>
            /// <param name="maxNumberOfXors"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public static int CheckForSeveralXorsInLine(BpmGene gene, int maxNumberOfXors, int count = 0)
            {
                var parent = gene;

                for (var i = 0; i < maxNumberOfXors; i++)
                {
                    if (parent == null)
                        break;

                    if (parent is BpmnXor)
                        parent = parent.Parent;
                    else
                        break;

                    if (i == 2)
                        count++;
                }

                if (parent is BpmnActivity)
                    return count;

                foreach (var child in gene.Children)
                    count += CheckForSeveralXorsInLine(child, maxNumberOfXors);

                return count;
            }


            /// <summary>
            ///     checks for too much equal activities in one gateway
            /// </summary>
            /// <param name="gene"></param>
            /// <returns></returns>
            public static int CheckForTooMuchActivities(BpmGene gene)
            {
                // TODO stimmt nicht
                if (gene is BpmnActivity)
                    return 1;

                var count = 0;

                foreach (var child in gene.Children)
                    count += CheckForTooMuchActivities(child);

                return count;
            }
        }

        public static class ProcessGenerator
        {
            private static BpmnXor GenerateRandomXor(HashSet<BpmnProcessAttribute> necessaryAttributesToCover)
            {
                var allDecisions = DataHelper.ActivityAttributeHelper.Instance().GetAll();
                allDecisions.RemoveWhere(x => !necessaryAttributesToCover.Contains(x));

                if (allDecisions.Count == 0)
                    return null;

                var selected = allDecisions.ElementAt(TreeHelper.RandomGenerator.Next(allDecisions.Count));

                return new BpmnXor(-1, null, selected.DecisionId, selected.DecisionValue);
            }

            private static BpmGene GenerateRandomGateway(HashSet<BpmnProcessAttribute> necessaryAttributesToCover)
            {
                var gateways = new List<BpmGene>
                {
                    new BpmnAnd(-1, null),
                    new BpmnSeq(-1, null)
                };

                if (necessaryAttributesToCover.Count > 0)
                {
                    var xor = GenerateRandomXor(necessaryAttributesToCover);

                    if (xor != null)
                        gateways.Add(xor);
                }

                return gateways.ElementAt(TreeHelper.RandomGenerator.Next(gateways.Count));
            }

            private static BpmnActivity GenerateRandomActivity(HashSet<BpmnObject> availableBpmnObjects,
                HashSet<BpmnProcessAttribute> necessaryAttributesToCover)
            {
                var allActivities = DataHelper.ActivityHelper.Instance().GetAll();
                var allPossibleActivities = new List<BpmnActivity>();
                var allPossiblePositiveActivities = new List<BpmnActivity>();

                foreach (var activity in allActivities)
                {
                    var necessaryInput = DataHelper.ActivityInputHelper.Instance().RequiredInputObjects(activity);
                    var attributesCovered = DataHelper.CoverHelper.Instance().CoveredAttributes(activity.Name);

                    if (necessaryInput.All(availableBpmnObjects.Contains)
                        && necessaryAttributesToCover.All(attributesCovered.Contains))
                    {
                        // check if activity produces new output
                        var output = DataHelper.ActivityOutputHelper.Instance().ProvidedOutputObjects(activity);

                        if (output.Any(x => !availableBpmnObjects.Contains(x)))
                            allPossiblePositiveActivities.Add(activity);

                        allPossibleActivities.Add(activity);
                    }
                }

                if (allPossiblePositiveActivities.Count > 0)
                    return allPossiblePositiveActivities.ElementAt(
                        TreeHelper.RandomGenerator.Next(allPossiblePositiveActivities.Count));

                return allPossibleActivities.ElementAt(TreeHelper.RandomGenerator.Next(allPossibleActivities.Count));
            }

            /// <summary>
            ///     generates a completely random bpm gene
            /// </summary>
            /// <param name="availableBpmnObjects"></param>
            /// <param name="currentAttributesToCover"></param>
            /// <returns></returns>
            public static BpmGene GenerateRandomBpmGene(HashSet<BpmnObject> availableBpmnObjects,
                HashSet<BpmnProcessAttribute> currentAttributesToCover)
            {
                var weightChooseNothing = 0;
                var weightChooseGateway = 50;
                var weightChooseActivity = 100;
                var weightSum = weightChooseActivity + weightChooseGateway + weightChooseNothing;

                var randomInt = TreeHelper.RandomGenerator.Next(weightSum);

                if (randomInt < weightChooseNothing)
                    return null;

                if (randomInt < weightChooseNothing + weightChooseActivity)
                    return GenerateRandomActivity(availableBpmnObjects, currentAttributesToCover);

                return GenerateRandomGateway(currentAttributesToCover);
            }

            /// <summary>
            ///     generates a random valid bpm genome, using the greedy-algo.
            ///     activities creating new objects are prefered over the rest
            /// </summary>
            /// <param name="maxDepth"></param>
            /// <param name="parent"></param>
            /// <param name="availableBpmnObjects"></param>
            /// <param name="currentAttributesToCover"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            public static BpmGene GenerateRandomValidBpmGenome2(int maxDepth, BpmGene parent,
                HashSet<BpmnObject> availableBpmnObjects = null,
                HashSet<BpmnProcessAttribute> currentAttributesToCover = null)
            {
                if (parent == null)
                {
                    availableBpmnObjects = DataHelper.ObjectHelper.Instance().GetProcessInput();
                    currentAttributesToCover = DataHelper.ActivityAttributeHelper.Instance().GetAll();
                }

                if (availableBpmnObjects == null || currentAttributesToCover == null)
                    throw new ArgumentNullException(nameof(availableBpmnObjects) + " or " +
                                                    nameof(currentAttributesToCover));

                var currentDepth = parent.CalculateNodeDepth();

                BpmGene randomGene = null;

                do
                {
                    if (currentDepth == maxDepth)
                        randomGene = GenerateRandomActivity(availableBpmnObjects, currentAttributesToCover);
                    else
                        randomGene = GenerateRandomBpmGene(availableBpmnObjects, currentAttributesToCover);
                } while (parent == null && randomGene == null);

                if (randomGene == null)
                    return null;

                randomGene.Parent = parent;

                if (randomGene is BpmnActivity)
                {
                    var output = DataHelper.ActivityOutputHelper.Instance()
                        .ProvidedOutputObjects((BpmnActivity) randomGene);
                    availableBpmnObjects.UnionWith(output);
                }

                var numberOfChildren = randomGene.GetType().CalculateRandomNumberOfChildren();

                if (randomGene is BpmnXor && numberOfChildren > 0)
                {
                    // randomly xor created
                    var xor = randomGene as BpmnXor;

                    // divide attributes by this ID and Value
                    var selectiveAttribute = new BpmnProcessAttribute(xor.DecisionId, xor.DecisionValue,
                        xor.ExecutionProbability);

                    var attributesIfCase = new HashSet<BpmnProcessAttribute>();
                    var attributesElseCase = new HashSet<BpmnProcessAttribute>();

                    // divide attributes into two buckets for ongoing process
                    foreach (var attribute in currentAttributesToCover)
                        if (selectiveAttribute.DecisionId.Equals(attribute.DecisionId)
                            && !selectiveAttribute.DecisionValue.Equals(attribute.DecisionValue))
                            attributesElseCase.Add(attribute);
                        else
                            attributesIfCase.Add(attribute);

                    // create new sets of bpmnobjects for both cases
                    var availableBpmnObjectsIf = new HashSet<BpmnObject>(availableBpmnObjects);
                    var availableBpmnObjectsElse = new HashSet<BpmnObject>(availableBpmnObjects);

                    // add the first child
                    var randomSubtreeIf = GenerateRandomValidBpmGenome2(maxDepth, randomGene, availableBpmnObjectsIf,
                        attributesIfCase);
                    xor.Children.Add(randomSubtreeIf);

                    if (numberOfChildren > 1)
                    {
                        // add a second child
                        var randomSubtreeElse = GenerateRandomValidBpmGenome2(maxDepth, randomGene,
                            availableBpmnObjectsElse,
                            attributesElseCase);
                        xor.Children.Add(randomSubtreeElse);
                    }

                    // collect the available objects from both paths
                    // TODO possible logic error, if both paths do not create the same output
                    availableBpmnObjects.UnionWith(availableBpmnObjectsIf);
                    availableBpmnObjects.UnionWith(availableBpmnObjectsElse);
                }

                if (randomGene is BpmnSeq || randomGene is BpmnAnd)
                    for (var i = 0; i < numberOfChildren; i++)
                    {
                        var availableBpmnObjectsChild = new HashSet<BpmnObject>(availableBpmnObjects);

                        var createdSubTree = GenerateRandomValidBpmGenome2(maxDepth, randomGene,
                            availableBpmnObjectsChild, currentAttributesToCover);

                        if (createdSubTree == null)
                            Debug.WriteLine("halt");
                        randomGene.Children.Add(createdSubTree);

                        if (randomGene is BpmnSeq)
                            availableBpmnObjects.UnionWith(availableBpmnObjectsChild);
                    }

                if (parent == null)
                    randomGene.RenumberIndicesOfBpmTree(gene => gene.Children);

                return randomGene;
            }

            /// <summary>
            ///     Generates a valid random BpmnGenome.
            /// </summary>
            /// <returns>random tree.</returns>
            public static void GenerateRandomValidBpmGenome(int maxDepth, BpmGene parent, BpmGenome genome,
                double executionProbability = 1)

            {
                if (parent is BpmnXor && parent.Children.Count > 2)
                    Debug.WriteLine("halt stop! BpmnXor error");

                var allDecisions = DataHelper.ActivityAttributeHelper.Instance().GetAll();
                var selected = allDecisions.ElementAt(TreeHelper.RandomGenerator.Next(allDecisions.Count));
                var xor = new BpmnXor(-1, parent, selected.DecisionId, selected.DecisionValue);

                // Choose a random gene from all available
                var gateways = new List<BpmGene>
                {
                    xor,
                    new BpmnAnd(-1, parent),
                    new BpmnSeq(-1, parent)
                };
                var activities = new List<BpmnActivity>
                {
                    new BpmnActivity(-1, parent, "a1"),
                    new BpmnActivity(-1, parent, "a2"),
                    new BpmnActivity(-1, parent, "a3"),
                    new BpmnActivity(-1, parent, "a4"),
                    new BpmnActivity(-1, parent, "a5"),
                    new BpmnActivity(-1, parent, "a6"),
                    new BpmnActivity(-1, parent, "a7"),
                    new BpmnActivity(-1, parent, "a8")
                };
                var randomGene = ChooseRandomBpmnGene(gateways, activities);

                // Create genome if parent is null.
                if (parent == null)
                {
                    genome.RootGene = randomGene;

                    GenerateRandomValidBpmGenome(maxDepth - 1, randomGene, genome);
                }
                // Handle Gateways
                else if ((parent is BpmnAnd || parent is BpmnSeq || parent is BpmnXor) && maxDepth > 1)
                {
                    // Handle the deeper levels in the tree
                    if (parent is BpmnAnd)
                    {
                        // Add the random gene to parents children
                        parent.Children.Add(randomGene);
                        randomGene.Parent = parent;

                        // Choose a random number inclusive lower bound, exclusive upper bound.
                        var numberOfChildren = TreeHelper.RandomGenerator.Next(2, activities.Count);
                        // Recursive call for randomly chosen number.
                        for (var i = numberOfChildren - parent.Children.Count; i > 0; i--)
                            GenerateRandomValidBpmGenome(maxDepth - 1, randomGene, genome);
                    }
                    else if (parent is BpmnSeq)
                    {
                        // Add the random gene to parents children
                        parent.Children.Add(randomGene);
                        randomGene.Parent = parent;

                        // Choose a random number inclusive lower bound, exclusive upper bound.
                        var numberOfChildren = TreeHelper.RandomGenerator.Next(2, activities.Count);
                        // Recursive call for randomly chosen number.
                        for (var i = numberOfChildren - parent.Children.Count; i > 0; i--)
                            GenerateRandomValidBpmGenome(maxDepth - 1, randomGene, genome);
                    }
                    // BPMN-XOR
                    else if (parent is BpmnXor)
                    {
                        // TODO: At the moment only one Decision available.
                        // Get all available decision values.
                        var descitionValues =
                            DataHelper.ActivityAttributeHelper.Instance()
                                .GetDecisionValues(
                                    DataHelper.ActivityAttributeHelper.Instance().GetAll().FirstOrDefault().DecisionId);
                        // Choos a random decision value.
                        var rand = TreeHelper.RandomGenerator.Next(descitionValues.Count);
                        // Get the probability of the randomly chosen value.
                        var randomExecProb =
                            DataHelper.ActivityAttributeHelper.Instance()
                                .GetDecisionProbability(
                                    DataHelper.ActivityAttributeHelper.Instance().GetAll().FirstOrDefault().DecisionId,
                                    descitionValues.ElementAt(rand));

                        if (parent.Children.Count < 2)
                        {
                            // Add the random gene to parents children
                            parent.Children.Add(randomGene);
                            randomGene.Parent = parent;
                        }

                        if (parent.Children.Count < 2)
                            GenerateRandomValidBpmGenome(maxDepth, parent, genome, randomExecProb);

                        GenerateRandomValidBpmGenome(maxDepth, randomGene, genome, randomExecProb);
                    }
                }
                else if ((parent is BpmnAnd || parent is BpmnSeq || parent is BpmnXor) && maxDepth == 1)
                {
                    if (parent is BpmnXor && parent.Children.Count < 2)
                    {
                        // Reached the maximum number of hirarchies. Leafs only can be Actions.
                        var activity = activities[TreeHelper.RandomGenerator.Next(activities.Count - 1)];

                        parent.Children.Add(activity);
                        activity.Parent = parent;
                    }
                }

                if (genome.RootGene.Children != null && maxDepth < 1)
                    genome.RootGene.RenumberIndicesOfBpmTree(gen => gen.Children);
            }

            /// <summary>
            ///     Chooses a random BPMN-Element out of the overgiven options.
            /// </summary>
            /// <param name="bpmnGateways">BPMN-Gateways to choose out of.</param>
            /// <param name="bpmnActivities">BPMN-Actions  to choose out of.</param>
            /// <returns>A random choosen BPMN-Element.</returns>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public static BpmGene ChooseRandomBpmnGene(List<BpmGene> bpmnGateways, List<BpmnActivity> bpmnActivities)
            {
                // TODO !!!find better probaility generation!!!
                var sumOfProbabilities = 3 * bpmnGateways.Count + bpmnActivities.Count;

                var choice = TreeHelper.RandomGenerator.Next(sumOfProbabilities);

                if (choice < bpmnGateways.Count * 3)
                    return bpmnGateways.ElementAt(choice / 3);
                if (choice >= bpmnGateways.Count * 3 && choice < sumOfProbabilities)
                    return bpmnActivities.ElementAt(choice - bpmnGateways.Count * 3);
                throw new ArgumentOutOfRangeException();
            }
        }

        public static class Validator
        {
            /// <summary>
            ///     checks wheter xors have more than 2 children
            /// </summary>
            /// <param name="gene"></param>
            /// <returns></returns>
            public static bool CheckForBpmnXorFailture(BpmGene gene)
            {
                if (gene == null || gene is BpmnActivity)
                    return true;

                if (gene is BpmnAnd || gene is BpmnSeq || gene is BpmnXor && gene.Children.Count <= 2)
                {
                    foreach (var geneForCheck in gene.Children)
                    {
                        var check = CheckForBpmnXorFailture(geneForCheck);

                        if (!check) return false;
                    }

                    return true;
                }
                return false;
            }

            /// <summary>
            ///     Validates an Genome for process input, output and attributes, count failtures on the referenced variable
            /// </summary>
            /// <param name="genome"></param>
            /// <param name="failures"></param>
            /// <returns></returns>
            public static bool ValidateGenome(BpmGenome genome, ref int failures)
            {
                var count = genome.RootGene.CountSpecificNodes(typeof(BpmnXor));
                var numberOfPaths = (int) Math.Pow(2, count);
                failures = 0;
                for (var i = 0; i < numberOfPaths; i++)
                {
                    var startObjects = DataHelper.ObjectHelper.Instance().GetProcessInput();
                    var endObjects = DataHelper.ObjectHelper.Instance().GetProcessOutput();
                    var allAttributes = DataHelper.ActivityAttributeHelper.Instance().GetAll();

                    ValidateGenome(genome.RootGene, ref failures, startObjects, allAttributes, i, numberOfPaths, 0);

                    failures += endObjects.Count(x => !startObjects.Contains(x));
                }


                return failures == 0;
            }

            private static void ValidateGenome(BpmGene gene, ref int failures,
                HashSet<BpmnObject> currentAvailableInput,
                HashSet<BpmnProcessAttribute> currentCoveredAttributes, int pathId, int numberOfPaths, int passedXors)
            {
                if (gene == null)
                    return;

                if (gene is BpmnActivity)
                {
                    var matchingAttributes =
                        DataHelper.CoverHelper.Instance().CoveredAttributes(((BpmnActivity) gene).Name);

                    var tmp = failures;

                    failures += currentCoveredAttributes.Count(x => !matchingAttributes.Contains(x));

                    if (tmp != failures)
                        return;

                    var input = DataHelper.ActivityInputHelper.Instance().RequiredInputObjects((BpmnActivity) gene);

                    failures += input.Count(x => !currentAvailableInput.Contains(x));

                    var output = DataHelper.ActivityOutputHelper.Instance().ProvidedOutputObjects((BpmnActivity) gene);
                    currentAvailableInput.UnionWith(output);

                    return;
                }

                if (gene is BpmnSeq)
                    foreach (var child in gene.Children)
                        ValidateGenome(child, ref failures, currentAvailableInput, currentCoveredAttributes, pathId,
                            numberOfPaths, passedXors);

                if (gene is BpmnAnd)
                {
                    var preAndAvailableInput = new HashSet<BpmnObject>(currentAvailableInput);
                    var postAndAvailableInput = new HashSet<BpmnObject>(currentAvailableInput);

                    foreach (var child in gene.Children)
                    {
                        var cloneCurrentAvailableInput = new HashSet<BpmnObject>(preAndAvailableInput);
                        var cloneCurrentCoveredAttributes = new HashSet<BpmnProcessAttribute>(currentCoveredAttributes);
                        ValidateGenome(child, ref failures, cloneCurrentAvailableInput, cloneCurrentCoveredAttributes,
                            pathId, numberOfPaths, passedXors);
                        postAndAvailableInput.UnionWith(cloneCurrentAvailableInput);
                    }

                    currentAvailableInput.UnionWith(postAndAvailableInput);
                }

                if (gene is BpmnXor)
                {
                    var xor = gene as BpmnXor;

                    // found XOR, follow decision by pathId and passedXors
                    var decisionBase = Convert.ToString(pathId, 2).PadLeft((int) Math.Log(numberOfPaths, 2), '0');
                    var decision = decisionBase[passedXors] - '0'; // dirty trick
                    var attribute = new BpmnProcessAttribute(xor.DecisionId, xor.DecisionValue,
                        DataHelper.ActivityAttributeHelper.Instance()
                            .GetDecisionProbability(xor.DecisionId, xor.DecisionValue));

                    passedXors++;

                    var ifCase = new HashSet<BpmnProcessAttribute>();
                    ifCase.Add(attribute);
                    var elseCase = new HashSet<BpmnProcessAttribute>(currentCoveredAttributes);
                    elseCase.Remove(attribute);

                    if (decision == 0)
                        ValidateGenome(gene.Children.ElementAtOrDefault(decision), ref failures, currentAvailableInput,
                            ifCase, pathId, numberOfPaths, passedXors);

                    if (decision == 1)
                        ValidateGenome(gene.Children.ElementAtOrDefault(decision), ref failures, currentAvailableInput,
                            elseCase, pathId, numberOfPaths, passedXors);
                }
            }
        }

        public class PathSplitter
        {
            private readonly BpmGene _root;
            private readonly List<Path> paths = new List<Path>();

            public PathSplitter(BpmGenome genome)
            {
                _root = genome.RootGene;

                foreach (var bpa in DataHelper.ActivityAttributeHelper.Instance().GetAll())
                {
                    var path = new List<BpmnActivity>();
                    CalculatePath(bpa, _root, path);
                    paths.Add(new Path {Probability = bpa.DecisionProbability, path = path});
                }
            }

            private void CalculatePath(BpmnProcessAttribute bpa, BpmGene gene, List<BpmnActivity> path)
            {
                if (gene == null)
                    return;
                if (gene is BpmnActivity)
                    path.Add(gene as BpmnActivity);
                else if (gene is BpmnSeq || gene is BpmnAnd)
                    foreach (var child in gene.Children)
                        CalculatePath(bpa, child, path);
                else if (gene is BpmnXor)
                    if ((gene as BpmnXor).ToProcessAttribute().Equals(bpa))
                    {
                        if (gene.Children.Count > 0)
                            CalculatePath(bpa, gene.Children[0], path);
                    }
                    else
                    {
                        if (gene.Children.Count > 1)
                            CalculatePath(bpa, gene.Children[1], path);
                    }
            }

            public List<Path> GetPaths()
            {
                return paths;
            }
        }
    }
}