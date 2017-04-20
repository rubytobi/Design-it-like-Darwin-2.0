using Bpm;
using Bpm.NotationElements;
using Bpm.NotationElements.Gateways;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

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

        private static string ReadFile(string path)
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath(path);

            if (fullPath == null)
            {
                fullPath = @"C:\Users\tobia\Documents\GitHub\BpmSolution\BpmSolution\App_Data\baseBpmnIo.xml";
            }

            var content = System.IO.File.ReadAllText(fullPath);
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
            XmlDocument xml = Base();

            XmlElement process = xml.CreateElement("bpmn", "process", bpmnUri);
            process.SetAttribute("id", "Process_" + Guid.NewGuid());
            process.SetAttribute("isExecutable", "false");
            xml.DocumentElement.AppendChild(process);

            XmlElement start = StartEvent(process);
            XmlElement outgoing = GeneToXml(process, genome.RootGene, start);
            XmlElement end = EndEvent(process, outgoing);

            XmlElement diagram = xml.CreateElement("bpmndi", "BPMNDiagram", bpmndiUri);
            diagram.SetAttribute("id", "BPMNDiagram_" + Guid.NewGuid());
            xml.DocumentElement.AppendChild(diagram);

            XmlElement plane = xml.CreateElement("bpmndi", "BPMNPlane", bpmndiUri);
            plane.SetAttribute("id", "BPMNPlane_" + Guid.NewGuid());
            plane.SetAttribute("bpmnElement", process.GetAttribute("id"));
            diagram.AppendChild(plane);

            Layouter layouter = new Layouter(xml.DocumentElement);
            layouter.auto(genome.RootGene, start, end);

            layouter.flows(process);

            foreach (XmlElement x in layouter.AllElements())
            {
                plane.AppendChild(x);
            }



            return xml;
        }

        internal class Layouter
        {
            internal class Dimension
            {
                internal int width = 0;
                internal int height = 0;
            }

            private const int elementSize = 50;
            private const int elementSpace = 50;
            XmlElement[][] matrix;
            List<XmlElement> flowElements = new List<XmlElement>();
            XmlElement doc;

            public Layouter(XmlElement doc)
            {
                this.doc = doc;
                matrix = new XmlElement[1][];
                matrix[0] = new XmlElement[1];
            }

            private XmlElement Start(XmlElement start)
            {
                XmlElement Start = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                Start.SetAttribute("id", start.GetAttribute("sourceRef") + "_di");
                Start.SetAttribute("bpmnElement", start.GetAttribute("sourceRef"));
                return Start;
            }

            private XmlElement End(XmlElement end)
            {
                XmlElement End = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                End.SetAttribute("id", end.GetAttribute("id") + "_di");
                End.SetAttribute("bpmnElement", end.GetAttribute("id"));
                return End;
            }

            private XmlElement Activity(BpmGene gene)
            {
                XmlElement Activity = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
                Activity.SetAttribute("id", "Task_" + gene.Id + "_di");
                Activity.SetAttribute("bpmnElement", "Task_" + gene.Id);
                return Activity;
            }

            private XmlElement And(BpmGene gene, bool open = true)
            {
                XmlElement And = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);

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
                XmlElement Xor = doc.OwnerDocument.CreateElement("bpmndi", "BPMNShape", bpmndiUri);
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
                Dimension d = AddGene(gene, 0, 1);

                AddColumn();
                matrix[0][d.width + 1] = End(end);

                SetPositions();
            }

            private Dimension AddGene(BpmGene gene, int x, int y)
            {
                if (gene is BpmnAnd)
                {
                    Dimension dim = new Dimension();

                    XmlElement and = And(gene);
                    matrix[x][y] = and;

                    AddColumn();

                    AddRow(gene.Children.Count - 1);

                    foreach (BpmGene child in gene.Children)
                    {
                        Dimension d = AddGene(child, x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }

                    AddColumn();

                    XmlElement and2 = And(gene, false);
                    matrix[x][y + 1 + dim.width] = and2;

                    dim.width += 2;

                    return dim;
                }
                else if (gene is BpmnXor)
                {
                    Dimension dim = new Dimension();

                    XmlElement xor = Xor(gene);
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
                        Dimension d = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }
                    else if (gene.Children[0] != null && gene.Children[1] != null)
                    {
                        // if
                        Dimension d1 = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d1.width);
                        dim.height += d1.height;

                        // else
                        Dimension d2 = AddGene(gene.Children[1], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d2.width);
                        dim.height += d2.height;
                    }
                    else
                    {
                        // if == null && else != null
                        // only one child -> first the empty one
                        dim.height += 1; // add empty row for straight line

                        // now the regular genes
                        Dimension d = AddGene(gene.Children[0], x + dim.height, y + 1);
                        dim.width = Math.Max(dim.width, d.width);
                        dim.height += d.height;
                    }

                    AddColumn();

                    XmlElement xor2 = Xor(gene, false);
                    matrix[x][y + 1 + dim.width] = xor2;

                    dim.width += 2;

                    return dim;
                }
                else if (gene is BpmnSeq)
                {
                    Dimension dim = new Dimension();

                    foreach (BpmGene child in gene.Children)
                    {
                        Dimension d = AddGene(child, x, y + dim.width);
                        dim.width += d.width;
                        dim.height = Math.Max(dim.height, d.height);

                        AddColumn();
                    }

                    return dim;
                }
                else
                {
                    XmlElement activity = Activity(gene);
                    matrix[x][y] = activity;

                    return new Dimension { width = 1, height = 1 };
                }
            }

            public List<XmlElement> AllElements()
            {
                List<XmlElement> all = new List<XmlElement>();

                foreach (XmlElement[] row in matrix)
                {
                    foreach (XmlElement columns in row)
                    {
                        if (columns != null)
                        {
                            all.Add(columns);
                        }
                    }
                }

                all.AddRange(flowElements);

                return all;
            }

            private void SetPositions()
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (matrix[i][j] != null)
                        {
                            XmlElement bounds = doc.OwnerDocument.CreateElement("dc", "Bounds", dcUri);
                            bounds.SetAttribute("width", elementSize + "");
                            bounds.SetAttribute("height", elementSize + "");
                            bounds.SetAttribute("x", (j * (elementSize + elementSpace)) + "");
                            bounds.SetAttribute("y", (i * (elementSize + elementSpace)) + "");

                            matrix[i][j].AppendChild(bounds);
                        }
                    }
                }

            }

            private void AddColumn()
            {
                XmlElement[][] newMatrix = new XmlElement[matrix.Length][];

                for (int i = 0; i < matrix.Length; i++)
                {
                    newMatrix[i] = new XmlElement[matrix[i].Length + 1];
                    Array.Copy(matrix[i], newMatrix[i], matrix[i].Length);
                }

                matrix = newMatrix;
            }

            private void AddRow(int count = 1)
            {
                if (count <= 0)
                {
                    return;
                }

                XmlElement[][] newMatrix = new XmlElement[matrix.Length + count][];

                for (int i = 0; i < matrix.Length; i++)
                {
                    newMatrix[i] = new XmlElement[matrix[i].Length];
                    Array.Copy(matrix[i], newMatrix[i], matrix[i].Length);
                }

                for (int i = matrix.Length; i < matrix.Length + count; i++)
                {
                    newMatrix[i] = new XmlElement[newMatrix[0].Length];
                }

                matrix = newMatrix;
            }

            public void flows(XmlElement process)
            {
                XmlNodeList flows = process.GetElementsByTagName("bpmn:sequenceFlow");

                foreach (XmlNode node in flows)
                {
                    arrangeFlow(node as XmlElement);
                }
            }

            private XmlElement GetBounds(XmlElement e)
            {
                return e.GetElementsByTagName("dc:Bounds").Item(0) as XmlElement;
            }

            private void arrangeFlow(XmlElement flow)
            {
                string sourceId = flow.GetAttribute("sourceRef");
                string targetId = flow.GetAttribute("targetRef");

                XmlElement sourceElement = findElement(sourceId);
                XmlElement targetElement = findElement(targetId);

                XmlElement sourceBounds = GetBounds(sourceElement);
                XmlElement targetBounds = GetBounds(targetElement);

                XmlElement edge = doc.OwnerDocument.CreateElement("bpmndi", "BPMNEdge", bpmndiUri);
                edge.SetAttribute("id", flow.id() + "_id");
                edge.SetAttribute("bpmnElement", flow.GetAttribute("id"));

                if (sourceBounds.y() == targetBounds.y())
                {
                    // waagrecht
                    XmlElement waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + sourceBounds.width());
                    waypointA.y(sourceBounds.y() + (sourceBounds.height() / 2));

                    XmlElement waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(targetBounds.x());
                    waypointB.y(targetBounds.y() + (targetBounds.height() / 2));

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                }
                else if (sourceBounds.y() < targetBounds.y())
                {
                    // runter rechts
                    XmlElement waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + (sourceBounds.width() / 2));
                    waypointA.y(sourceBounds.y() + sourceBounds.height());

                    XmlElement waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(sourceBounds.x() + (sourceBounds.width() / 2));
                    waypointB.y(targetBounds.y() + (targetBounds.height() / 2));

                    XmlElement waypointC = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointC.x(targetBounds.x());
                    waypointC.y(targetBounds.y() + (targetBounds.height() / 2));

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                    edge.AppendChild(waypointC);
                }
                else if (sourceBounds.y() > targetBounds.y())
                {
                    // rechts hoch
                    XmlElement waypointA = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointA.x(sourceBounds.x() + sourceBounds.width());
                    waypointA.y(sourceBounds.y() + (sourceBounds.height() / 2));

                    XmlElement waypointB = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointB.x(targetBounds.x() + (targetBounds.width() / 2));
                    waypointB.y(sourceBounds.y() + (sourceBounds.height() / 2));

                    XmlElement waypointC = doc.OwnerDocument.CreateElement("di", "waypoint", diUri);
                    waypointC.x(targetBounds.x() + (targetBounds.width() / 2));
                    waypointC.y(targetBounds.y() + targetBounds.height());

                    edge.AppendChild(waypointA);
                    edge.AppendChild(waypointB);
                    edge.AppendChild(waypointC);
                }


                flowElements.Add(edge);
            }

            private XmlElement findElement(string id)
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (matrix[i][j] != null && matrix[i][j].GetAttribute("id").Equals(id + "_di"))
                        {
                            return matrix[i][j];
                        }
                    }
                }

                return null;
            }
        }

        private static XmlElement GeneToXml(XmlElement process, BpmGene gene, XmlElement incomingSequence)
        {
            if (gene is BpmnActivity)
            {
                XmlElement activity = process.OwnerDocument.CreateElement("bpmn", "task", bpmnUri);
                activity.SetAttribute("id", "Task_" + gene.Id);
                activity.SetAttribute("name", (gene as BpmnActivity).Name);
                process.AppendChild(activity);

                XmlElement incoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                incoming.InnerText = incomingSequence.GetAttribute("id");
                activity.AppendChild(incoming);

                incomingSequence.SetAttribute("targetRef", activity.GetAttribute("id"));

                XmlElement outgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                outgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                activity.AppendChild(outgoing);

                XmlElement sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", outgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", activity.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                return sequenceFlow;
            }
            else if (gene is BpmnAnd)
            {
                string attributeName = "ParallelGateway_";
                string localName = "parallelGateway";

                // Open Gateway
                XmlElement gatewayOpen = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayOpen.SetAttribute("id", attributeName + "open_" + gene.Id);
                process.AppendChild(gatewayOpen);

                // Gateway open incoming
                XmlElement gatewayOpenIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                gatewayOpenIncoming.InnerText = incomingSequence.GetAttribute("id");
                gatewayOpen.AppendChild(gatewayOpenIncoming);

                // Incoming Sequence
                incomingSequence.SetAttribute("targetRef", gatewayOpen.GetAttribute("id"));

                // Close Gateway
                XmlElement gatewayClose = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayClose.SetAttribute("id", attributeName + "close_" + gene.Id);
                process.AppendChild(gatewayClose);

                // Gateway close outgoing
                XmlElement gatewayCloseOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayCloseOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayClose.AppendChild(gatewayCloseOutgoing);

                // Outgoing Sequence
                XmlElement sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", gatewayCloseOutgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", gatewayClose.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                foreach (BpmGene child in gene.Children)
                {
                    // Gateway open outgoing
                    XmlElement gatewayOpenOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                    gatewayOpenOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                    gatewayOpen.AppendChild(gatewayOpenOutgoing);

                    // Gateway open outgoing sequence
                    XmlElement innerSequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                    innerSequenceFlow.SetAttribute("id", gatewayOpenOutgoing.InnerText);
                    innerSequenceFlow.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                    process.AppendChild(innerSequenceFlow);

                    innerSequenceFlow = GeneToXml(process, child, innerSequenceFlow);

                    // gateway close incoming
                    XmlElement gatewayCloseIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncoming.InnerText = innerSequenceFlow.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncoming);

                    // gateway close incoming sequence
                    innerSequenceFlow.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }

                return sequenceFlow;
            }
            else if (gene is BpmnXor)
            {
                string attributeName = "ExclusiveGateway_";
                string localName = "exclusiveGateway";

                // Open Gateway
                XmlElement gatewayOpen = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayOpen.SetAttribute("id", attributeName + "open_" + gene.Id);
                process.AppendChild(gatewayOpen);

                // add name/id to gateway
                gatewayOpen.SetAttribute("name", "v[" + (gene as BpmnXor).ToProcessAttribute().DecisionId + "]");

                // Gateway open incoming
                XmlElement gatewayOpenIncoming = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                gatewayOpenIncoming.InnerText = incomingSequence.GetAttribute("id");
                gatewayOpen.AppendChild(gatewayOpenIncoming);

                // Incoming Sequence
                incomingSequence.SetAttribute("targetRef", gatewayOpen.GetAttribute("id"));

                // Close Gateway
                XmlElement gatewayClose = process.OwnerDocument.CreateElement("bpmn", localName, bpmnUri);
                gatewayClose.SetAttribute("id", attributeName + "close_" + gene.Id);
                process.AppendChild(gatewayClose);

                // Gateway close outgoing
                XmlElement gatewayCloseOutgoing = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayCloseOutgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayClose.AppendChild(gatewayCloseOutgoing);

                // Outgoing Sequence
                XmlElement sequenceFlow = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                sequenceFlow.SetAttribute("id", gatewayCloseOutgoing.InnerText);
                sequenceFlow.SetAttribute("sourceRef", gatewayClose.GetAttribute("id"));
                process.AppendChild(sequenceFlow);

                #region XOR-if
                // Gateway open outgoing
                XmlElement gatewayOpenOutgoingIf = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayOpenOutgoingIf.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayOpen.AppendChild(gatewayOpenOutgoingIf);

                // Gateway open outgoing sequence
                XmlElement innerSequenceFlowIf = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                innerSequenceFlowIf.SetAttribute("id", gatewayOpenOutgoingIf.InnerText);
                innerSequenceFlowIf.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                process.AppendChild(innerSequenceFlowIf);

                // add name/decision to sequenceFlow
                innerSequenceFlowIf.SetAttribute("name", "==" + (gene as BpmnXor).ToProcessAttribute().DecisionValue);

                if (gene.Children == null || gene.Children.Count == 0 || gene.Children[0] == null)
                {
                    // gateway close incoming
                    XmlElement gatewayCloseIncomingIf = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingIf.InnerText = innerSequenceFlowIf.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingIf);

                    // gateway close incoming sequence
                    innerSequenceFlowIf.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                else
                {
                    innerSequenceFlowIf = GeneToXml(process, gene.Children[0], innerSequenceFlowIf);

                    // gateway close incoming
                    XmlElement gatewayCloseIncomingIf = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingIf.InnerText = innerSequenceFlowIf.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingIf);

                    // gateway close incoming sequence
                    innerSequenceFlowIf.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                #endregion

                #region XOR-else
                // Gateway open outgoing
                XmlElement gatewayOpenOutgoingElse = process.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
                gatewayOpenOutgoingElse.InnerText = "SequenceFlow_" + Guid.NewGuid();
                gatewayOpen.AppendChild(gatewayOpenOutgoingElse);

                // Gateway open outgoing sequence
                XmlElement innerSequenceFlowElse = process.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
                innerSequenceFlowElse.SetAttribute("id", gatewayOpenOutgoingElse.InnerText);
                innerSequenceFlowElse.SetAttribute("sourceRef", gatewayOpen.GetAttribute("id"));
                process.AppendChild(innerSequenceFlowElse);

                // add name/decision to sequenceFlow
                innerSequenceFlowElse.SetAttribute("name", "<>" + (gene as BpmnXor).ToProcessAttribute().DecisionValue);

                if (gene.Children == null || gene.Children.Count < 2 || gene.Children[1] == null)
                {
                    // gateway close incoming
                    XmlElement gatewayCloseIncomingElse = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingElse.InnerText = innerSequenceFlowElse.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingElse);

                    // gateway close incoming sequence
                    innerSequenceFlowElse.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                else
                {
                    innerSequenceFlowElse = GeneToXml(process, gene.Children[1], innerSequenceFlowElse);

                    // gateway close incoming
                    XmlElement gatewayCloseIncomingElse = process.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
                    gatewayCloseIncomingElse.InnerText = innerSequenceFlowElse.GetAttribute("id");
                    gatewayClose.AppendChild(gatewayCloseIncomingElse);

                    // gateway close incoming sequence
                    innerSequenceFlowElse.SetAttribute("targetRef", gatewayClose.GetAttribute("id"));
                }
                #endregion

                return sequenceFlow;
            }
            else
            {
                foreach (BpmGene child in gene.Children)
                {
                    incomingSequence = GeneToXml(process, child, incomingSequence);
                }

                return incomingSequence;
            }
        }

        private static XmlElement StartEvent(XmlElement xml)
        {
            XmlElement startEvent = xml.OwnerDocument.CreateElement("bpmn", "startEvent", bpmnUri);
            startEvent.SetAttribute("id", "StartEvent_" + Guid.NewGuid());
            xml.AppendChild(startEvent);

            XmlElement outgoing = xml.OwnerDocument.CreateElement("bpmn", "outgoing", bpmnUri);
            outgoing.InnerText = "SequenceFlow_" + Guid.NewGuid();
            startEvent.AppendChild(outgoing);

            XmlElement sequenceFlow = xml.OwnerDocument.CreateElement("bpmn", "sequenceFlow", bpmnUri);
            sequenceFlow.SetAttribute("id", outgoing.InnerText);
            sequenceFlow.SetAttribute("sourceRef", startEvent.GetAttribute("id"));
            xml.AppendChild(sequenceFlow);

            return sequenceFlow;
        }

        private static XmlElement EndEvent(XmlElement xml, XmlElement incomingSequence)
        {
            XmlElement endEvent = xml.OwnerDocument.CreateElement("bpmn", "endEvent", bpmnUri);
            endEvent.SetAttribute("id", "EndEvent_" + Guid.NewGuid());

            XmlElement incoming = xml.OwnerDocument.CreateElement("bpmn", "incoming", bpmnUri);
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

    }
}