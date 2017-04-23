using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;
using Bpm.NotationElements;
using Bpm.NotationElements.Gateways;

namespace Bpm.Helpers
{
    public static class LinqXml
    {
        public static void x(this XmlElement xml, int x)
        {
            xml.SetAttribute("x", x + "");
        }

        public static void y(this XmlElement xml, int y)
        {
            xml.SetAttribute("y", y + "");
        }

        public static int x(this XmlElement xml)
        {
            return int.Parse(xml.GetAttribute("x"));
        }

        public static int y(this XmlElement xml)
        {
            return int.Parse(xml.GetAttribute("y"));
        }

        public static int width(this XmlElement xml)
        {
            return int.Parse(xml.GetAttribute("width"));
        }

        public static int height(this XmlElement xml)
        {
            return int.Parse(xml.GetAttribute("height"));
        }

        public static string id(this XmlElement xml)
        {
            return xml.GetAttribute("id");
        }
    }

    public class XmlHelper
    {
        private const string diUri = "http://www.omg.org/spec/DD/20100524/DI";
        private const string bpmnUri = "http://www.omg.org/spec/BPMN/20100524/MODEL";
        private const string bpmndiUri = "http://www.omg.org/spec/BPMN/20100524/DI";
        private const string dcUri = "http://www.omg.org/spec/DD/20100524/DC";

        public static string GetXMLFromObject(object o)
        {
            var sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                    tw.Close();
            }
            return sw.ToString();
        }

        public static object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception exp)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
                if (strReader != null)
                    strReader.Close();
            }
            return obj;
        }

        private static string ReadFile(string path)
        {
            var fullPath = HostingEnvironment.MapPath(path);

            if (fullPath == null)
                fullPath = @"C:\Users\tobia\Documents\GitHub\BpmSolution\BpmSolution\App_Data\baseBpmnIo.xml";

            var content = File.ReadAllText(fullPath);
            return content;
        }

        private static XmlDocument Base()
        {
            var xml = new XmlDocument();
            xml.LoadXml(ReadFile("~/app_Data/baseBpmnIo.xml"));
            return xml;
        }

        public static XmlDocument BpmnToXml(BpmGenome genome)
        {
            var xml = Base();

            var data = xml.CreateElement("bpmn", "data", bpmnUri);
            xml.DocumentElement.AppendChild(data);

            var process = xml.CreateElement("bpmn", "process", bpmnUri);
            process.SetAttribute("id", "Process_" + Guid.NewGuid());
            process.SetAttribute("isExecutable", "false");
            xml.DocumentElement.AppendChild(process);

            var start = StartEvent(process);
            var outgoing = GeneToXml(process, genome.RootGene, start);
            var end = EndEvent(process, outgoing);

            var diagram = xml.CreateElement("bpmndi", "BPMNDiagram", bpmndiUri);
            diagram.SetAttribute("id", "BPMNDiagram_" + Guid.NewGuid());
            xml.DocumentElement.AppendChild(diagram);

            var plane = xml.CreateElement("bpmndi", "BPMNPlane", bpmndiUri);
            plane.SetAttribute("id", "BPMNPlane_" + Guid.NewGuid());
            plane.SetAttribute("bpmnElement", process.GetAttribute("id"));
            diagram.AppendChild(plane);

            var layouter = new Layouter(xml.DocumentElement);
            layouter.auto(genome.RootGene, start, end);

            layouter.flows(process);

            foreach (var x in layouter.AllElements())
                plane.AppendChild(x);


            return xml;
        }

        private static XmlElement GeneToXml(XmlElement process, BpmGene gene, XmlElement incomingSequence)
        {
            if (gene is BpmnActivity)
            {
                var activity = process.OwnerDocument.CreateElement("bpmn", "task", bpmnUri);
                activity.SetAttribute("id", "Task_" + gene.Id);
                activity.SetAttribute("name", (gene as BpmnActivity).Name);
                process.AppendChild(activity);

                var incoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                incoming.InnerText = incomingSequence.GetAttribute("id");
                activity.AppendChild(incoming);

                incomingSequence.SetAttribute("targetRef", activity.GetAttribute("id"));

                var outgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                outgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                activity.AppendChild(outgoing);

                var sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", outgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", activity.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                return sequenceFlow;
            }
            if (gene is BpmnAnd)
            {
                var attributeName = "ParallelGateway_";
                var localName = "parallelGateway";

                // Open Gateway
                var gatewayOpen = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayOpen.SetAttribute("id", attributeName + "open_" + gene.Id);
                process.AppendChild(gatewayOpen);

                // Gateway open incoming
                var gatewayOpenIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                gatewayOpenIncoming.InnerText = incomingSequence.GetAttribute("id");
                gatewayOpen.AppendChild(gatewayOpenIncoming);

                // Incoming Sequence
                incomingSequence.SetAttribute("targetRef", gatewayOpen.GetAttribute("id"));

                // Close Gateway
                var gatewayClose = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayClose.SetAttribute("id", attributeName + "close_" + gene.Id);
                process.AppendChild(gatewayClose);

                // Gateway close outgoing
                var gatewayCloseOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayCloseOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayClose.AppendChild(gatewayCloseOutgoing);

                // Outgoing Sequence
                var sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", gatewayCloseOutgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", gatewayClose.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                foreach (var child in gene.Children)
                {
                    // Gateway open outgoing
                    var gatewayOpenOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                    gatewayOpenOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                    gatewayOpen.AppendChild(gatewayOpenOutgoing);

                    // Gateway open outgoing sequence
                    var innerSequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                    innerSequenceFlow.SetAttribute("id", gatewayOpenOutgoing.InnerText);
                    innerSequenceFlow.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                    process.AppendChild(innerSequenceFlow);

                    innerSequenceFlow = GeneToXml(process, child, innerSequenceFlow);

                    // gateway close incoming
                    var gatewayCloseIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncoming.InnerText = innerSequenceFlow.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncoming);

                    // gateway close incoming sequence
                    innerSequenceFlow.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }

                return sequenceFlow;
            }
            if (gene is BpmnXor)
            {
                var attributeName = "ExclusiveGateway_";
                var localName = "exclusiveGateway";

                // Open Gateway
                var gatewayOpen = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayOpen.SetAttribute("id", attributeName + "open_" + gene.Id);
                process.AppendChild(gatewayOpen);

                // add name/id to gateway
                gatewayOpen.SetAttribute("name", "v[" + (gene as BpmnXor).ToProcessAttribute().DecisionId + "]");

                // Gateway open incoming
                var gatewayOpenIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                gatewayOpenIncoming.InnerText = incomingSequence.GetAttribute("id");
                gatewayOpen.AppendChild(gatewayOpenIncoming);

                // Incoming Sequence
                incomingSequence.SetAttribute("targetRef", gatewayOpen.GetAttribute("id"));

                // Close Gateway
                var gatewayClose = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayClose.SetAttribute("id", attributeName + "close_" + gene.Id);
                process.AppendChild(gatewayClose);

                // Gateway close outgoing
                var gatewayCloseOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayCloseOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayClose.AppendChild(gatewayCloseOutgoing);

                // Outgoing Sequence
                var sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", gatewayCloseOutgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", gatewayClose.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                #region XOR-if

                // Gateway open outgoing
                var gatewayOpenOutgoingIf = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayOpenOutgoingIf.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayOpen.AppendChild(gatewayOpenOutgoingIf);

                // Gateway open outgoing sequence
                var innerSequenceFlowIf = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                innerSequenceFlowIf.SetAttribute("id", gatewayOpenOutgoingIf.InnerText);
                innerSequenceFlowIf.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                process.AppendChild(innerSequenceFlowIf);

                // add name/decision to sequenceFlow
                innerSequenceFlowIf.SetAttribute("name", "==" + (gene as BpmnXor).ToProcessAttribute().DecisionValue);

                if (gene.Children == null || gene.Children.Count == 0 || gene.Children[0] == null)
                {
                    // gateway close incoming
                    var gatewayCloseIncomingIf = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingIf.InnerText = innerSequenceFlowIf.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingIf);

                    // gateway close incoming sequence
                    innerSequenceFlowIf.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                else
                {
                    innerSequenceFlowIf = GeneToXml(process, gene.Children[0], innerSequenceFlowIf);

                    // gateway close incoming
                    var gatewayCloseIncomingIf = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingIf.InnerText = innerSequenceFlowIf.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingIf);

                    // gateway close incoming sequence
                    innerSequenceFlowIf.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }

                #endregion

                #region XOR-else

                // Gateway open outgoing
                var gatewayOpenOutgoingElse = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayOpenOutgoingElse.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayOpen.AppendChild(gatewayOpenOutgoingElse);

                // Gateway open outgoing sequence
                var innerSequenceFlowElse = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                innerSequenceFlowElse.SetAttribute("id", gatewayOpenOutgoingElse.InnerText);
                innerSequenceFlowElse.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                process.AppendChild(innerSequenceFlowElse);

                // add name/decision to sequenceFlow
                innerSequenceFlowElse.SetAttribute("name", "<>" + (gene as BpmnXor).ToProcessAttribute().DecisionValue);

                if (gene.Children == null || gene.Children.Count < 2 || gene.Children[1] == null)
                {
                    // gateway close incoming
                    var gatewayCloseIncomingElse = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingElse.InnerText = innerSequenceFlowElse.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingElse);

                    // gateway close incoming sequence
                    innerSequenceFlowElse.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                else
                {
                    innerSequenceFlowElse = GeneToXml(process, gene.Children[1], innerSequenceFlowElse);

                    // gateway close incoming
                    var gatewayCloseIncomingElse = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingElse.InnerText = innerSequenceFlowElse.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingElse);

                    // gateway close incoming sequence
                    innerSequenceFlowElse.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }

                #endregion

                return sequenceFlow;
            }
            foreach (var child in gene.Children)
                incomingSequence = GeneToXml(process, child, incomingSequence);

            return incomingSequence;
        }

        private static XmlElement StartEvent(XmlElement xml)
        {
            var startEvent = xml.OwnerDocument.CreateElement("bpmn", "startEvent", bpmnUri);
            startEvent.SetAttribute("id", "StartEvent_" + Guid.NewGuid());
            xml.AppendChild(startEvent);

            var outgoing = xml.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
            outgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
            startEvent.AppendChild(outgoing);

            var sequenceFlow = xml.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
            sequenceFlow.SetAttribute("id", outgoing.InnerText);
            sequenceFlow.SetAttribute("sourceRef", startEvent.GetAttribute("id"));
            xml.AppendChild(sequenceFlow);

            return sequenceFlow;
        }

        private static XmlElement EndEvent(XmlElement xml, XmlElement incomingSequence)
        {
            var endEvent = xml.OwnerDocument.CreateElement("bpmn", "endEvent", bpmnUri);
            endEvent.SetAttribute("id", "EndEvent_" + Guid.NewGuid());

            var incoming = xml.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
            incoming.InnerText = incomingSequence.GetAttribute("id");

            incomingSequence.SetAttribute("targetRef", endEvent.GetAttribute("id"));

            xml.AppendChild(endEvent);
            endEvent.AppendChild(incoming);

            return endEvent;
        }

        public static XmlDocument Dummy()
        {
            var xml = new XmlDocument();
            //xml.LoadXml(ReadFile("~/App_Data/defaultDiagram.xml"));
            xml.LoadXml(ReadFile("~/App_Data/futureDiagram.xml"));
            return xml;
        }

        internal class Layouter
        {
            private const int elementSize = 50;
            private const int elementSpace = 50;
            private readonly XmlElement doc;
            private readonly List<XmlElement> flowElements = new List<XmlElement>();
            private XmlElement[][] matrix;

            public Layouter(XmlElement doc)
            {
                this.doc = doc;
                matrix = new XmlElement[1][];
                matrix[0] = new XmlElement[1];
            }

            private XmlElement Start(XmlElement start)
            {
                var Start = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                Start.SetAttribute("id", start.GetAttribute("sourceRef") + "_di");
                Start.SetAttribute("bpmnElement", start.GetAttribute("sourceRef"));
                return Start;
            }

            private XmlElement End(XmlElement end)
            {
                var End = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                End.SetAttribute("id", end.GetAttribute("id") + "_di");
                End.SetAttribute("bpmnElement", end.GetAttribute("id"));
                return End;
            }

            private XmlElement Activity(BpmGene gene)
            {
                var Activity = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                Activity.SetAttribute("id", "Task_" + gene.Id + "_di");
                Activity.SetAttribute("bpmnElement", "Task_" + gene.Id);
                return Activity;
            }

            private XmlElement And(BpmGene gene, bool open = true)
            {
                var And = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);

                if (open)
                {
                    And.SetAttribute("id", "ParallelGateway_open_" + gene.Id + "_di");
                    And.SetAttribute("bpmnElement", "ParallelGateway_open_" + gene.Id);
                }
                else
                {
                    And.SetAttribute("id", "ParallelGateway_close_" + gene.Id + "_di");
                    And.SetAttribute("bpmnElement", "ParallelGateway_close_" + gene.Id);
                }

                return And;
            }

            private XmlElement Xor(BpmGene gene, bool open = true)
            {
                var Xor = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                Xor.SetAttribute("isMarkerVisible", "true");

                if (open)
                {
                    Xor.SetAttribute("id", "ExclusiveGateway_open_" + gene.Id + "_di");
                    Xor.SetAttribute("bpmnElement", "ExclusiveGateway_open_" + gene.Id);
                }
                else
                {
                    Xor.SetAttribute("id", "ExclusiveGateway_close_" + gene.Id + "_di");
                    Xor.SetAttribute("bpmnElement", "ExclusiveGateway_close_" + gene.Id);
                }

                return Xor;
            }

            public void auto(BpmGene gene, XmlElement start, XmlElement end)
            {
                matrix[0][0] = Start(start);

                AddColumn();
                var d = AddGene(gene, 0, 1);

                AddColumn();
                matrix[0][d.width + 1] = End(end);

                SetPositions();
            }

            private Dimension AddGene(BpmGene gene, int x, int y)
            {
                if (gene is BpmnAnd)
                {
                    var dim = new Dimension();

                    var and = And(gene);
                    matrix[x][y] = and;

                    AddColumn();

                    AddRow(gene.Children.Count - 1);

                    foreach (var child in gene.Children)
                    {
                        var d = AddGene(child, x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }

                    AddColumn();

                    var and2 = And(gene, false);
                    matrix[x][y + 1 + dim.width] = and2;

                    dim.width += 2;

                    return dim;
                }
                if (gene is BpmnXor)
                {
                    var dim = new Dimension();

                    var xor = Xor(gene);
                    matrix[x][y] = xor;

                    AddColumn();

                    AddRow(1);

                    if (gene.Children == null || gene.Children.Count == 0)
                    {
                        // nothind to do
                    }
                    else if (gene.Children.Count == 1)
                    {
                        // only one child -> first the empty one
                        dim.height += 1; // add empty row for straight line

                        // now the regular genes
                        var d = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }
                    else if (gene.Children[0] != null && gene.Children[1] != null)
                    {
                        // if
                        var d1 = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d1.width);
                        dim.height += d1.height;

                        // else
                        var d2 = AddGene(gene.Children[1], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d2.width);
                        dim.height += d2.height;
                    }
                    else
                    {
                        // if == null && else != null
                        // only one child -> first the empty one
                        dim.height += 1; // add empty row for straight line

                        // now the regular genes
                        var d = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }

                    AddColumn();

                    var xor2 = Xor(gene, false);
                    matrix[x][y + 1 + dim.width] = xor2;

                    dim.width += 2;

                    return dim;
                }
                if (gene is BpmnSeq)
                {
                    var dim = new Dimension();

                    foreach (var child in gene.Children)
                    {
                        var d = AddGene(child, x, y + dim.width);
                        dim.width += d.width;
                        dim.height = Math.Max(dim.height, d.height);

                        AddColumn();
                    }

                    return dim;
                }
                var activity = Activity(gene);
                matrix[x][y] = activity;

                return new Dimension {width = 1, height = 1};
            }

            public List<XmlElement> AllElements()
            {
                var all = new List<XmlElement>();

                foreach (var row in matrix)
                foreach (var columns in row)
                    if (columns != null)
                        all.Add(columns);

                all.AddRange(flowElements);

                return all;
            }

            private void SetPositions()
            {
                for (var i = 0; i < matrix.Length; i++)
                for (var j = 0; j < matrix[i].Length; j++)
                    if (matrix[i][j] != null)
                    {
                        var bounds = doc.OwnerDocument.CreateElement("dc", "Bounds", dcUri);
                        bounds.SetAttribute("width", elementSize + "");
                        bounds.SetAttribute("height", elementSize + "");
                        bounds.SetAttribute("x", j * (elementSize + elementSpace) + "");
                        bounds.SetAttribute("y", i * (elementSize + elementSpace) + "");

                        matrix[i][j].AppendChild(bounds);
                    }
            }

            private void AddColumn()
            {
                var newMatrix = new XmlElement[matrix.Length][];

                for (var i = 0; i < matrix.Length; i++)
                {
                    newMatrix[i] = new XmlElement[matrix[i].Length + 1];
                    Array.Copy(matrix[i], newMatrix[i], matrix[i].Length);
                }

                matrix = newMatrix;
            }

            private void AddRow(int count = 1)
            {
                if (count <= 0)
                    return;

                var newMatrix = new XmlElement[matrix.Length + count][];

                for (var i = 0; i < matrix.Length; i++)
                {
                    newMatrix[i] = new XmlElement[matrix[i].Length];
                    Array.Copy(matrix[i], newMatrix[i], matrix[i].Length);
                }

                for (var i = matrix.Length; i < matrix.Length + count; i++)
                    newMatrix[i] = new XmlElement[newMatrix[0].Length];

                matrix = newMatrix;
            }

            public void flows(XmlElement process)
            {
                var flows = process.GetElementsByTagName("bpmn:sequenceFlow");

                foreach (XmlNode node in flows)
                    arrangeFlow(node as XmlElement);
            }

            private XmlElement GetBounds(XmlElement e)
            {
                return e.GetElementsByTagName("dc:Bounds").Item(0) as XmlElement;
            }

            private void arrangeFlow(XmlElement flow)
            {
                var sourceId = flow.GetAttribute("sourceRef");
                var targetId = flow.GetAttribute("targetRef");

                var sourceElement = findElement(sourceId);
                var targetElement = findElement(targetId);

                var sourceBounds = GetBounds(sourceElement);
                var targetBounds = GetBounds(targetElement);

                var edge = doc.OwnerDocument.CreateElement("bpmndi", "BPMNEdge", bpmndiUri);
                edge.SetAttribute("id", flow.id() + "_id");
                edge.SetAttribute("bpmnElement", flow.GetAttribute("id"));

                if (sourceBounds.y() == targetBounds.y())
                {
                    // waagrecht
                    var waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + sourceBounds.width());
                    waypointA.y(sourceBounds.y() + sourceBounds.height() / 2);

                    var waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(targetBounds.x());
                    waypointB.y(targetBounds.y() + targetBounds.height() / 2);

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                }
                else if (sourceBounds.y() < targetBounds.y())
                {
                    // runter rechts
                    var waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + sourceBounds.width() / 2);
                    waypointA.y(sourceBounds.y() + sourceBounds.height());

                    var waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(sourceBounds.x() + sourceBounds.width() / 2);
                    waypointB.y(targetBounds.y() + targetBounds.height() / 2);

                    var waypointC = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointC.x(targetBounds.x());
                    waypointC.y(targetBounds.y() + targetBounds.height() / 2);

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                    edge.AppendChild(waypointC);
                }
                else if (sourceBounds.y() > targetBounds.y())
                {
                    // rechts hoch
                    var waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + sourceBounds.width());
                    waypointA.y(sourceBounds.y() + sourceBounds.height() / 2);

                    var waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(targetBounds.x() + targetBounds.width() / 2);
                    waypointB.y(sourceBounds.y() + sourceBounds.height() / 2);

                    var waypointC = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointC.x(targetBounds.x() + targetBounds.width() / 2);
                    waypointC.y(targetBounds.y() + targetBounds.height());

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                    edge.AppendChild(waypointC);
                }


                flowElements.Add(edge);
            }

            private XmlElement findElement(string id)
            {
                for (var i = 0; i < matrix.Length; i++)
                for (var j = 0; j < matrix[i].Length; j++)
                    if (matrix[i][j] != null && matrix[i][j].GetAttribute("id").Equals(id + "_di"))
                        return matrix[i][j];

                return null;
            }

            internal class Dimension
            {
                internal int height;
                internal int width;
            }
        }
    }
}