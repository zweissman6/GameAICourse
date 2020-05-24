using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using GameAICourse;

public class MoveBall : MonoBehaviour, IBallMover
{

    private const string PathVizGroupName = "PathViz";

    public float Speed = 1.0f;
    Rigidbody rigidBody;
    SphereCollider sphereCollider;
 
    public LayerMask mask;

    IDiscretizedSpace discretizedSpace;
    //the distance at which the path node is reached
    float epsilon = 0.01f;

    List<int> followPath;


    public float Radius { get => this.sphereCollider.radius * this.transform.lossyScale.x; }

    public Vector2 CurrentPosition
    {
        get => new Vector2(transform.position.x, transform.position.z);

        set
        {
            rigidBody.MovePosition(new Vector3(value.x, 0f, value.y));

            Init();
        }
    }


    public Color StartMarkerColor = Color.green;
    public Color EndMarkerColor = Color.blue;
    public Color PathColor = Color.gray;
    public Material MarkerLineMaterial;
    public int IncrSearchMaxNodeExplore = 1;


    List<Tuple<Vector2, Vector2>> rawPath;
    int currPathIndex = 0;
    float pathStartTime;

    bool initSearch = false;
    bool activeSearch = false;
    bool incrementalSearch = false;
    int nearestGoalNodeIndex = -1;
    int nearestStartNodeIndex = -1;
    //Vector2 startPos;
    Vector2 goalPos;
    int currentNode = -1;
    Dictionary<int, PathSearchNodeRecord> searchNodeRecords = null;
    Priority_Queue.SimplePriorityQueue<int, float> openNodes = null;
    HashSet<int> closedNodes = null;
    List<int> returnPath = null;


    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();

        if (!rigidBody)
            Debug.LogError("No rigid body");

        sphereCollider = this.GetComponent<SphereCollider>();

        if (!sphereCollider)
            Debug.LogError("No collider");
    }

    void Start()
    {

        discretizedSpace = (IDiscretizedSpace)Manager.Instance.DiscretizedSpace;
        if (discretizedSpace == null)
            Debug.LogError("path network is null");

        Init();
    }

    public void Init()
    {
        rawPath = null;
        currPathIndex = 0;

        PurgeOutdatedLineViz();

    }



    // Update is called once per frame
    void Update()
    {

        PathSearchResultType pathResult = PathSearchResultType.InProgress;
 

        if (activeSearch)
        {
            if (incrementalSearch)
            {
                pathResult = Manager.Instance.PathSearchProvider.FindPathIncremental(
                    discretizedSpace.PathNodes, discretizedSpace.PathEdges, nearestStartNodeIndex, nearestGoalNodeIndex,
                    IncrSearchMaxNodeExplore, initSearch, ref currentNode, ref searchNodeRecords, ref openNodes,
                    ref closedNodes, ref returnPath);
            }
            else
            {
                pathResult = Manager.Instance.PathSearchProvider.FindPath(
                    discretizedSpace.PathNodes, discretizedSpace.PathEdges, nearestStartNodeIndex, nearestGoalNodeIndex,
                    ref currentNode, ref searchNodeRecords, ref openNodes, ref closedNodes, ref returnPath);
            }
        }

        DEBUG_pathResult = pathResult;
        DEBUG_startIndex = nearestStartNodeIndex;
        DEBUG_goalIndex = nearestGoalNodeIndex;

        if(nearestStartNodeIndex >= 0 && nearestStartNodeIndex < discretizedSpace.PathEdges.Count) 
            DEBUG_edgesAtStart = discretizedSpace.PathEdges[nearestStartNodeIndex];
        if(nearestGoalNodeIndex >= 0 && nearestGoalNodeIndex < discretizedSpace.PathEdges.Count)
            DEBUG_edgesAtGoal = discretizedSpace.PathEdges[nearestGoalNodeIndex];

        if (initSearch)
        {
            //Debug.Log("Search was initialized");

            initSearch = false;
        }


        if(activeSearch)
        {

            //Debug.Log("Updating search viz");

            PurgeOutdatedLineViz();

            CreateMarkerLines(discretizedSpace.PathNodes[nearestGoalNodeIndex], 0.2f, EndMarkerColor);

            CreateMarkerLines(discretizedSpace.PathNodes[nearestStartNodeIndex], 0.2f, StartMarkerColor);

            CreateSetLines(discretizedSpace.PathNodes, searchNodeRecords, openNodes, closedNodes);

            if (pathResult != PathSearchResultType.InProgress)
            {
                //Debug.Log("Ending active search because pathResult was not InProgress: " + pathResult);

                activeSearch = false;

                if (pathResult != PathSearchResultType.InitializationError)
                {

                    CreatePathLines(returnPath, CurrentPosition, goalPos, pathResult);

                    //rawPath = GenerateRawPath(returnPath, CurrentPosition, goalPos, pathResult);

                    var fullPath = GenerateFullPath(returnPath, CurrentPosition, goalPos, pathResult);

                    List<Vector2> refinedPath;

                    var refPathRes = PathRefinement.Refine(
                        new Vector2(discretizedSpace.Boundary.min.x, discretizedSpace.Boundary.min.z),
                        discretizedSpace.Boundary.size.x,
                        discretizedSpace.Boundary.size.z,
                        Radius,
                        fullPath,
                        discretizedSpace.Obstacles.getObstacles(),
                        1,
                        0,
                        out refinedPath);

                    if (refPathRes)
                        rawPath = GenerateRawPath(refinedPath);
                    else
                        rawPath = GenerateRawPath(fullPath);

                    currPathIndex = 0;
                    pathStartTime = Time.timeSinceLevelLoad;

                    DEBUG_path = returnPath;
                    DEBUG_rawPath = rawPath;

                }
            }

        }



        if(rawPath != null && rawPath.Count > 0 && currPathIndex < rawPath.Count)
        {
            var currPathSegment = rawPath[currPathIndex];
            var currDist = Vector2.Distance(currPathSegment.Item1, currPathSegment.Item2);
            var currTime = Time.timeSinceLevelLoad;
            var elapsedTime = currTime - pathStartTime;
            var expectedArrivalTime = currDist / Speed;
            var distTraveled = elapsedTime * Speed;

            if( elapsedTime >= expectedArrivalTime)
            {
                ++currPathIndex;
                var pos = new Vector3(currPathSegment.Item2.x, 0f, currPathSegment.Item2.y);
                rigidBody.MovePosition(pos);
                pathStartTime = Time.timeSinceLevelLoad;
            }
            else
            {
                var t = 1f;
                if (currDist >= epsilon)
                    t = distTraveled / currDist;

                var newPos = Vector2.Lerp(currPathSegment.Item1, currPathSegment.Item2, t);
                var pos = new Vector3(newPos.x, 0f, newPos.y);
                rigidBody.MovePosition(pos);
            }

        }
    }


    int NearestValidNodeIndex(Vector2 v)
    {
        var minDist = float.MaxValue;
        int minNodeIndex = -1;

        for(int i = 0; i < discretizedSpace.PathNodes.Count; ++i)
        {
            var n = discretizedSpace.PathNodes[i];

            var dist = Vector2.Distance(v, n);


            if (dist < minDist)
            {

                bool goodPoint = true;

                foreach(var o in discretizedSpace.Obstacles.getObstacles())
                {
                    if(o.IsPointInPolygon(n))
                    {
                        //Debug.Log("Point in polygon");
                        goodPoint = false;
                        break;
                    }
                    else if(Utils.Intersects(v,n, o.GetPolygon()))
                    {
                        //Debug.Log("No line of sight");
                        goodPoint = false;
                        break;
                    }
                }

                if (!goodPoint)
                    continue;

                minDist = dist;
                minNodeIndex = i;
            }

        }

        return minNodeIndex;
    }

    public List<int> DEBUG_path;
    public List<Tuple<Vector2, Vector2>> DEBUG_rawPath;
    public PathSearchResultType DEBUG_pathResult;
    public int DEBUG_startIndex;
    public int DEBUG_goalIndex;
    public List<int> DEBUG_edgesAtGoal;
    public List<int> DEBUG_edgesAtStart;



    static bool ballTooClose(Vector2 testPos, Vector2 lineposA, Vector2 lineposB, float ballRadius)
    {
        var d = Utils.DistanceToLine(testPos, lineposA, lineposB);

        return (d <= ballRadius);
        
    }

    public void OnClicked(RaycastHit hit, bool isLeft)
    {
        PurgeOutdatedLineViz();

        var clickLoc = new Vector2(hit.point.x, hit.point.z);

        foreach(var o in discretizedSpace.Obstacles.getObstacles())
        {
            //cannot click inside a shape since those can be dragged and catch the mouse
            //but if that changes...

            var min = discretizedSpace.Boundary.min;
            var max = discretizedSpace.Boundary.max;

            if (
                ballTooClose(clickLoc, new Vector2(min.x, min.z), new Vector2(max.x, min.z), Radius) ||
                ballTooClose(clickLoc, new Vector2(max.x, min.z), new Vector2(max.x, max.z), Radius) ||
                ballTooClose(clickLoc, new Vector2(max.x, max.z), new Vector2(min.x, max.z), Radius) ||
                ballTooClose(clickLoc, new Vector2(min.x, max.z), new Vector2(min.x, min.z), Radius) )
            {
                Debug.Log("Ball cant fit here");
                return;
            }


            var pts = o.GetPoints();
            for(int i = 0, j = pts.Length-1; i<pts.Length; j = i++ )
            {
                var pt0 = pts[i];
                var pt1 = pts[j];

                if (ballTooClose(clickLoc, pt0, pt1, Radius))
                {
                    Debug.Log("Ball can't fit here");
                    return;
                }
            }

        }

        nearestGoalNodeIndex = NearestValidNodeIndex(clickLoc);

        if(nearestGoalNodeIndex < 0)
        {
            Debug.Log("Not a valid click location");
            return;
        }

        var nearestGoalNode = discretizedSpace.PathNodes[nearestGoalNodeIndex];

        nearestStartNodeIndex = NearestValidNodeIndex(CurrentPosition);

        if(nearestStartNodeIndex < 0)
        {
            Debug.Log("Could not assign start location");
            return;
        }

        var nearestStartNode = discretizedSpace.PathNodes[nearestStartNodeIndex];

        //startPos = CurrentPosition;
        goalPos = clickLoc;

        CreateMarkerLines(nearestGoalNode, 0.2f, EndMarkerColor);

        CreateMarkerLines(nearestStartNode, 0.2f, StartMarkerColor);

        initSearch = true;
        activeSearch = true;
        incrementalSearch = !isLeft;
        rawPath = null;
        currPathIndex = 0;

        ////TODO Remove all this to Update()

        //PathResultType pathResult;
        //var path = Manager.Instance.SearchProvider.FindPath(discretizedSpace.PathNodes,
        //    discretizedSpace.PathEdges, nearestStartNodeIndex, nearestGoalNodeIndex, out pathResult);

        //CreatePathLines(path, CurrentPosition, clickLoc, pathResult);

        //rawPath = GenerateRawPath(path, CurrentPosition, clickLoc, pathResult);
        //currPathIndex = 0;
        //pathStartTime = Time.timeSinceLevelLoad;

        //DEBUG_path = path;
        //DEBUG_rawPath = rawPath;
        //DEBUG_pathResult = pathResult;
        //DEBUG_startIndex = nearestStartNodeIndex;
        //DEBUG_goalIndex = nearestGoalNodeIndex;
        //DEBUG_edgesAtStart = discretizedSpace.PathEdges[nearestStartNodeIndex];
        //DEBUG_edgesAtGoal = discretizedSpace.PathEdges[nearestGoalNodeIndex];
    }


    List<Vector2> GenerateFullPath(List<int> path, Vector2 startPos, Vector2 goalPos, PathSearchResultType pathRes)
    {

        if (path == null || path.Count < 1)
            return null;

        var resPath = new List<Vector2>(path.Count + 2);

        resPath.Add(startPos);
        
        for (int i = 0; i < path.Count; ++i)
        {
            resPath.Add(discretizedSpace.PathNodes[path[i]]);
        }

        if (pathRes == PathSearchResultType.Complete)
        {
            resPath.Add(goalPos);
        }

        return resPath;

    }

    List<System.Tuple<Vector2, Vector2>> GenerateRawPath(List<Vector2> path)
    {
        var res = new List<System.Tuple<Vector2, Vector2>>(path.Count);

        if (path.Count < 2)
            return res;

        for (int i = 0; i < path.Count - 1; ++i)
            res.Add(new Tuple<Vector2, Vector2>(path[i], path[i + 1]));


        return res;
    }

    List<System.Tuple<Vector2, Vector2>> GenerateRawPath(List<int> path, Vector2 startPos, Vector2 goalPos, PathSearchResultType pathRes)
    {

        if (path == null || path.Count < 1)
            return null;

        var rawPath = new List<System.Tuple<Vector2, Vector2>>(path.Count + 2);

        rawPath.Add(new System.Tuple<Vector2, Vector2>(startPos, discretizedSpace.PathNodes[path[0]]));

        for(int i = 0; i<path.Count -1; ++i)
        {
            rawPath.Add(new System.Tuple<Vector2, Vector2>(discretizedSpace.PathNodes[path[i]], discretizedSpace.PathNodes[path[i + 1]]));

        }

        if(pathRes == PathSearchResultType.Complete)
        {
            rawPath.Add(new System.Tuple<Vector2, Vector2>(discretizedSpace.PathNodes[path[path.Count-1]], goalPos));
        }

        return rawPath;

    }


    void PurgeOutdatedLineViz()
    {

        var linegroup = this.transform.Find(PathVizGroupName);

        if (linegroup != null)
        {
            linegroup.name = "MARKED_FOR_DELETION";
            linegroup.gameObject.SetActive(false);
            Destroy(linegroup.gameObject);
        }
    }


    void CreateSetLines(
        List<Vector2> nodes,
        Dictionary<int, PathSearchNodeRecord> searchNodeRecords,
        Priority_Queue.SimplePriorityQueue<int, float> openNodes,
        HashSet<int> closedNodes)
    {

        foreach(var nindex in openNodes)
        {
            var pindex = searchNodeRecords[nindex].FromNodeIndex;
            var node = nodes[nindex];
            float angle = 0f;
            bool directed = false;
            if (pindex >= 0 && pindex < nodes.Count)
            {
                var pnode = nodes[pindex];
                Vector2 flip = new Vector2(1f, -1f);
                angle = Vector2.SignedAngle(Vector2.right, flip*(pnode - node));
                directed = true;
            }

            //Debug.Log("Angle: " + angle);

            if (directed)
                CreateDirectedMarkerLines(node, 0.1f, Color.white, angle);
            else
                CreateMarkerLines(node, 0.1f, Color.white, 0.01f);
        }

        foreach(var nindex in closedNodes)
        {
            var pindex = searchNodeRecords[nindex].FromNodeIndex;
            var node = nodes[nindex];
            float angle = 0f;
            bool directed = false;
            if (pindex >= 0 && pindex < nodes.Count)
            {
                var pnode = nodes[pindex];
                Vector2 flip = new Vector2(1f, -1f);
                angle = Vector2.SignedAngle(Vector2.right, flip*(pnode - node));
                directed = true;
            }
            if(directed)
                CreateDirectedMarkerLines(node, 0.1f, Color.cyan, angle);
            else
                CreateMarkerLines(node, 0.1f, Color.white, 0.01f);
        }

    }


    void CreatePathLines(List<int> path, Vector2 startPos, Vector2 goalPos, PathSearchResultType pathResult)
    {
        var parent = Utils.FindOrCreateGameObjectByName(this.gameObject, PathVizGroupName);

        if (path.Count < 1)
            return;

        Utils.DrawLine(startPos, discretizedSpace.PathNodes[path[0]], Utils.ZOffset * 1.9f, parent, PathColor, MarkerLineMaterial);

        for(int i=0; i<path.Count-1; ++i)
        {
            var pn1 = path[i];
            var pn2 = path[i + 1];

            Utils.DrawLine(discretizedSpace.PathNodes[pn1], discretizedSpace.PathNodes[pn2], Utils.ZOffset * 1.9f, parent, PathColor, MarkerLineMaterial);
        }

        if(pathResult == PathSearchResultType.Complete)
        {
            Utils.DrawLine(discretizedSpace.PathNodes[path[path.Count-1]], goalPos, Utils.ZOffset * 1.9f, parent, PathColor, MarkerLineMaterial);
        }

    }


    void CreateDirectedMarkerLines(Vector2 pos, float halfWidth, Color c, float angleRot)
    {
        float lineWidth = 0.01f;
        var parent = Utils.FindOrCreateGameObjectByName(this.gameObject, PathVizGroupName);

        var rot = Quaternion.Euler(0f, angleRot, 0f);

        Vector3 pos3d, p1, p2;
        pos3d = new Vector3(pos.x, 0f, pos.y);

        p1 = new Vector3(halfWidth, 0f, 0f);
        p2 = new Vector3(0, 0f, -halfWidth / 2f);

        p1 = pos3d + rot * p1;
        p2 = pos3d + rot * p2;

        Utils.DrawLine(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);

        p1 = new Vector3(0f,0f, -halfWidth / 2f);
        p2 = new Vector3(0f, 0f, halfWidth / 2f);

        p1 = pos3d + rot * p1;
        p2 = pos3d + rot * p2;

        Utils.DrawLine(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);


        p1 = new Vector3(0f,0f, halfWidth / 2f);
        p2 = new Vector3(halfWidth, 0f, 0f);

        p1 = pos3d + rot * p1;
        p2 = pos3d + rot * p2;

        Utils.DrawLine(new Vector2(p1.x, p1.z), new Vector2(p2.x, p2.z),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);

        


    }


    void CreateMarkerLines(Vector2 pos, float halfWidth, Color c, float lineWidth = 0.04f)
    {
        var parent = Utils.FindOrCreateGameObjectByName(this.gameObject, PathVizGroupName);

        Utils.DrawLine(pos + new Vector2(halfWidth, halfWidth), pos + new Vector2(halfWidth, -halfWidth),
            Utils.ZOffset*2, parent, c, MarkerLineMaterial, lineWidth);

        Utils.DrawLine(pos + new Vector2(halfWidth, -halfWidth), pos + new Vector2(-halfWidth, -halfWidth),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);

        Utils.DrawLine(pos + new Vector2(-halfWidth, -halfWidth), pos + new Vector2(-halfWidth, halfWidth),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);

        Utils.DrawLine(pos + new Vector2(-halfWidth, halfWidth), pos + new Vector2(halfWidth, halfWidth),
            Utils.ZOffset * 2, parent, c, MarkerLineMaterial, lineWidth);

    }


}
