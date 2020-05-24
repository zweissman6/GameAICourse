using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

using GameAICourse;

public class GreedyBestFirstPathSearch : PathSearchProvider
{

    private static PathSearchProvider instance;
    public static PathSearchProvider Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GreedyBestFirstPathSearch();
            }
            return instance;
        }
    }



    override public PathSearchResultType FindPathIncremental(List<Vector2> nodes, List<List<int>> edges,
    int startNodeIndex, int goalNodeIndex, int maxNumNodesToExplore, bool doInitialization, ref int currentNodeIndex, ref Dictionary<int, PathSearchNodeRecord> searchNodeRecords, ref SimplePriorityQueue<int, float> openNodes, ref HashSet<int> closedNodes, ref List<int> returnPath)
    {

        AStarPathSearchImpl.GCallback NullG = (Vector2 a, Vector2 b) => 0f;

        return AStarPathSearchImpl.FindPathIncremental(nodes, edges, NullG, AStarPathSearchImpl.Heuristic, startNodeIndex, goalNodeIndex, maxNumNodesToExplore, doInitialization,
            ref currentNodeIndex, ref searchNodeRecords, ref openNodes, ref closedNodes, ref returnPath);

    }

}
