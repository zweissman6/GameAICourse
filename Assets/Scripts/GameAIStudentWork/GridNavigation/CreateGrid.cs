using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAICourse {

    public class CreateGrid
    {

        // Please change this string to your name
        public const string StudentAuthorName = "George P. Burdell ← Not your name, change it!";


        // Create(): Creates a grid lattice discretized space for navigation.
        // canvasOrigin: bottom left corner of navigable region in world coordinates
        // canvasWidth: width of navigable region in world dimensions
        // canvasHeight: height of navigable region in world dimensions
        // cellWidth: target cell width (of a grid cell) in world dimensions
        // obstacles: a list of collider obstacles
        // grid: an array of bools. row major. a cell is true if navigable, false otherwise
        // pathNodes: a list of graph nodes, centered on each cell
        // pathEdges: graph adjacency list for each graph node. cooresponding index of pathNodes to match
        //      node with its edge list. All nodes must have an edge list (no null list)
        //      entries in each edge list are indices into pathNodes
        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight, float cellWidth,
        List<Obstacle> obstacles,
        out bool[,] grid,
        out List<Vector2> pathNodes,
        out List<List<int>> pathEdges
        )
        {
            grid = new bool[1, 1];
            grid[0, 0] = true;

            pathNodes = new List<Vector2>();

            //ignoring the obstacles for this limited demo; use various Util and Obstacle namespace
            //methods for calculating geometric overlap that makes a cell not navigable
            //Carefully consider all possible geometry interactions

            //also ignoring the world boundary defined by canvasOrigin and canvasWidth and canvasHeight
            //but should be treated similarly to other obstacles!

            //example of node placed in center of cell
            pathNodes.Add(canvasOrigin + new Vector2(cellWidth / 2f, cellWidth / 2f));

            //initalization of a path edge that corresponds to same index pathNode
            pathEdges = new List<List<int>>();

            //only one node, so can't be connected to anything, but we still initialize
            //to an empty list. Null not allowed!
            pathEdges.Add(new List<int>());

        }


    }

}