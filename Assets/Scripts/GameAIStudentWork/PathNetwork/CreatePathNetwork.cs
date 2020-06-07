using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAICourse
{

    public class CreatePathNetwork
    {

        public const string StudentAuthorName = "George P. Burdell ← Not your name, change it!";

        //Student code to build the path network from the given pathNodes and Obstacles
        //Obstacles - List of obstacles on the plane
        //agentWidth - the width of the traversing agent
        //pathEdges - out parameter that will contain the edges you build.
        //  Edges cannot intersect with obstacles. Edges must be at least agentRadius distance
        //  from all obstacle/boundary line segments

        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
            List<Obstacle> obstacles, float agentRadius, List<Vector2> pathNodes, out List<List<int>> pathEdges)
        {

            //STUDENT CODE HERE

            // Uncomment the following two lines if you want to see solution output
            // This is assuming you don't modify any of the presets provided, including dragging objects
            // If the preset doesn't match perfectly, a null solution is presented

            //HardCodedPathNetworkDemo.Create(canvasOrigin, canvasWidth, canvasHeight, obstacles, agentRadius,
            //    pathNodes, out pathEdges);

            //return;

            pathEdges = new List<List<int>>(pathNodes.Count);

            for (int i = 0; i < pathNodes.Count; ++i)
            {
                pathEdges.Add( new List<int>() );
            }


            // END STUDENT CODE

        }


    }

}