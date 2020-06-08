using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPolygonEdge
{

    public Vector2 A { get; protected set; }
    public Vector2 B { get; protected set; }

    public CommonPolygonEdge(Vector2 A, Vector2 B)
    {
        this.A = A;
        this.B = B;
    }

    // vertex order doesn't matter for matching a common edge
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var ocpe = obj as CommonPolygonEdge;

        if (ocpe == null)
            return false;

        if ((this.A == ocpe.A && this.B == ocpe.B) ||
            (this.A == ocpe.B && this.B == ocpe.A)
            )
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        // dot product is commutative
        return Mathf.RoundToInt(Vector2.Dot(this.A, this.B));
    }

    public override string ToString()
    {
        return $"A:({this.A.x}, {this.A.y}), B:({this.B.x}, {this.B.y})";
    }
}
