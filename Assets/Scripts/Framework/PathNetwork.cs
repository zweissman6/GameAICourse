using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameAICourse;

/*
 * Class that holds all information about the path network
 *
 */
public class PathNetwork : DiscretizedSpaceMonoBehavior
{

    private string PathNodeMarkersGroupName = "PathNodeMarkersGroup";

    //public GameObject PathNodeMarkersGroup;
    public MoveBall moveBall;
    //material for drawing the path network line
    public Material LineMaterial;
    public Color LineColor = Color.green;


    public override void Awake()
    {
        base.Awake();

        Obstacles = this.GetComponent<Obstacles>();
    }


    void Start()
    {
        Utils.DisplayName("CreatePathNetwork", CreatePathNetwork.StudentAuthorName);
        Obstacles.Init();

        //Init();

        Bake();
    }


    //Get all child node cubes and create their corresponding path node vectors
    void Init()
    {

        var PathNodeMarkersGroup = GameObject.Find(PathNodeMarkersGroupName);

        if (PathNodeMarkersGroup != null)
        {

            PathNodeMarkers = new List<GameObject>(PathNodeMarkersGroup.transform.childCount);

            for (int i = 0; i < PathNodeMarkersGroup.transform.childCount; ++i)
            {
                PathNodeMarkers.Add(PathNodeMarkersGroup.transform.GetChild(i).gameObject);
            }

        }

    }


    void PurgeOutdatedLineViz()
    {

        var linegroup = this.transform.Find(Utils.LineGroupName);

        if (linegroup != null)
        {
            linegroup.name = "MARKED_FOR_DELETION";
            linegroup.gameObject.SetActive(false);
            Destroy(linegroup.gameObject);
        }
    }



    void CreateNetworkLines()
    {
        PurgeOutdatedLineViz();

        var parent = Utils.FindOrCreateGameObjectByName(this.gameObject, Utils.LineGroupName);


        HashSet<System.Tuple<int, int>> hs = new HashSet<System.Tuple<int, int>>();


        if (PathEdges != null)
        {
            for (int i = 0; i < PathEdges.Count; ++i)
            {
                var pts = PathEdges[i];
                if (pts != null)
                {
                    for (int j = 0; j < pts.Count; ++j)
                    {
                        var smaller = i;
                        var bigger = pts[j];

                        if (bigger < smaller)
                        {
                            var tmp = bigger;
                            bigger = smaller;
                            smaller = tmp;
                        }

                        var tup = new System.Tuple<int, int>(smaller, bigger);
                        if (!hs.Contains(tup))
                        {
                            hs.Add(tup);
                            Utils.DrawLine(PathNodes[i], PathNodes[pts[j]], Utils.ZOffset, parent, Color.green, LineMaterial);
                        }

                    }
                }
            }
        }
    }


    public override void Bake()
    {

        Init();

        PathNodes = new List<Vector2>(PathNodeMarkers.Count);

        for (int i = 0; i < PathNodeMarkers.Count; i++)
        {
            PathNodes.Add(new Vector2(PathNodeMarkers[i].transform.position.x, PathNodeMarkers[i].transform.position.z));
        }

        List<List<int>> pathEdges;

        CreatePathNetwork.Create(BottomLeftCornerWCS, Boundary.size.x, Boundary.size.z,
                                    Obstacles.getObstacles(), moveBall.Radius, PathNodes, out pathEdges);

        PathEdges = pathEdges;

        CreateNetworkLines();

    }

}
