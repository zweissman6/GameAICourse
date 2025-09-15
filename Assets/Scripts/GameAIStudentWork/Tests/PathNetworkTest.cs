using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using System.Linq;

using GameAICourse;
using System.Text;

namespace Tests
{
    public class PathNetworkTest
    {
        // You can run the tests in this class in the Unity Editor if you open
        // the Test Runner Window and choose the EditMode tab


        // Annotate methods with [Test] or [TestCase(...)] to create tests like this one!
        [Test]
        public void TestName()
        {
            // Tests are performed through assertions. You can Google NUnit Assertion
            // documentation to learn about them. Several examples follow.
            Assert.That(CreatePathNetwork.StudentAuthorName, Is.Not.Contains("George P. Burdell"),
                "You forgot to change to your name!");
        }


        [Test]
        public void ExampleTest()
        {
            // Set up some parameters for testing

            Vector2 origin = new Vector2(-5f, -5f);
            Vector2 size = new Vector2(10f, 10f);
            List<Polygon> obstacles = new List<Polygon>();
            float agentRadius = 1f;
            List<Vector2> pathNodes = new List<Vector2>()
            {
                new Vector2(0f, 0f), //<-- In the middle of the canvas
                new Vector2(4.5f, 0f) //<-- Close to the middle of the right edge of the canvas boundary
            };

            // output param

            List<List<int>> pathEdges;


            //Execute your code!

            CreatePathNetwork.Create(origin, size.x, size.y, obstacles, agentRadius, agentRadius+0.01f, agentRadius*2.5f, ref pathNodes, out pathEdges, PathNetworkMode.Predefined);

            //Various assertions to validate your returned result

            Assert.That(pathEdges, Is.Not.Null);
            Assert.That(pathEdges, Has.Count.EqualTo(pathNodes.Count));
            Assert.That(pathEdges, Is.All.Not.Null);

            for (int i = 0; i < pathEdges.Count; ++i)
            {
                var edges = pathEdges[i];

                Debug.Log($"[{i}]:{string.Join(",", edges)}");

                //TODO check for self edges, dupe edges, out of range edge ends, etc...

            }

            // Nodes are not expected to connect because right node is too close to canvas boundary

            Assert.That(pathEdges, Is.All.Empty);


            // TODO add more asserts for things not tested in this example
        }

        // TODO write more tests!
        // ==================== ADDITIONAL TESTS ====================
[Test]
public void ZeroNodes_ReturnsEmpty()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var obstacles = new List<Polygon>();
    var nodes = new List<Vector2>();
    List<List<int>> edges;

    CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    Assert.IsNotNull(edges);
    Assert.AreEqual(0, edges.Count);
}

[Test]
public void OneNode_NoEdges()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var obstacles = new List<Polygon>();
    var nodes = new List<Vector2> { new Vector2(50, 50) };
    List<List<int>> edges;

    CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    Assert.AreEqual(1, edges.Count);
    Assert.AreEqual(0, edges[0].Count);
}

[Test]
public void IdenticalNodes_ShouldConnect_ZeroLengthEdgeAllowed()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var obstacles = new List<Polygon>();
    var nodes = new List<Vector2> { new Vector2(50, 50), new Vector2(50, 50) };
    List<List<int>> edges;

    CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    Assert.AreEqual(2, edges.Count);
    CollectionAssert.Contains(edges[0], 1);
    CollectionAssert.Contains(edges[1], 0);
    Assert.IsFalse(edges[0].Contains(0));
    Assert.IsFalse(edges[1].Contains(1));
}

[Test]
public void NodeNearBoundary_IsExcludedByAgentRadius()
{
    var origin = new Vector2(0, 0);
    var W = 10f; var H = 10f;
    var obstacles = new List<Polygon>();
    var radius = 1f;
    var nodes = new List<Vector2>
    {
        new Vector2(0.4f, 5f), // too close to left boundary for r=1
        new Vector2(5f, 5f)
    };
    List<List<int>> edges;

    CreatePathNetwork.Create(origin, W, H, obstacles, radius, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    // First node should be unusable, so no edge between them
    Assert.AreEqual(2, edges.Count);
    Assert.AreEqual(0, edges[0].Count);
    Assert.AreEqual(0, edges[1].Count);
}

[Test]
public void ObstacleIntersection_BlocksEdge()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var nodes = new List<Vector2> { new Vector2(20, 50), new Vector2(80, 50) };

    // Vertical rectangle that intersects the segment between nodes
    var rect = MakeRectPoly(49, 0, 2, 100);
    var obstacles = new List<Polygon> { rect };

    List<List<int>> edges;
    CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    Assert.AreEqual(0, edges[0].Count);
    Assert.AreEqual(0, edges[1].Count);
}

[Test]
public void ClearanceTooTight_BlocksEdge_ThenPassesWithSmallerRadius()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var nodes = new List<Vector2> { new Vector2(20, 50), new Vector2(80, 50) };

    // Skinny horizontal obstacle near the path but not intersecting
    var nearRect = MakeRectPoly(50, 52, 2, 8);
    var obstacles = new List<Polygon> { nearRect };

    // Larger radius: should fail clearance test
    {
        var nodesA = new List<Vector2>(nodes);
        List<List<int>> edges;
        CreatePathNetwork.Create(origin, W, H, obstacles, 3f, 0, 0, ref nodesA, out edges, PathNetworkMode.Predefined);
        Assert.AreEqual(0, edges[0].Count);
    }

    // Smaller radius: should pass
    {
        var nodesB = new List<Vector2>(nodes);
        List<List<int>> edges;
        CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodesB, out edges, PathNetworkMode.Predefined);
        Assert.AreEqual(1, edges[0].Count);
        CollectionAssert.Contains(edges[0], 1);
        CollectionAssert.Contains(edges[1], 0);
    }
}

[Test]
public void Bidirectional_NoDuplicates_NoSelfEdges()
{
    var origin = new Vector2(0, 0);
    var W = 100f; var H = 100f;
    var obstacles = new List<Polygon>();
    var nodes = new List<Vector2>
    {
        new Vector2(20, 20),
        new Vector2(50, 50),
        new Vector2(80, 80)
    };
    List<List<int>> edges;

    CreatePathNetwork.Create(origin, W, H, obstacles, 1f, 0, 0, ref nodes, out edges, PathNetworkMode.Predefined);

    // For any listed edge i->j, we must have j->i; no i->i; and no duplicates
    for (int i = 0; i < edges.Count; i++)
    {
        // no self edges
        Assert.IsFalse(edges[i].Contains(i), $"Node {i} has a self-edge.");

        // no duplicates
        var seen = new HashSet<int>();
        foreach (var j in edges[i])
        {
            Assert.IsTrue(seen.Add(j), $"Duplicate edge {i}->{j}");
            // reciprocal
            Assert.IsTrue(edges[j].Contains(i), $"Edge {i}->{j} without reciprocal {j}->{i}");
            // index in range
            Assert.IsTrue(j >= 0 && j < nodes.Count, $"Edge {i}->{j} out of range.");
        }
    }
}

// ---------- helpers ----------
private Polygon MakeRectPoly(float x, float y, float w, float h)
{
    // CCW rectangle
    var pts = new Vector2[]
    {
        new Vector2(x,       y),
        new Vector2(x + w,   y),
        new Vector2(x + w,   y + h),
        new Vector2(x,       y + h)
    };

    var poly = new Polygon();
    poly.SetPoints(pts);   // this fills intPoints, bounds, centroid, etc.
    return poly;
}




    }
}
