using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
public class BasicPathSearchImpl
{
    static float G(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }
    public static PathSearchResultType FindPathIncremental(List<Vector2> nodes, List<List<int>> edges,
       int startNodeIndex, int goalNodeIndex,
       bool IsBFS, //true for BFS, false for DFS
       int maxNumNodesToExplore, bool doInitialization,
       ref int currentNodeIndex,
       ref Dictionary<int, PathSearchNodeRecord> searchNodeRecords,
       ref SimplePriorityQueue<int, float> openNodes, ref HashSet<int> closedNodes, ref List<int> returnPath)
    {
        PathSearchResultType pathResult = PathSearchResultType.InProgress;
        if (nodes == null || startNodeIndex >= nodes.Count || goalNodeIndex >= nodes.Count ||
            edges == null || startNodeIndex >= edges.Count || goalNodeIndex >= edges.Count ||
            edges.Count != nodes.Count ||
            startNodeIndex < 0 || goalNodeIndex < 0 ||
            maxNumNodesToExplore <= 0 ||
            (!doInitialization &&
             (openNodes == null || closedNodes == null || currentNodeIndex < 0 ||
              currentNodeIndex >= nodes.Count || currentNodeIndex >= edges.Count)))
        {
            searchNodeRecords = new Dictionary<int, PathSearchNodeRecord>();
            openNodes = new SimplePriorityQueue<int, float>();
            closedNodes = new HashSet<int>();

            return PathSearchResultType.InitializationError;
        }

        float max_dfs_priority = Mathf.Pow(2f, 20f);

        if (doInitialization)
        {
            currentNodeIndex = startNodeIndex;
            searchNodeRecords = new Dictionary<int, PathSearchNodeRecord>();
            openNodes = new SimplePriorityQueue<int, float>();
            closedNodes = new HashSet<int>();
            var firstNodeRecord = new PathSearchNodeRecord(currentNodeIndex);
            searchNodeRecords.Add(firstNodeRecord.NodeIndex, firstNodeRecord);
            float startingPriority = IsBFS ? 0f : max_dfs_priority; 
            openNodes.Enqueue(firstNodeRecord.NodeIndex, startingPriority);
            returnPath = new List<int>();
        }

        float currentPriority = 0f;

        if (openNodes.Count > 0)
            currentPriority = openNodes.GetPriority(openNodes.First);


        int nodesProcessed = 0;
        while (nodesProcessed < maxNumNodesToExplore && openNodes.Count > 0)
        {
            //Find the smallest element in the open list using the estimated total cost
            var currentNodeRecord = searchNodeRecords[openNodes.First]; 
            currentNodeIndex = currentNodeRecord.NodeIndex;

            currentPriority = IsBFS ? currentPriority + 1f : currentPriority - 1f;

            ++nodesProcessed;
         
            if (currentNodeIndex == goalNodeIndex)
                break;

            PathSearchNodeRecord edgeNodeRecord;
   
            foreach (var edgeNodeIndex in edges[currentNodeIndex])
            {
                var costToEdgeNode = currentNodeRecord.CostSoFar +
                    G(nodes[currentNodeIndex], nodes[edgeNodeIndex]);


                if (closedNodes.Contains(edgeNodeIndex) || openNodes.Contains(edgeNodeIndex))
                {
                    continue;
                }
                else
                {
                    edgeNodeRecord = new PathSearchNodeRecord(edgeNodeIndex);      
                }

                edgeNodeRecord.FromNodeIndex = currentNodeIndex;
         
                searchNodeRecords[edgeNodeIndex] = edgeNodeRecord;

                if (!openNodes.Contains(edgeNodeIndex))     
                    openNodes.Enqueue(edgeNodeIndex, currentPriority); 
                
            } //foreach() edge processing of current node

            openNodes.Remove(currentNodeIndex);
            closedNodes.Add(currentNodeIndex);
        } //while
        if (openNodes.Count <= 0 && currentNodeIndex != goalNodeIndex)
        {
            pathResult = PathSearchResultType.Partial;
            //find the closest node we looked at and use for partial path
            int closest = -1;
            float closestDist = float.MaxValue;
            foreach (var n in closedNodes)
            {
                var nrec = searchNodeRecords[n];
                var d = Vector2.Distance(nodes[nrec.NodeIndex], nodes[goalNodeIndex]);
                if (d < closestDist)
                {
                    closest = n;
                    closestDist = d;
                }
            }
            if (closest >= 0)
            {
                currentNodeIndex = closest;
            }
        }
        else if (currentNodeIndex == goalNodeIndex)
        {
            pathResult = PathSearchResultType.Complete;
        }


        if (pathResult != PathSearchResultType.InProgress)
        {
            // processing complete, a path (possibly partial) can be generated returned
            returnPath = new List<int>();
            while (currentNodeIndex != startNodeIndex)
            {
                returnPath.Add(currentNodeIndex);
                currentNodeIndex = searchNodeRecords[currentNodeIndex].FromNodeIndex;
            }
            returnPath.Add(startNodeIndex);
            returnPath.Reverse();
        }

        return pathResult;

    }
}
