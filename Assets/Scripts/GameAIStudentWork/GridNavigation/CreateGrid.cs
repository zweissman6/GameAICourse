using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAICourse {

    public class CreateGrid
    {

        // Please change this string to your name
        public const string StudentAuthorName = "George P. Burdell ← Not your name, change it!";


        // Helper method provided to help you implement this file. Leave as is.
        // Returns true if point p is inside (or on edge) the polygon defined by pts (CCW winding). False, otherwise
        public static bool IsPointInsidePolygon(Vector2Int[] pts, Vector2Int p)
        {
            return CG.InPoly1(pts, p) != CG.PointPolygonIntersectionType.Outside;
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns float converted to int according to default scaling factor (1000)
        public static int Convert(float v)
        {
            return CG.Convert(v);
        }


        // Helper method provided to help you implement this file. Leave as is.
        // Returns Vector2 converted to Vector2Int according to default scaling factor (1000)
        public static Vector2Int Convert(Vector2 v)
        {
            return CG.Convert(v);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns true is segment AB intersects CD properly or improperly
        static public bool Intersects(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int d)
        {
            return CG.Intersect(a, b, c, d);
        }


        // PointInsideBoundingBox(): Determines whether a point (Vector2Int:p) is On/Inside a bounding box (such as a grid cell) defined by
        // minCellBounds and maxCellBounds (both Vector2Int's).
        // Returns true if the point is ON/INSIDE the cell and false otherwise
        // This method should return true if the point p is on one of the edges of the cell.
        // This is more efficient than PointInsidePolygon() for an equivalent dimension poly
        public static bool PointInsideBoundingBox(Vector2Int minCellBounds, Vector2Int maxCellBounds, Vector2Int p)
        {
            //TODO IMPLEMENT

            // placeholder logic to be replaced by the student
            return true;
        }


        // Istraversable(): returns true if the grid is traversable from grid[x,y] in the direction dir, false otherwise.
        // The grid boundaries are not traversable. If the grid position x,y is itself not traversable but the grid cell in direction
        // dir is traversable, the function will return false.
        // returns false if the grid is null, grid rank is not 2 dimensional, or any dimension of grid is zero length
        // returns false if x,y is out of range
        public static bool Istraversable(bool[,] grid, int x, int y, TraverseDirection dir, GridConnectivity conn)
        {

            // TODO IMPLEMENT

            //placeholder logic to be replaced by the student
            return true;
        }


        // CreatePathNetworkFromGrid(): Creates a path network from a grid according to traversability
        // from one node to an adjacent node. Each node should be centered in the cell.
        // Edges from A to B should always have a matching B to A edge
        // pathNodes: a list of graph nodes, centered on each cell
        // pathEdges: graph adjacency list for each graph node. cooresponding index of pathNodes to match
        //      node with its edge list. All nodes must have an edge list (no null list)
        //      entries in each edge list are indices into pathNodes
        public static void CreatePathGraphFromGrid(
            Vector2 canvasOrigin, float canvasWidth, float canvasHeight, float cellWidth,
            GridConnectivity conn,
            bool[,] grid, out List<Vector2> pathNodes, out List<List<int>> pathEdges
            )
        {

            if (grid == null || grid.Rank != 2)
            {
                pathNodes = new List<Vector2>();
                pathEdges = new List<List<int>>();
                return;
            }

            // TODO IMPLEMENT


            pathNodes = new List<Vector2>();


            //example of node placed in center of cell
            pathNodes.Add(canvasOrigin + new Vector2(cellWidth / 2f, cellWidth / 2f));

            //initalization of a path edge that corresponds to same index pathNode
            pathEdges = new List<List<int>>();

            //only one node, so can't be connected to anything, but we still initialize
            //to an empty list. Null not allowed!
            pathEdges.Add(new List<int>());

        }

        // Create(): Creates a grid lattice discretized space for navigation.
        // canvasOrigin: bottom left corner of navigable region in world coordinates
        // canvasWidth: width of navigable region in world dimensions
        // canvasHeight: height of navigable region in world dimensions
        // cellWidth: target cell width (of a grid cell) in world dimensions
        // obstacles: a list of collider obstacles
        // grid: an array of bools. row major. a cell is true if navigable, false otherwise

        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight, float cellWidth,
            List<Polygon> obstacles,
            out bool[,] grid
            )
        {
            // ignoring the obstacles for this limited demo; 
            // Marks cells of the grid untraversable if geometry intersects interior!
            // Carefully consider all possible geometry interactions

            // also ignoring the world boundary defined by canvasOrigin and canvasWidth and canvasHeight


            grid = new bool[1, 1];
            grid[0, 0] = true;


        }

    }

}