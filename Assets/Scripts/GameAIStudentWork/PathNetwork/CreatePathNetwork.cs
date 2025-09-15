// Remove the line above if you are subitting to GradeScope for a grade. But leave it if you only want to check
// that your code compiles and the autograder can access your public methods.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAICourse
{

    public class CreatePathNetwork
    {

        public const string StudentAuthorName = "Zachary Weissman";




        // Helper method provided to help you implement this file. Leave as is.
        // Returns Vector2 converted to Vector2Int according to default scaling factor (1000)
        public static Vector2Int ConvertToInt(Vector2 v)
        {
            return CG.Convert(v);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns float converted to int according to default scaling factor (1000)
        public static int ConvertToInt(float v)
        {
            return CG.Convert(v);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns Vector2Int converted to Vector2 according to default scaling factor (1000)
        public static Vector2 ConvertToFloat(Vector2Int v)
        {
            float f = 1f / (float)CG.FloatToIntFactor;
            return new Vector2(v.x * f, v.y * f);
        }

        // Helper method provided to help you implement this file. Leave as is.
        // Returns int converted to float according to default scaling factor (1000)
        public static float ConvertToFloat(int v)
        {
            float f = 1f / (float)CG.FloatToIntFactor;
            return v * f;
        }


        // Helper method provided to help you implement this file. Leave as is.
        // Returns true is segment AB intersects CD properly or improperly
        static public bool Intersects(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int d)
        {
            return CG.Intersect(a, b, c, d);
        }


        //Get the shortest distance from a point to a line
        //Line is defined by the lineStart and lineEnd points
        public static float DistanceToLineSegment(Vector2Int point, Vector2Int lineStart, Vector2Int lineEnd)
        {
            return CG.DistanceToLineSegment(point, lineStart, lineEnd);
        }


        //Get the shortest distance from a point to a line
        //Line is defined by the lineStart and lineEnd points
        public static float DistanceToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            return CG.DistanceToLineSegment(point, lineStart, lineEnd);
        }


        // Helper method provided to help you implement this file. Leave as is.
        // Determines if a point is inside/on a CCW polygon and if so returns true. False otherwise.
        public static bool IsPointInPolygon(Vector2Int[] polyPts, Vector2Int point)
        {
            return CG.PointPolygonIntersectionType.Outside != CG.InPoly1(polyPts, point);
        }

        // Returns true iff p is strictly to the left of the directed
        // line through a to b.
        // You can use this method to determine if 3 adjacent CCW-ordered
        // vertices of a polygon form a convex or concave angle

        public static bool Left(Vector2Int a, Vector2Int b, Vector2Int p)
        {
            return CG.Left(a, b, p);
        }

        // Vector2 version of above
        public static bool Left(Vector2 a, Vector2 b, Vector2 p)
        {
            return CG.Left(CG.Convert(a), CG.Convert(b), CG.Convert(p));
        }






        //Student code to build the path network from the given pathNodes and Obstacles
        //Obstacles - List of obstacles on the plane
        //agentRadius - the radius of the traversing agent
        //minPoVDist AND maxPoVDist - used for Points of Visibility (see assignment doc)
        //pathNodes - ref parameter that contains the pathNodes to connect (or return if pathNetworkMode is set to PointsOfVisibility)
        //pathEdges - out parameter that will contain the edges you build.
        //  Edges cannot intersect with obstacles or boundaries. Edges must be at least agentRadius distance
        //  from all obstacle/boundary line segments. No self edges, duplicate edges. No null lists (but can be empty)
        //pathNetworkMode - enum that specifies PathNetwork type (see assignment doc)

        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
            List<Polygon> obstacles, float agentRadius, float minPoVDist, float maxPoVDist, ref List<Vector2> pathNodes, out List<List<int>> pathEdges,
            PathNetworkMode pathNetworkMode)
        {

            //STUDENT CODE HERE
            //epsilon fudge factor
            const float EPS = 1e-5f;

            //boundary rectangle
            Vector2 bottomLeft = canvasOrigin;
            Vector2 bottomRight = canvasOrigin + new Vector2(canvasWidth, 0f);
            Vector2 topLeft = canvasOrigin + new Vector2(0f, canvasHeight);
            Vector2 topRight = canvasOrigin + new Vector2(canvasWidth, canvasHeight);
            Vector2[] boundaryPoints = new Vector2[] { bottomLeft, bottomRight, topRight, topLeft };
            Vector2Int[] boundaryPointsInt = new Vector2Int[] { ConvertToInt(bottomLeft), ConvertToInt(bottomRight), ConvertToInt(topRight), ConvertToInt(topLeft) };

            //obstacle segments
            var obstacleSegmentFloat = new List<(Vector2 a, Vector2 b)>();
            var obstacleSegmentInt = new List<(Vector2Int a, Vector2Int b)>();
            var obstaclePolygonInt = new List<Vector2Int[]>();
            foreach (var poly in obstacles)
            {
                var pointsInt = poly.getIntegerPoints();
                obstaclePolygonInt.Add(pointsInt);

                int n = pointsInt.Length;
                for (int i = 0; i < n; i++)
                {
                    var aInt = pointsInt[i];
                    var bInt = pointsInt[(i + 1) % n];
                    obstacleSegmentInt.Add((aInt, bInt));
                    obstacleSegmentFloat.Add((ConvertToFloat(aInt), ConvertToFloat(bInt)));
                }
            }

            //boundary segments add to obstacles as well
            for (int i = 0; i < 4; i++)
            {
                var aInt = boundaryPointsInt[i];
                var bInt = boundaryPointsInt[(i + 1) % 4];
                obstacleSegmentInt.Add((aInt, bInt));
                obstacleSegmentFloat.Add((ConvertToFloat(aInt), ConvertToFloat(bInt)));
            }

            //build path network
            pathEdges = new List<List<int>>(pathNodes?.Count ?? 0);
            if (pathNodes == null || pathNodes.Count == 0) return;
            for (int i = 0; i < pathNodes.Count; i++) pathEdges.Add(new List<int>());
            int nNodes = pathNodes.Count;

            //helper method for margins:
            bool InsideCanvasWithMargin(Vector2 p, float margin)
            {
                return p.x >= bottomLeft.x + margin - EPS && p.x <= bottomRight.x - margin + EPS && p.y >= bottomLeft.y + margin - EPS && p.y <= topLeft.y - margin + EPS;
            }

            //helper method for obstacles:
            bool PointInsideAnyObstacle(Vector2Int pI)
            {
                foreach (var polyInt in obstaclePolygonInt)
                {
                    if (IsPointInPolygon(polyInt, pI)) return true;
                }
                return false;
            }

            //helper method for getting distance between segments
            float SegmentToSegmentMinDistance(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
            {
                float d1 = DistanceToLineSegment(a, c, d);
                float d2 = DistanceToLineSegment(b, c, d);
                float d3 = DistanceToLineSegment(c, a, b);
                float d4 = DistanceToLineSegment(d, a, b);
                return Mathf.Min(Mathf.Min(d1, d2), Mathf.Min(d3, d4));
            }

            //determine usable nodes:
            var nodeUsable = new bool[nNodes];
            for (int i = 0; i < nNodes; i++)
            {
                Vector2 p = pathNodes[i];
                if (!InsideCanvasWithMargin(p, agentRadius))
                {
                    nodeUsable[i] = false;
                    continue;
                }
                var pI = ConvertToInt(p);
                if (PointInsideAnyObstacle(pI))
                {
                    nodeUsable[i] = false;
                    continue;
                }
                //check enpoints from obsatacles
                bool violate = false;
                foreach (var seg in obstacleSegmentFloat)
                {
                    if (DistanceToLineSegment(p, seg.a, seg.b) + EPS < agentRadius)
                    {
                        nodeUsable[i] = false;
                        violate = true;
                        break;
                    }
                }
                if (violate) continue;
                nodeUsable[i] = true;
            }

            //build edges (predefined mode)
            for (int i = 0; i < nNodes; i++)
            {
                if (!nodeUsable[i]) continue;
                for (int j = i + 1; j < nNodes; j++)
                {
                    if (!nodeUsable[j]) continue;
                    Vector2 a = pathNodes[i];
                    Vector2 b = pathNodes[j];

                    bool zeroLength = (a == b);
                    Vector2Int aInt = ConvertToInt(a);
                    Vector2Int bInt = ConvertToInt(b);
                    bool violate = false;

                    //segment obstacle check
                    if (!zeroLength)
                    {
                        foreach (var seg in obstacleSegmentInt)
                        {
                            if (Intersects(aInt, bInt, seg.a, seg.b))
                            {
                                violate = true;
                                break;
                            }
                        }
                        if (violate) continue;
                    }

                    //endpoint boundary + obstacle clearance
                    foreach (var seg in obstacleSegmentFloat)
                    {
                        if (DistanceToLineSegment(a, seg.a, seg.b) + EPS < agentRadius)
                        {
                            violate = true;
                            break;
                        }
                        if (DistanceToLineSegment(b, seg.a, seg.b) + EPS < agentRadius)
                        {
                            violate = true;
                            break;
                        }
                    }
                    if (violate) continue;

                    //segment boundary + obstacle clearance
                    if (!zeroLength)
                    {
                        foreach (var seg in obstacleSegmentFloat)
                        {
                            float minDist = SegmentToSegmentMinDistance(a, b, seg.a, seg.b);
                            if (minDist + EPS < agentRadius)
                            {
                                violate = true;
                                break;
                            }
                        }
                        if (violate) continue;
                    }

                    //add edge
                    pathEdges[i].Add(j);
                    pathEdges[j].Add(i);
                }
            }



            // END STUDENT CODE

        }


    }

}