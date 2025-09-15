// Remove the line above if you are subitting to GradeScope for a grade. But leave it if you only want to check
// that your code compiles and the autograder can access your public methods.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAICourse {

    public class CreateGrid
    {

        // Please change this string to your name
        public const string StudentAuthorName = "Zachary Weissman";


        // Helper method provided to help you implement this file. Leave as is.
        // Returns true if point p is inside (or on edge) the polygon defined by pts (CCW winding). False, otherwise
        static bool IsPointInsidePolygon(Vector2Int[] pts, Vector2Int p)
        {
            return CG.InPoly1(pts, p) != CG.PointPolygonIntersectionType.Outside;
        }


        // Helper method provided to help you implement this file. Leave as is.
        // Returns float converted to int according to default scaling factor (1000)
        static int Convert(float v)
        {
            return CG.Convert(v);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns Vector2 converted to Vector2Int according to default scaling factor (1000)
        static Vector2Int Convert(Vector2 v)
        {
            return CG.Convert(v);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns true if segment AB intersects CD properly or improperly
        static bool Intersects(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int d)
        {
            return CG.Intersect(a, b, c, d);
        }


        // IsPointInsideAxisAlignedBoundingBox(): Determines whether a point (Vector2Int:p) is On/Inside a bounding box (such as a grid cell) defined by
        // minCellBounds and maxCellBounds (both Vector2Int's).
        // Returns true if the point is ON/INSIDE the cell and false otherwise
        // This method should return true if the point p is on one of the edges of the cell.
        // This is more efficient than PointInsidePolygon() for an equivalent dimension poly
        // Preconditions: minCellBounds <= maxCellBounds, per dimension
        static bool IsPointInsideAxisAlignedBoundingBox(Vector2Int minCellBounds, Vector2Int maxCellBounds, Vector2Int p)
        {
            return p.x >= minCellBounds.x && p.x <= maxCellBounds.x && p.y >= minCellBounds.y && p.y <= maxCellBounds.y;
        }




        // IsRangeOverlapping(): Determines if the range (inclusive) from min1 to max1 overlaps the range (inclusive) from min2 to max2.
        // The ranges are considered to overlap if one or more values is within the range of both.
        // Returns true if overlap, false otherwise.
        // Preconditions: min1 <= max1 AND min2 <= max2
        static bool IsRangeOverlapping(int min1, int max1, int min2, int max2)
        {
            return !(max1 < min2 || max2 < min1);
        }

        // IsAxisAlignedBoundingBoxOverlapping(): Determines if the AABBs defined by min1,max1 and min2,max2 overlap or touch
        // Returns true if overlap, false otherwise.
        // Preconditions: min1 <= max1, per dimension. min2 <= max2 per dimension
        static bool IsAxisAlignedBoundingBoxOverlapping(Vector2Int min1, Vector2Int max1, Vector2Int min2, Vector2Int max2)
        {

            return IsRangeOverlapping(min1.x, max1.x, min2.x, max2.x) && IsRangeOverlapping(min1.y, max1.y, min2.y, max2.y);
        }





        // IsTraversable(): returns true if the grid is traversable from grid[x,y] in the direction dir, false otherwise.
        // The grid boundaries are not traversable. If the grid position x,y is itself not traversable but the grid cell in direction
        // dir is traversable, the function will return false.
        // returns false if the grid is null, grid rank is not 2 dimensional, or any dimension of grid is zero length
        // returns false if x,y is out of range
        // Note: public methods are autograded
        public static bool IsTraversable(bool[,] grid, int x, int y, TraverseDirection dir)
        {
            //edge cases
            if (grid == null || grid.Rank != 2) return false;
            int sizeX = grid.GetLength(0);
            int sizeY = grid.GetLength(1);
            if (sizeX == 0 || sizeY == 0) return false;
            if (x < 0 || x >= sizeX || y < 0 || y >= sizeY) return false;
            if (!grid[x, y]) return false;

            //directions
            int dx = 0;
            int dy = 0;
            switch (dir)
            {
                case TraverseDirection.Up: dy = 1; break;
                case TraverseDirection.Down: dy = -1; break;
                case TraverseDirection.Left: dx = -1; break;
                case TraverseDirection.Right: dx = 1; break;
                case TraverseDirection.UpLeft: dx = -1; dy = 1; break;
                case TraverseDirection.UpRight: dx = 1; dy = 1; break;
                case TraverseDirection.DownLeft: dx = -1; dy = -1; break;
                case TraverseDirection.DownRight: dx = 1; dy = -1; break;
                default: return false;
            }
            int newX = x + dx;
            int newY = y + dy;

            //check boundaries
            if (newX < 0 || newY < 0 || newX >= sizeX || newY >= sizeY) return false;

            return grid[newX, newY];
        }


        // Create(): Creates a grid lattice discretized space for navigation.
        // canvasOrigin: bottom left corner of navigable region in world coordinates
        // canvasWidth: width of navigable region in world dimensions
        // canvasHeight: height of navigable region in world dimensions
        // cellWidth: target cell width (of a grid cell) in world dimensions
        // obstacles: a list of collider obstacles
        // grid: an array of bools. A cell is true if navigable, false otherwise
        //    Example: grid[x_pos, y_pos]

        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight, float cellWidth,
            List<Polygon> obstacles,
            out bool[,] grid
            )
        {
            // ignoring the obstacles for this limited demo; 
            // Marks cells of the grid untraversable if geometry intersects interior!
            // Carefully consider all possible geometry interactions

            // also ignoring the world boundary defined by canvasOrigin and canvasWidth and canvasHeight

            //grid sizes
            int gridSizeX = Mathf.Max(1, Mathf.FloorToInt(canvasWidth / cellWidth));
            int gridSizeY = Mathf.Max(1, Mathf.FloorToInt(canvasHeight / cellWidth));

            grid = new bool[gridSizeX, gridSizeY];

            //shrink
            Vector2Int shrink = new Vector2Int(1, 1);


            //iterate over all cells
            for (int ix = 0; ix < gridSizeX; ix++)
            {
                for (int iy = 0; iy < gridSizeY; iy++)
                {
                    Vector2 cellMinWorld = new Vector2(canvasOrigin.x + ix * cellWidth, canvasOrigin.y + iy * cellWidth);
                    Vector2 cellMaxWorld = new Vector2(canvasOrigin.x + (ix + 1) * cellWidth, canvasOrigin.y + (iy + 1) * cellWidth);
                    Vector2Int cellMin = Convert(cellMinWorld);
                    Vector2Int cellMax = Convert(cellMaxWorld);
                    //shrink aabbs for interior not edges
                    Vector2Int innerMin = new Vector2Int(cellMin.x + shrink.x, cellMin.y + shrink.y);
                    Vector2Int innerMax = new Vector2Int(cellMax.x - shrink.x, cellMax.y - shrink.y);
                    if (innerMin.x > innerMax.x) innerMin.x = innerMax.x = (cellMin.x + cellMax.x) / 2;
                    if (innerMin.y > innerMax.y) innerMin.y = innerMax.y = (cellMin.y + cellMax.y) / 2;

                    //box corners:
                    Vector2Int a = innerMin;
                    Vector2Int b = new Vector2Int(innerMax.x, innerMin.y);
                    Vector2Int c = innerMax;
                    Vector2Int d = new Vector2Int(innerMin.x, innerMax.y);
                    //center
                    Vector2Int center = new Vector2Int((innerMin.x + innerMax.x) / 2, (innerMin.y + innerMax.y) / 2);

                    //set up obstacle blockages:
                    bool blocked = false;
                    if (obstacles != null && obstacles.Count > 0)
                    {
                        for (int oi = 0; oi < obstacles.Count && !blocked; oi++)
                        {
                            Polygon poly = obstacles[oi];
                            Vector2Int omin = poly.MinIntBounds;
                            Vector2Int omax = poly.MaxIntBounds;
                            //compare obstacleAABB to shrunkencell AABB
                            if (!IsAxisAlignedBoundingBoxOverlapping(innerMin, innerMax, omin, omax))
                            {
                                continue;
                            }
                            //compare polygon points to shrunken cell (obstacle is inside cell)
                            Vector2Int[] pts = poly.getIntegerPoints();
                            for (int k = 0; k < pts.Length && !blocked; k++)
                            {
                                if (IsPointInsideAxisAlignedBoundingBox(innerMin, innerMax, pts[k]))
                                {
                                    blocked = true;
                                    break;
                                }
                            }
                            //compare shrunken cell corners/center to polygon (obstacles takes up cell)
                            if (!blocked)
                            {
                                if (IsPointInsidePolygon(pts, a) || IsPointInsidePolygon(pts, b) || IsPointInsidePolygon(pts, c) || IsPointInsidePolygon(pts, d) || IsPointInsidePolygon(pts, center))
                                {
                                    blocked = true;
                                }
                            }
                            //compare polygon edges to shrunken cell edges
                            if (!blocked)
                            {
                                Vector2Int edge1a = a, edge1b = b; //edge 1: a to b
                                Vector2Int edge2a = b, edge2b = c; //edge 2: b to c
                                Vector2Int edge3a = c, edge3b = d; //edge 3: c to d
                                Vector2Int edge4a = d, edge4b = a; //edge 4: d to a
                                for (int k = 0; k < pts.Length && !blocked; k++)
                                {
                                    Vector2Int p0 = pts[k];
                                    Vector2Int p1 = pts[(k + 1) % pts.Length];
                                    if (Intersects(p0, p1, edge1a, edge1b) || Intersects(p0, p1, edge2a, edge2b) || Intersects(p0, p1, edge3a, edge3b) || Intersects(p0, p1, edge4a, edge4b))
                                    {
                                        blocked = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    grid[ix, iy] = !blocked;
                }
            }
        }

    }

}
