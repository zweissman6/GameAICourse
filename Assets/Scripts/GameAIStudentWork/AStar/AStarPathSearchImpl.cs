// Remove the line above if you are subitting to GradeScope for a grade. But leave it if you only want to check
// that your code compiles and the autograder can access your public methods.

using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;


namespace GameAICourse
{


    public class AStarPathSearchImpl
    {

        // Please change this string to your name
        public const string StudentAuthorName = "Zachary Weissman";


        // Null Heuristic for Dijkstra
        public static float HeuristicNull(Vector2 nodeA, Vector2 nodeB)
        {
            return 0f;
        }

        // Null Cost for Greedy Best First
        public static float CostNull(Vector2 nodeA, Vector2 nodeB)
        {
            return 0f;
        }



        // Heuristic distance fuction implemented with manhattan distance
        public static float HeuristicManhattan(Vector2 nodeA, Vector2 nodeB)
        {
            //STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation
            return Mathf.Abs(nodeA.x - nodeB.x) + Mathf.Abs(nodeA.y - nodeB.y);

            //END CODE 
        }

        // Heuristic distance function implemented with Euclidean distance
        public static float HeuristicEuclidean(Vector2 nodeA, Vector2 nodeB)
        {
            //STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation
            return Vector2.Distance(nodeA, nodeB);

            //END CODE 
        }


        // Cost is only ever called on adjacent nodes. So we will always use Euclidean distance.
        // We could use Manhattan dist for 4-way connected grids and avoid sqrroot and mults.
        // But we will avoid that for simplicity.
        public static float Cost(Vector2 nodeA, Vector2 nodeB)
        {
            //STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation
            return Vector2.Distance(nodeA, nodeB);

            //END STUDENT CODE
        }

        //helper method for choosing closest closed node to the goal
        private static int ChooseClosest(int goalIndex, ref HashSet<int> closed, ref Dictionary<int, PathSearchNodeRecord> records, GetNode getNode)
        {
            int best = -1;
            float bestH = float.PositiveInfinity;
            float bestG = float.PositiveInfinity;
            Vector2 goalPosition = getNode(goalIndex);

            foreach (var i in closed)
            {
                if (!records.ContainsKey(i)) continue;
                var r = records[i];
                float h = Mathf.Max(0f, r.EstimatedTotalCost - r.CostSoFar);

                //for h == 0, just use Euclidean
                if (h == 0f)
                {
                    h = HeuristicEuclidean(getNode(i), goalPosition);
                }
                if (h < bestH - Mathf.Epsilon)
                {
                    bestH = h;
                    bestG = r.CostSoFar;
                    best = i;
                }
                else if (Mathf.Abs(h - bestH) <= Mathf.Epsilon)
                {
                    //this is a tie
                    if (r.CostSoFar < bestG - Mathf.Epsilon)
                    {
                        bestG = r.CostSoFar;
                        best = i;
                    }
                }
            }
            return best;
        }

        //helper to reconstruct path from endIndex to start
        private static void Reconstruct(int startIndex, int endIndex, ref Dictionary<int, PathSearchNodeRecord> records, ref List<int> path)
        {
            path = new List<int>();
            int curr = endIndex;
            while (true)
            {
                path.Add(curr);
                if (curr == startIndex) break;
                curr = records[curr].FromNodeIndex;
            }
            path.Reverse();

        }



        public static PathSearchResultType FindPathIncremental(
            GetNodeCount getNodeCount,
            GetNode getNode,
            GetNodeAdjacencies getAdjacencies,
            CostCallback G,
            CostCallback H,
            int startNodeIndex, int goalNodeIndex,
            int maxNumNodesToExplore, bool doInitialization,
            ref int currentNodeIndex,
            ref Dictionary<int, PathSearchNodeRecord> searchNodeRecords,
            ref SimplePriorityQueue<int, float> openNodes, ref HashSet<int> closedNodes, ref List<int> returnPath)
        {
            PathSearchResultType pathResult = PathSearchResultType.InProgress;

            var nodeCount = getNodeCount();

            if (startNodeIndex >= nodeCount || goalNodeIndex >= nodeCount ||
                startNodeIndex < 0 || goalNodeIndex < 0 ||
                maxNumNodesToExplore <= 0 ||
                (!doInitialization &&
                 (openNodes == null || closedNodes == null || currentNodeIndex < 0 ||
                  currentNodeIndex >= nodeCount)))

                return PathSearchResultType.InitializationError;


            // STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation

            //initialize
            if (doInitialization || openNodes == null || closedNodes == null || searchNodeRecords == null)
            {
                searchNodeRecords = new Dictionary<int, PathSearchNodeRecord>();
                openNodes = new SimplePriorityQueue<int, float>();
                closedNodes = new HashSet<int>();
                returnPath = null;

                //start node
                var startPosition = getNode(startNodeIndex);
                var goalPosition = getNode(goalNodeIndex);
                float g0 = 0f;
                float h0 = Mathf.Max(0f, H(startPosition, goalPosition));
                var startRec = new PathSearchNodeRecord(startNodeIndex, startNodeIndex, g0, g0 + h0);

                searchNodeRecords[startNodeIndex] = startRec;
                openNodes.Enqueue(startNodeIndex, startRec.EstimatedTotalCost);
                currentNodeIndex = startNodeIndex;
            }

            int nodesProcessed = 0;
            //process up to N nodes:
            while (nodesProcessed < maxNumNodesToExplore)
            {
                if (openNodes.Count == 0)
                {
                    //no open nodes means we need to attempt to build a path to the node closest to the goal
                    int closest = ChooseClosest(goalNodeIndex, ref closedNodes, ref searchNodeRecords, getNode);
                    //no closest:
                    if (closest == -1)
                    {
                        closest = startNodeIndex;
                    }

                    //closest node should have a record
                    if (!searchNodeRecords.ContainsKey(closest))
                    {
                        //create a record
                        var p = getNode(closest);
                        var goalPosition = getNode(goalNodeIndex);
                        float g = 0f;
                        float h = Mathf.Max(0f, H(p, goalPosition));
                        searchNodeRecords[closest] = new PathSearchNodeRecord(closest, closest, g, g + h);
                    }

                    Reconstruct(startNodeIndex, closest, ref searchNodeRecords, ref returnPath);
                    pathResult = PathSearchResultType.Partial;
                    return pathResult;
                }

                //otherwise, use lowest f = g + h
                int curr = openNodes.Dequeue();
                currentNodeIndex = curr;
                if (currentNodeIndex == goalNodeIndex)
                {
                    Reconstruct(startNodeIndex, goalNodeIndex, ref searchNodeRecords, ref returnPath);
                    pathResult = PathSearchResultType.Complete;
                    return pathResult;
                }

                closedNodes.Add(curr);
                //work into neighbors:
                var currentRecord = searchNodeRecords[curr];
                var currentPostion = getNode(curr);
                var goalPosition2 = getNode(goalNodeIndex);
                var neighbors = getAdjacencies(curr);
                if (neighbors != null)
                {
                    foreach (var n in neighbors)
                    {
                        if (n < 0 || n >= nodeCount) continue;

                        var nPosition = getNode(n);
                        float gNew = currentRecord.CostSoFar + Mathf.Max(0f, G(currentPostion, nPosition));
                        float hNew = Mathf.Max(0f, H(nPosition, goalPosition2));
                        float fNew = gNew + hNew;

                        bool inClosed = closedNodes.Contains(n);
                        bool inOpen = openNodes.Contains(n);

                        //create record if needed
                        if (!searchNodeRecords.ContainsKey(n))
                        {
                            searchNodeRecords[n] = new PathSearchNodeRecord(n, curr, gNew, fNew);
                            openNodes.Enqueue(n, fNew);
                        }
                        else
                        {
                            var existingRecord = searchNodeRecords[n];
                            if (gNew + 1e-6f < existingRecord.CostSoFar)
                            {
                                //update record if better
                                existingRecord.CostSoFar = gNew;
                                existingRecord.FromNodeIndex = curr;
                                existingRecord.EstimatedTotalCost = fNew;
                                searchNodeRecords[n] = existingRecord;

                                if (inOpen)
                                {
                                    openNodes.UpdatePriority(n, fNew);
                                }
                                else if (inClosed)
                                {
                                    closedNodes.Remove(n);
                                    openNodes.Enqueue(n, fNew);
                                }
                                else
                                {
                                    openNodes.Enqueue(n, fNew); //fallback
                                }
                            } //worse path can be ignored
                        }
                    }
                }
                nodesProcessed++;
            }

            pathResult = PathSearchResultType.InProgress;
            return pathResult;

            //END STUDENT CODE HERE
        }

    }

}