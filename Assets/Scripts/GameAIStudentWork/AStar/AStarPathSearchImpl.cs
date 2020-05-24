using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;


namespace GameAICourse
{


    public class AStarPathSearchImpl
    {

        // Please change this string to your name
        public const string StudentAuthorName = "George P. Burdell ← Not your name, change it!";

        // Heuristic Function
        public delegate float HCallback(Vector2 a, Vector2 b);
        // G Cost so far function
        public delegate float GCallback(Vector2 a, Vector2 b);


        public static float Heuristic(Vector2 nodeA, Vector2 nodeB)
        {
            //STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation

            return 0f;

            //END CODE 
        }
        public static float Cost(Vector2 nodeA, Vector2 nodeB)
        {
            //STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation

            return 0f;

            //END STUDENT CODE
        }

        public static PathSearchResultType FindPathIncremental(List<Vector2> nodes, List<List<int>> edges,
        GCallback G,
        HCallback H,
        int startNodeIndex, int goalNodeIndex,
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

                return PathSearchResultType.InitializationError;


            // STUDENT CODE HERE

            // The following code is just a placeholder so that the method has a valid return
            // You will replace it with the correct implementation

            pathResult = PathSearchResultType.Complete;

            searchNodeRecords = new Dictionary<int, PathSearchNodeRecord>();
            openNodes = new SimplePriorityQueue<int, float>();
            closedNodes = new HashSet<int>();

            returnPath = new List<int>();

            return pathResult;

            //END STUDENT CODE HERE
        }

     
    }

}