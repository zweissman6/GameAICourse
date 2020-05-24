using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiscretizedSpaceMonoBehavior : MonoBehaviour, IDiscretizedSpace
{

    private Obstacles obstacles;
    private List<GameObject> pathNodeMarkers = new List<GameObject>();
    private List<Vector2> pathNodes = new List<Vector2>();
    private List<List<int>> pathEdges = new List<List<int>>();

    protected Renderer _renderer;

    public Obstacles Obstacles { get => obstacles; protected set => obstacles = value; }

    public List<Vector2> PathNodes { get => pathNodes; protected set => pathNodes = value; }

    public List<List<int>> PathEdges { get => pathEdges; protected set => pathEdges = value; }

    public List<GameObject> PathNodeMarkers { get => pathNodeMarkers; protected set => pathNodeMarkers = value; }

    public Vector2 BottomLeftCornerWCS
    {
        get
        {
            //return new Vector2(this.transform.position.x - Boundary.size.x / 2f, this.transform.position.z - Boundary.size.z / 2f);
            return new Vector2(Boundary.min.x, Boundary.min.z);
        }
    }

    public Bounds Boundary { get => _renderer.bounds; }

    public virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer == null)
            Debug.LogError("No renderer");
    }


    virtual public void Bake()
    {

    }
}
