using System;
using Vs.Graph.Abstractions.Structure;
using Vs.Graph.Algorithms.ShortestPath;
using Vs.Graph.Parsers.GraphML;
using Vs.Graph.Structure;
using Xunit;

namespace Vs.Simulation.Core.Tests.State
{
    public class GraphTests
    {
        private class Person : Vertex
        {
            public Person(int bsn)
            {
                Bsn = bsn;
                this.Attributes.Add("created", DateTime.Now);
                this.Attributes.Add("bsn", Bsn);
            }

            public int Bsn { get; set; }
        }

        public class Partner : Edge
        {
            public Partner(IVertex mySource, IVertex myTarget) : base(mySource, myTarget)
            {
                this.Attributes.Add("type", "partnerOf");
                this.Attributes.Add("created", DateTime.Now);
            }
        }

        public class ParentOf : Edge
        {
            public ParentOf(IVertex mySource, IVertex myTarget) : base(mySource, myTarget)
            {
                this.Attributes.Add("type", "parentOf");
                this.Attributes.Add("created", DateTime.Now);
            }
        }

        [Fact]
        public void CreateGraphTest()
        {
            var g = new Graph.Structure.Database();
            g["label"] = "Population";
            var person1 = new Person(12345);
            var person2 = new Person(123456);
            var person3 = new Person(1234567);

            g.AddVertex(person1);
            g.AddVertex(person2);
            g.AddVertex(person3);
            g.AddEdge(new Partner(person1, person2));
            g.AddEdge(new ParentOf(person1, person3));
            g.AddEdge(new ParentOf(person2, person3));

            var path = BreadthFirstSearch.Search(g, person1, person2);
            foreach (Person v in path)
            {
                Console.WriteLine(v.UUID);
            }
            // Write to graph ml file

            Console.WriteLine($"Write: {g.VertexCount} vertices and {g.EdgeCount} edges.");
            GraphMLWriter writer = new GraphMLWriter(true);
            writer.AddAttribute("type", "string", "type", "edge");
            writer.AddAttribute("bsn", "string", "bsn", "node");
            writer.AddAttribute("created", "string", "created", "edge");
            writer.Write(g, "population.graphml");
        }
    }
}
