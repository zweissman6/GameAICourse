using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

using GameAICourse;

public class AStarPathSearch : PathSearchProvider
{

    private static PathSearchProvider instance;
    public static PathSearchProvider Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AStarPathSearch();
            }
            return instance;
        }
    }
    override public PathSearchResultType FindPathIncremental(List<Vector2> nodes, List<List<int>> edges,
        int startNodeIndex, int goalNodeIndex, int maxNumNodesToExplore, bool doInitialization, ref int currentNodeIndex, ref Dictionary<int, PathSearchNodeRecord> searchNodeRecords, ref SimplePriorityQueue<int, float> openNodes, ref HashSet<int> closedNodes, ref List<int> returnPath)
    {

        return AStarPathSearchImpl.FindPathIncremental(nodes, edges, AStarPathSearchImpl.Cost, AStarPathSearchImpl.Heuristic, startNodeIndex, goalNodeIndex, maxNumNodesToExplore, doInitialization,
            ref currentNodeIndex, ref searchNodeRecords, ref openNodes, ref closedNodes, ref returnPath);
            
    }
}
