using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : IEquatable<Polygon>
{
    Vector2[] points;       //list of all points in the polygon
    Vector2[,] lines;
    Vector2 minBounds, maxBounds;
    Vector2 centroid;
    float epsilon = 0.1f;
    float width, height;
    
    public void Init()
    {
        const float d = 1f;
        if (points == null) points = new Vector2[] { new Vector2(d, 0f), new Vector2(d,d), new Vector2(d,0f) };
        calculateBounds();
        CreateLines();
        CalculateCentroid();
    }
    public void SetPoints(Vector2[] newPoints)
    {
        points = newPoints;
        calculateBounds();
        CreateLines();
        CalculateCentroid();

    }
    void CalculateCentroid()
    {
        centroid = Utils.GetCentroid(getPoints());
    }
    void CreateLines()
    {
        lines = new Vector2[points.Length, 2];
        for (int i = 0; i < points.Length; i++)
        {
            lines[i, 0] = points[i];
            lines[i, 1] = points[(i + 1) % points.Length];
        }
    }
    public Vector2 GetCentroid()
    {
        return centroid;
    }

    public Vector2[,] GetLines()
    {
        return lines;
    }

    public Vector2[] getPoints()
    {
        return points;
    }


    public void CalculateBounds(out Vector2 min, out Vector2 max)
    {
        min = float.MaxValue * Vector2.one;
        max = float.MinValue * Vector2.one;

        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].x < minBounds.x) minBounds.x = points[i].x;
            if (points[i].x > maxBounds.x) maxBounds.x = points[i].x;
            if (points[i].y < minBounds.y) minBounds.y = points[i].y;
            if (points[i].y > maxBounds.y) maxBounds.y = points[i].y;
        }
    }

    void calculateBounds()
    {
        minBounds = points[0]; maxBounds = points[0];
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].x < minBounds.x) minBounds.x = points[i].x;
            if (points[i].x > maxBounds.x) maxBounds.x = points[i].x;
            if (points[i].y < minBounds.y) minBounds.y = points[i].y;
            if (points[i].y > maxBounds.y) maxBounds.y = points[i].y;
        }
        minBounds.x -= epsilon; minBounds.y -= epsilon; maxBounds.x += epsilon; maxBounds.y += epsilon;
    }


    // FOR INTERNAL USE ONLY DUE TO THE EPSILON. ONLY MEANT FOR FIRST PASS POINT CONTAINMENT TEST
    bool inBounds(Vector2 pt)
    {
        if (pt.x < minBounds.x || pt.y < minBounds.y || pt.x > maxBounds.x || pt.y > maxBounds.y)
            return false;
        return true;
    }


    ////This function does not consider points on the boundary as being on the inside
    //public bool IsPointInPolygon(Vector2 p)
    //{
    //    // first pass axis aligned bounding box test
    //    if (!inBounds(p)) return false;

    //    bool inside = false;
    //    for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
    //    {
    //        // Short circuit bool eval protects divByZero potential
    //        if (
    //            (
    //            (points[i].y > p.y) != (points[j].y > p.y) ||
    //            (points[i].y >= p.y) != (points[j].y >= p.y)
    //            ) &&
    //             p.x < (points[j].x - points[i].x) * (p.y - points[i].y) / (points[j].y - points[i].y) + points[i].x)
    //        {
    //            Debug.Assert((points[j].y - points[i].y) != 0f, "0 DENOM");

    //            inside = !inside;
    //        }
    //    }
    //    return inside;
    //}


    // Check if a point is one of the vertices
    public bool IsPointAVertex(Vector2 p)
    {
        foreach (var pt in getPoints())
        {
            if (p == pt)
                return true;
        }

        return false;
    }

    //is the point inside the polygon area
    //This function is inconsistent regarding points on the boundary as being on the inside
    public bool IsPointInPolygon(Vector2 p)
    {
        // first pass axis aligned bounding box test
        if (!inBounds(p)) return false;

        bool inside = false;
        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            // Short circuit bool eval protects divByZero potential
            if ((points[i].y > p.y) != (points[j].y > p.y) &&
                 p.x < (points[j].x - points[i].x) * (p.y - points[i].y) / (points[j].y - points[i].y) + points[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    public bool IsLineInPolygonSameDirection(Vector2 ptA, Vector2 ptB)
    {
        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            if (ptB == points[i] && ptA == points[j])
                return true;
        }
        return false;
    }

    public bool IsLineInPolygonOppositeDirection(Vector2 ptA, Vector2 ptB)
    {
        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            if (ptA == points[i] && ptB == points[j])
                return true;
        }
        return false;
    }

    public bool IsLineInPolygon(Vector2 ptA, Vector2 ptB)
    {
        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            if ((ptA == points[i] && ptB == points[j]) ||
                    (ptB == points[i] && ptA == points[j]))
                return true;
        }
        return false;
    }

    public bool IsConvex()
    {
        return Utils.IsConvex(this.getPoints());
    }

    public int GetLength()
    {
        return getPoints().Length;
    }
    /**
     * Returns if the polygon is clockwise
     * Will only work for convex polygons
     */
    public bool IsClockwise()
    {
        if (GetLength() < 3)
            return false;
        for (int i = 0; i < GetLength(); i++)
        {
            Vector2 l1 = getPoints()[(i + 1) % GetLength()] - getPoints()[i];
            Vector2 l2 = getPoints()[(i + 2) % GetLength()] - getPoints()[(i + 1) % GetLength()];
            if (Utils.det(l1, l2) > 0)
                return true;
            else if (Utils.det(l1, l2) < 0)
                return false;
            //if 0 continue
        }
        Debug.LogError("Error, the polygon was just a straight line");
        //would ideally raise an exception here
        return false;
    }

    /*
     * Reverses the direction of this polygon
     */
    public void Reverse()
    {
        System.Array.Reverse(getPoints());
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Polygon);
    }

    public bool Equals(Polygon other)
    {
        return other != null &&
               EqualityComparer<Vector2[]>.Default.Equals(points, other.points);
    }

    public override int GetHashCode()
    {
        var hashCode = 1410917715;
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector2[]>.Default.GetHashCode(points);

        return hashCode;
    }
}
