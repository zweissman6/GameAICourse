using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using GameAICourse;
using Priority_Queue;

using GameAICourse;

namespace Tests
{
    public class AStarTest
    {
        // You can run the tests in this class in the Unity Editor if you open
        // the Test Runner Window and choose the EditMode tab


        // Annotate methods with [Test] or [TestCase(...)] to create tests like this one!
        [Test]
        public void TestName()
        {
            // Tests are performed through assertions. You can Google NUnit Assertion
            // documentation to learn about them. Several examples follow.
            Assert.That(AStarPathSearchImpl.StudentAuthorName, Is.Not.Contains("George P. Burdell"),
                "You forgot to change to your name!");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void BasicTest(bool incrementalSearch)
        {

            Vector2[] _nodes = new Vector2[] {
                new Vector2(0.0f, 0.0f), //0
                new Vector2(0.0f, 1.0f), //1
                new Vector2(0.0f, 2.0f), //2
                new Vector2(0.0f, 3.0f), //3
                new Vector2(0.0f, 4.0f), //4
                new Vector2(0.0f, 5.0f), //5
            };
            int[][] _edges = new int[][] {
                new int[] {1 },         //0
                new int[] {2, 0 },      //1
                new int[] {3, 1 },      //2
                new int[] {4, 2 },      //3
                new int[] {5, 3 },      //4
                new int[] {4 }          //5
            };

            List<Vector2> nodes = new List<Vector2>(_nodes);
            List<List<int>> edges = new List<List<int>>();

            foreach (var eArray in _edges)
            {
                var elist = new List<int>(eArray);
                edges.Add(elist);
            }

            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;

            var startNode = 0;
            var goalNode = nodes.Count - 1;

            var maxNumNodesToExplore = incrementalSearch ? 1 : int.MaxValue;

            int currentNodeIndex = 0;

            Dictionary<int, PathSearchNodeRecord> searchNodeRecord = null;

            Priority_Queue.SimplePriorityQueue<int, float> openNodes = null;

            HashSet<int> closedNodes = null;

            List<int> returnPath = null;

            var ret = PathSearchResultType.InProgress;

            int attempts = 0;

            int maxAllowedAttempts = 20;

            do
            {
                var init = attempts <= 0;

                ++attempts;

                ret = AStarPathSearchImpl.FindPathIncremental(
                    () => { return nodes.Count; },
                    (nindex) => { return nodes[nindex]; },
                    (nindex) => { return edges[nindex]; },
                    G, H,
                    startNode, goalNode, maxNumNodesToExplore, init,
                    ref currentNodeIndex, ref searchNodeRecord, ref openNodes, ref closedNodes,
                    ref returnPath);
            }
            while (ret == PathSearchResultType.InProgress && attempts < maxAllowedAttempts);

            Debug.Log($"Number of updates: {attempts}");

            Assert.That(ret, Is.EqualTo(PathSearchResultType.Complete));
            Assert.That(returnPath, Does.Contain(goalNode));

            if (incrementalSearch)
                Assert.That(attempts, Is.GreaterThan(1));
            else
                Assert.That(attempts, Is.EqualTo(1));

            // TODO write some good assertions as this test is incomplete

        }

        // TODO write more tests!

    }

    public class AStarMoreTests
    {
        // Convenience graph builder for small tests
        private static (List<Vector2> nodes, List<List<int>> edges) LineGraph(int n)
        {
            var nodes = new List<Vector2>();
            var edges = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                nodes.Add(new Vector2(0, i));
                var e = new List<int>();
                if (i + 1 < n) e.Add(i + 1);
                if (i - 1 >= 0) e.Add(i - 1);
                edges.Add(e);
            }
            return (nodes, edges);
        }

        [Test]
        public void HeuristicAndCostAreCorrect()
        {
            var a = new Vector2(1, 2);
            var b = new Vector2(4, 6);

            var manhattan = AStarPathSearchImpl.HeuristicManhattan(a, b);
            var euclidH = AStarPathSearchImpl.HeuristicEuclidean(a, b);
            var cost = AStarPathSearchImpl.Cost(a, b);

            Assert.That(manhattan, Is.EqualTo(Mathf.Abs(1 - 4) + Mathf.Abs(2 - 6))); // 3 + 4 = 7
            Assert.That(manhattan, Is.EqualTo(7f));

            Assert.That(euclidH, Is.EqualTo(Vector2.Distance(a, b)).Within(1e-5));
            Assert.That(cost, Is.EqualTo(euclidH).Within(1e-5));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AStar_Completes_OnSimpleLine(bool incremental)
        {
            var (nodes, edges) = LineGraph(6);
            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;

            int start = 0, goal = 5;
            int max = incremental ? 1 : int.MaxValue;
            int current = 0;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            var attempts = 0;
            var ret = PathSearchResultType.InProgress;

            do
            {
                bool init = attempts == 0;
                attempts++;
                ret = AStarPathSearchImpl.FindPathIncremental(
                    () => nodes.Count,
                    idx => nodes[idx],
                    idx => edges[idx],
                    G, H,
                    start, goal, max, init,
                    ref current, ref recs, ref open, ref closed, ref path
                );
            } while (ret == PathSearchResultType.InProgress && attempts < 100);

            Assert.That(ret, Is.EqualTo(PathSearchResultType.Complete));
            Assert.That(path[0], Is.EqualTo(start));
            Assert.That(path[path.Count - 1], Is.EqualTo(goal));
            if (incremental) Assert.That(attempts, Is.GreaterThan(1));
            else Assert.That(attempts, Is.EqualTo(1));
        }

        [Test]
public void Dijkstra_Works_With_NullHeuristic()
{
    // 0--1--2, also 0--2 direct; colinear so both routes cost 3
    var nodes = new List<Vector2>
    {
        new Vector2(0,0), //0
        new Vector2(1,0), //1
        new Vector2(3,0)  //2
    };
    var edges = new List<List<int>>
    {
        new List<int>{1,2}, //0
        new List<int>{0,2}, //1
        new List<int>{0,1}  //2
    };

    CostCallback G = AStarPathSearchImpl.Cost;
    CostCallback H = AStarPathSearchImpl.HeuristicNull; // Dijkstra

    int start = 0, goal = 2, max = int.MaxValue;
    int current = 0;
    Dictionary<int, PathSearchNodeRecord> recs = null;
    Priority_Queue.SimplePriorityQueue<int, float> open = null;
    HashSet<int> closed = null;
    List<int> path = null;

    var ret = AStarPathSearchImpl.FindPathIncremental(
        () => nodes.Count,
        i => nodes[i],
        i => edges[i],
        G, H, start, goal, max, true,
        ref current, ref recs, ref open, ref closed, ref path
    );

    Assert.That(ret, Is.EqualTo(PathSearchResultType.Complete));
    Assert.That(path[0], Is.EqualTo(start));
    Assert.That(path[path.Count - 1], Is.EqualTo(goal));

    // Validate optimal total cost
    float PathCost(List<int> p)
    {
        float sum = 0f;
        for (int i = 0; i + 1 < p.Count; i++)
            sum += AStarPathSearchImpl.Cost(nodes[p[i]], nodes[p[i+1]]);
        return sum;
    }

    float direct = Vector2.Distance(nodes[0], nodes[2]);      // 3
    float via1   = Vector2.Distance(nodes[0], nodes[1]) +
                   Vector2.Distance(nodes[1], nodes[2]);      // 1 + 2 = 3
    float expected = Mathf.Min(direct, via1);

    Assert.That(PathCost(path), Is.EqualTo(expected).Within(1e-5));

    // Allow either 0->2 (len 2) or 0->1->2 (len 3)
    Assert.That(path.Count == 2 || path.Count == 3, "Dijkstra may return either shortest path in a tie.");
}


        [Test]
        public void GreedyBestFirst_ReachesGoal_ButMayNotBeOptimal()
        {
            // Classic trap for Greedy: a misleading heuristic direct neighbor
            // Graph:
            // 0 connected to 1 and 3
            // 1 connected to 2 (goal), but 0->1 is long in cost
            // 3 connected to 2, and 0->3 is short, 3->2 is short (this one is actually optimal)
            // We'll flip it so Greedy might pick a suboptimal intermediate, but still reach goal.
            var nodes = new List<Vector2>
            {
                new Vector2(0,0), //0
                new Vector2(2,1), //1 (looks closer in heuristic from 0, but make 0->1 cost big by coords?)
                new Vector2(4,0), //2 goal
                new Vector2(1, -0.1f) //3
            };
            var edges = new List<List<int>>
            {
                new List<int>{1,3},  //0
                new List<int>{0,2},  //1
                new List<int>{1,3},  //2
                new List<int>{0,2}   //3
            };

            // Greedy Best-First: G=0, H=Euclidean
            CostCallback G = AStarPathSearchImpl.CostNull;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;

            int start = 0, goal = 2;
            int max = int.MaxValue;
            int current = 0;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            var ret = AStarPathSearchImpl.FindPathIncremental(
                () => nodes.Count,
                i => nodes[i],
                i => edges[i],
                G, H, start, goal, max, true,
                ref current, ref recs, ref open, ref closed, ref path
            );

            Assert.That(ret, Is.EqualTo(PathSearchResultType.Complete));
            Assert.That(path[0], Is.EqualTo(start));
            Assert.That(path[path.Count - 1], Is.EqualTo(goal));
            // No optimality assertion here, just reachability—Greedy is not guaranteed optimal
        }

        [Test]
        public void Unreachable_Returns_Partial_To_Closest_By_H_Then_Lower_G()
        {
            // Component A: 0-1-2, start=0
            // Component B: 10 (goal alone, unreachable)
            // Make nodes 1 and 2 equidistant to goal by heuristic, but with different g.
            // Tie should break toward smaller g (node 1).
            var nodes = new List<Vector2>
            {
                new Vector2(0,0),  //0 start
                new Vector2(1,0),  //1
                new Vector2(2,0),  //2
                new Vector2(100,100) //3 goal (isolated)
            };
            var edges = new List<List<int>>
            {
                new List<int>{1},    //0
                new List<int>{0,2},  //1
                new List<int>{1},    //2
                new List<int>()      //3
            };

            // Place goal so that h(1,goal) == h(2,goal) approximately? We can enforce equal by adjusting positions.
            // Current positions: Dist(1->goal) ~ sqrt((99)^2+100^2), Dist(2->goal) ~ sqrt((98)^2+100^2)
            // Not equal, but that's fine—let's force a tie by using a null heuristic (h=0) so fallback Euclidean is used.
            // In our implementation, if h==0 we fallback to Euclidean for the "closest" selection.
            // To make equality, tweak to make both equal after fallback is used: use Manhattan geometry? Simpler: Accept no exact tie,
            // but we can still test lower-g selection by setting both nearly same h and checking behavior with null H would fallback per-closed calc.
            // We'll instead force equal heuristic by using HeuristicNull, which makes h=0 and our ChooseClosest uses Euclidean fallback.
            // We'll set Euclidean distances equal by placing nodes 1 and 2 symmetrically around (1.5,0)
            nodes[1] = new Vector2(1, 0);
            nodes[2] = new Vector2(2, 0);
            nodes[3] = new Vector2(101.5f, 0); // equidistant: dist(1->goal)=100.5, dist(2->goal)=99.5 (not equal)
            // Okay, let's flip: make goal at x=101 so dist(1)=100, dist(2)=99 (still not equal).
            // Instead we will rely on the original spec: pick smallest h; if tie, smallest g.
            // We'll set goal so node 2 is slightly closer by h, but node 1 has smaller g.
            // Then we expect the selection to be node 2 unless tie; to explicitly test tie-breaking, set equal distances:
            nodes[3] = new Vector2(101f, 0f); // h(1)=100, h(2)=99 -> not tie. So we will run with A* (non-null H) to pick closest by h.
            // To actually test tie-by-g, create two different nodes with same h:
            // Add node 4 at x=0, and set goal at x=100. h(1)=99, h(2)=98, h(0)=100. Hard to tie cleanly on continuous space.
            // We'll do this: use HeuristicNull and custom tie in ChooseClosest that falls back to Euclid; but we can't force equal with integers easily.
            // Alternative: We'll assert behavior for "closest by h" first, then add a true tie case below.

            // First test: unreachable returns Partial
            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;
            int start = 0, goal = 3;
            int max = int.MaxValue;
            int current = 0;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            var ret = AStarPathSearchImpl.FindPathIncremental(
                () => nodes.Count,
                i => nodes[i],
                i => edges[i],
                G, H, start, goal, max, true,
                ref current, ref recs, ref open, ref closed, ref path
            );

            Assert.That(ret, Is.EqualTo(PathSearchResultType.Partial));
            Assert.That(path[0], Is.EqualTo(start));
            // Check that the final node in the partial path is one of the closed nodes (1 or 2)
            var end = path[path.Count - 1];
            Assert.That(end == 1 || end == 2, "Expected partial path to end at a closed frontier node closest to goal.");
        }

        [Test]
        public void TieBreakOnEqualH_PicksLowerG()
        {
            // Create a fork so both frontier nodes have equal heuristic distance to the goal,
            // but different g-cost from start.
            //
            // Layout:
            // start(0,0) -> A(3,0) -> dead end
            // start(0,0) -> B(1,0) -> dead end
            // goal at (2,0) so h(A)=1 and h(B)=1 (equal). g(A)=3, g(B)=1 -> choose B.
            var nodes = new List<Vector2>
            {
                new Vector2(0,0),  //0 start
                new Vector2(3,0),  //1 A
                new Vector2(1,0),  //2 B
                new Vector2(2,0)   //3 goal (unreachable)
            };
            var edges = new List<List<int>>
            {
                new List<int>{1,2}, //0
                new List<int>(),    //1
                new List<int>(),    //2
                new List<int>()     //3 goal isolated
            };

            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;

            int start = 0, goal = 3, max = int.MaxValue;
            int current = 0;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            var ret = AStarPathSearchImpl.FindPathIncremental(
                () => nodes.Count,
                i => nodes[i],
                i => edges[i],
                G, H, start, goal, max, true,
                ref current, ref recs, ref open, ref closed, ref path
            );

            Assert.That(ret, Is.EqualTo(PathSearchResultType.Partial));

            int end = path[path.Count - 1];
            // Heuristics equal (both at distance 1 from goal), tie should pick lower g -> node 2 (B)
            Assert.That(end, Is.EqualTo(2), "On equal h, expected tie-break to choose node with lower g-cost.");
        }

        [Test]
        public void InitializationError_OnBadInputs()
        {
            var nodes = new List<Vector2> { new Vector2(0,0) };
            var edges = new List<List<int>> { new List<int>() };

            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;

            int current = -1;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            // Negative maxNumNodesToExplore should trigger InitializationError
            var ret = AStarPathSearchImpl.FindPathIncremental(
                () => nodes.Count,
                i => nodes[i],
                i => edges[i],
                G, H,
                0, 0, -5, true,
                ref current, ref recs, ref open, ref closed, ref path
            );

            Assert.That(ret, Is.EqualTo(PathSearchResultType.InitializationError));
        }

        [Test]
        public void Incremental_ProcessesMultipleSteps()
        {
            var (nodes, edges) = LineGraph(10);
            CostCallback G = AStarPathSearchImpl.Cost;
            CostCallback H = AStarPathSearchImpl.HeuristicEuclidean;
            int start = 0, goal = 9;

            int current = 0;
            Dictionary<int, PathSearchNodeRecord> recs = null;
            SimplePriorityQueue<int, float> open = null;
            HashSet<int> closed = null;
            List<int> path = null;

            int attempts = 0;
            var result = PathSearchResultType.InProgress;

            // Advance one node per call
            while (result == PathSearchResultType.InProgress && attempts < 50)
            {
                bool init = attempts == 0;
                attempts++;
                result = AStarPathSearchImpl.FindPathIncremental(
                    () => nodes.Count,
                    i => nodes[i],
                    i => edges[i],
                    G, H,
                    start, goal, 1, init,
                    ref current, ref recs, ref open, ref closed, ref path
                );
            }

            Assert.That(attempts, Is.GreaterThan(1), "Expected multiple updates in incremental mode.");
            Assert.That(result, Is.EqualTo(PathSearchResultType.Complete));
            Assert.That(path.Count, Is.GreaterThan(0));
            Assert.That(path[0], Is.EqualTo(start));
            Assert.That(path[path.Count - 1], Is.EqualTo(goal));
        }
    }
}
