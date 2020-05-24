using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{

    public const string LineGroupName = "LineVizGroup";
    public const string LineGroupLayer = "LineViz";
    public const float ZOffset = 0.01f;

    public static bool Intersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        //check if c and d lie on opposite sides of a and b
        //check if a and b lie on opposite sides of c and d
        float detA = det((d - c), (a - c));
        float detB = det((d - c), (b - c));
        bool dirA = detA > 0;
        bool dirB = detB > 0;
        if (!(dirA ^ dirB) || detA == 0 || detB == 0) return false;
        float detC = det((b - a), (c - a));
        float detD = det((b - a), (d - a));
        bool dirC = detC > 0;
        bool dirD = detD > 0;
        if (!(dirC ^ dirD) || detC == 0 || detD == 0) return false;
        return true;
    }

    public static bool Intersects(Vector2 a, Vector2 b, Polygon poly)
    {

        if (poly == null || poly.getPoints() == null || poly.getPoints().Length < 1)
            return false;

        bool doesIntersect = false;

        for(int i = 0; i < poly.getPoints().Length; ++i)
        {
            var pt1 = poly.getPoints()[i];
            var pt2 = poly.getPoints()[(i + 1) % poly.getPoints().Length];
            if (Intersects(a, b, pt1, pt2))
            {
                doesIntersect = true;
                break;
            }
        }

        return doesIntersect;

    }

    public static float det(Vector2 a, Vector2 b)
    {
        float res = a.x * b.y - b.x * a.y;
        return res;
    }
    public static GameObject FindOrCreateGameObjectByName(string name)
    {
        var go = GameObject.Find(name);

        if(go == null)
        {
            go = new GameObject(name);
        }

        return go;
    }

    public static GameObject FindOrCreateGameObjectByName(GameObject parent, string name)
    {
     
        var xform = parent.transform.Find(name);

        GameObject go = null;

        if (xform == null)
        {
            go = new GameObject(name);
            go.transform.parent = parent.transform;
        }
        else
        {
            go = xform.gameObject;
        }

        return go;
    }
    public static GameObject DrawLine(Vector2 start, Vector2 end, float zpos, GameObject parent, Color color, Material mat = null, float lineWidth=0.05f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.parent = parent.transform;//FindOrCreateByName(Utils.LineGroupName).transform;
        myLine.layer = LayerMask.NameToLayer(LineGroupLayer);
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        if (mat != null)
            lr.material = mat;
        lr.material.color = color;
        lr.startColor = color;
        lr.startWidth = lineWidth;
        lr.endColor = color;
        lr.endWidth = lr.startWidth;
        lr.SetPosition(0, new Vector3(start.x, zpos, start.y));
        lr.SetPosition(1, new Vector3(end.x, zpos, end.y));

        return myLine;
    }


    public static void DisplayName(string parent, string name)
    {
        var snameObj = GameObject.Find("StudentName");

        if (snameObj == null)
        {
            Debug.LogError("Name text field not found!");
        }
        else
        {
            var txt = snameObj.GetComponent<Text>();

            if (txt == null)
            {
                Debug.LogError("No text!");
            }
            else
            {
                txt.text += parent + ":" + name + System.Environment.NewLine;
            }
        }
    }
    
    //Get the shortest distance from a point to a line
    //Line is defined by the lineStart and lineEnd points
    public static float DistanceToLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        //If point is beyond the two points of the line, return the shorter distance to the line end points
        Vector2 ptToA = lineStart - point;
        Vector2 ptToB = lineEnd - point;
        if (Vector2.Dot(ptToA, ptToB) > 0)
        {
            //point is beyond end points
            return Mathf.Min(ptToA.magnitude, ptToB.magnitude);
        }
        else
        {
            //find the perpendicular distance to line
            Vector2 AB = lineStart - lineEnd;
            AB.Normalize();
            float scale = Vector2.Dot(-ptToB, AB);
            AB.Scale(new Vector2(scale, scale));
            Vector2 pt = lineEnd + AB;
            return Vector2.Distance(pt, point);
        }
    }
    /*
     * Returns true if the point lies on the polygon.
     * Since exact distance cannot be measured we rely on a small value of epsilon
     */
    public static bool PointOnPolygon(Vector2 point, Vector2[] polygon)
    {
        float epsilon = 0.1f;
        for (int i = 0; i < polygon.Length; i++)
        {
            int j = (i + 1) % polygon.Length;
            if (DistanceToLine(point, polygon[i], polygon[j]) < epsilon)
                return true;
        }
        return false;      
    }
    /**
     * Returns true if the polygon is convex
     */
    public static bool IsConvex(Vector2[] polygon)
    {
        if (polygon.Length <= 2)
            return true;
        float crossProduct = det(polygon[1]-polygon[0], polygon[2]-polygon[1]);
        for ( int i = 1; i < polygon.Length; i++)
        {
            int j = (i + 1) % polygon.Length;
            int k = (j + 1) % polygon.Length;
            float newProduct = det(polygon[j] - polygon[i], polygon[k] - polygon[j]);
            if (crossProduct / newProduct < 0)
                return false;
        }
        return true;
    }
    /*
     * Returns false if there is an unobstructed  ray from point to dest point
     * Obstruction can be caused by the values in the lines
     */
    public static bool IsRayObstructed(Vector2 src_point, Vector2 dest_point, Vector2[,] lines)
    {
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            if (Intersects(src_point, dest_point, lines[i, 0], lines[i, 1]))
                return true;
        }
        return false;
    }
    
    /*
    * Returns false if there is an unobstructed  ray from point to dest point
    * Obstruction can be caused by the values in the lines
    */
    public static bool IsRayObstructed(Vector2 src_point, Vector2 dest_point, List<Polygon> polygons)
    {
        for (int i = 0; i < polygons.Count; i++)
        {
            Vector2[] points = polygons[i].getPoints();
            int i1 = -1, i2 = -1;
            for (int j = 0; j < points.Length; j++)
            {
                if (src_point == points[j])
                {
                    i1 = j;
                    //both points on same polygon? We are going inside a convex polygon
                    //
                    /*if (i2 != -1)
                        if ((i2 + 1) % points.Length != i1 && (i1 + 1) % points.Length != i2)
                        {
                            //check intersection of this line against every line of the obstacle.
                            //this check is needed for concave polygon
                            for(int k = 0; k < p)
                            return true;
                        }*/
                }
                if (dest_point == points[j])
                {
                    i2 = j;
                    //both points on same polygon? We are going inside a convex polygon
                    /*if (i1 != -1)
                        if ((i2 + 1) % points.Length != i1 && (i1 + 1) % points.Length != i2)
                            return true;
                */
                }
                if (Intersects(src_point, dest_point, points[j], points[(j+1)%points.Length]))
                        return true;
            }
            //mid point check for polygons
            //in case the points lie on the polygons, this test will help us figure that out
            //do this only in case the points are not found 
            if ((i1 == -1) || (i2 == -1) || (i1 != -1 && i2 != -1) && (i1 + 1)% points.Length != i2 && (i2+1)%points.Length != i1)
            {
                Vector2 midPoint = new Vector2(0.00f, 0.00f);

                midPoint.x = midPoint.x + 0.50f * (src_point.x + dest_point.x);
                midPoint.y = midPoint.y + 0.50f * (src_point.y + dest_point.y);
                if (polygons[i].IsPointInPolygon(midPoint))
                    return true;
            }
        }
        return false;
    }

    /*
     * Returns the index of the point in nodes that is closest to the current point without
     * Being obstructed by any line in the lines tuple
     * Will return -1 if there is no such point
     */
    public static int FindClosestUnobstructed(Vector2 point, Vector2[] nodes, List<Polygon> polygons)
    {
        float minDist = float.MaxValue;
        int minIndex = -1;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (IsRayObstructed(point, nodes[i], polygons))
                continue;
            float dist = Vector2.Distance(point, nodes[i]);
            if ( minDist > dist)
            {
                minIndex = i;
                minDist = dist;
            }
        }
        return minIndex;
    }

    /**
     * Returns true if the two polygons are adjacent to each other 
     * Polygons are adjacent if any two sides are the same
     */
    public static bool PolygonsAdjacent(Vector2[] polygon1, Vector2[] polygon2)
    {
        float epsilon = 0.01f;
        for (int i = 0; i < polygon1.Length; i++)
        {
            int i_n = (i + 1) % polygon1.Length;
            for (int j = 0; j < polygon2.Length; j++)
            {
                int j_n = (j + 1) % polygon2.Length;
                if ((Vector2.Distance(polygon1[i], polygon2[j]) < epsilon && Vector2.Distance(polygon1[i_n], polygon2[j_n]) < epsilon)
                    || (Vector2.Distance(polygon1[i_n], polygon2[j]) < epsilon && Vector2.Distance(polygon1[i], polygon2[j_n]) < epsilon))
                    return true;    
            }
        }
        return false;
    }

    //Creates a hash for the line segment created by points a and b
    public static string HashLine(Vector2 a, Vector2 b)
    {
        string line;
        if (a.magnitude < b.magnitude)
        {
            line = a.ToString() + b.ToString();
        }
        else
        {
            line = b.ToString() + a.ToString();
        }
        return line;
    }

    /**Finds the centroid of the given polygon
     * */
    public static Vector2 GetCentroid(Vector2[] polygon)
    {
        if (polygon.Length == 0) return Vector2.zero;
        Vector2 sum = Vector2.zero;
        for (int i = 0; i < polygon.Length; i++)
        {
            sum += polygon[i];
        }
        sum = sum / polygon.Length;
        return sum;
    }

    public static Vector2[] CombineArrays(Vector2[] v1, Vector2[] v2)
    {
        int totalLength = v1.Length + v2.Length;
        Vector2[] cArray = new Vector2[totalLength];
        v1.CopyTo(cArray, 0);
        v2.CopyTo(cArray, v1.Length);
        return cArray;
    }

    public static Vector2 PerturbPoint(Vector2 p)
    {
        float epsilon = 0.01f;
        Vector2 pt = p + new Vector2(Random.Range(0, epsilon), Random.Range(0, epsilon));
        return pt;
    }
}
