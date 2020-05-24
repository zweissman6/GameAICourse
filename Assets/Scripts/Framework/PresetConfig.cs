using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetConfig : MonoBehaviour
{

    public GameObject ObstaclesGroup;

    public GameObject CubeObstaclePrefab;
    public GameObject SoftStarObstaclePrefab;

    public Obstacles obstacles;


    public DiscretizedSpaceMonoBehavior DiscretizedSpace;

    public MoveBall Agent;


    private GameObject PathNodeMarkersGroup;
    public GameObject WaypointPrefab;

    private string PathNodeMarkersGroupName = "PathNodeMarkersGroup";


    protected List<SceneConfig> SceneConfigs = new List<SceneConfig>();


    private void Start()
    {
        if (obstacles == null)
            Debug.LogError("No obstacles");


        if (ObstaclesGroup == null)
            Debug.LogError("No Obstacles group set");

        if (CubeObstaclePrefab == null)
            Debug.LogError("No cube set");

        if (SoftStarObstaclePrefab == null)
            Debug.LogError("No soft star prefab  set");

        if (DiscretizedSpace == null)
            Debug.LogError("No discretized space set");

        if (Agent == null)
            Debug.LogError("agent (ball) not set");

        if (WaypointPrefab == null)
            Debug.LogError("no waypoint prefab");


        LoadConfig(0);

    }


    bool LoadConfig(int pos)
    {

        if (pos >= 0 && pos < SceneConfigs.Count)
        {
            var sc = SceneConfigs[pos];

            Configure(sc.WorldSize, sc.AgentPos, sc.AgentScale, sc.ObstacleConfigs, sc.PathNodes, sc.GridCellSize, sc.NumExtraPathNodes, sc.Seed);

            return true;
        }

        return false;
    }


    // Update is called once per frame
    void Update()
    {
        int pos = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            pos = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            pos = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            pos = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            pos = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            pos = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            pos = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            pos = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8))
            pos = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            pos = 8;
        if (Input.GetKeyDown(KeyCode.Alpha0))
            pos = 9;


        if (pos > -1 && !LoadConfig(pos))
        {

            Debug.Log("Config " + pos + " doesn't exist!");
        }
    }


    void SetDiscretizedSpaceSize(float scalex, float scalez)
    {

        DiscretizedSpace.transform.localScale = new Vector3(scalex, 1f, scalez);

        var s = DiscretizedSpace.GetComponent<UVAdjustment>();

        if (s != null)
        {
            s.AdjustTextureCoords();
        }
    }


    void PurgeOutdatedWaypoints()
    {

        var gp = GameObject.Find(PathNodeMarkersGroupName);

        if (gp != null)
        {
            gp.name = "MARKED_FOR_DELETION";
            gp.gameObject.SetActive(false);
            Destroy(gp.gameObject);
        }

    }


    protected bool NewPathNodeIsValid(Vector2 np)
    {
        bool isValid = true;

        foreach (var ob in obstacles.getObstacles())
        {
            if (ob.IsPointInPolygon(np))
            {
                isValid = false;
                break;
            }

            if (!isValid)
                break;


            Tuple<Vector2, Vector2>[] boundary = new Tuple<Vector2, Vector2>[4];
            Vector2[] boundaryCorners = new Vector2[4];
            Vector2 wOffset = new Vector2(DiscretizedSpace.Boundary.size.x, 0f);
            Vector2 hOffset = new Vector2(0f, DiscretizedSpace.Boundary.size.z);
            boundaryCorners[0] = DiscretizedSpace.BottomLeftCornerWCS;
            boundaryCorners[1] = DiscretizedSpace.BottomLeftCornerWCS + wOffset;
            boundaryCorners[2] = DiscretizedSpace.BottomLeftCornerWCS + wOffset + hOffset;
            boundaryCorners[3] = DiscretizedSpace.BottomLeftCornerWCS + hOffset;

            for (int b = 0; b < boundaryCorners.Length; ++b)
                boundary[b] = new Tuple<Vector2, Vector2>(boundaryCorners[b], boundaryCorners[(b + 1) % boundaryCorners.Length]);


            //test boundary
            for (int b = 0; b < boundary.Length; ++b)
            {
                var dist = Utils.DistanceToLine(np, boundary[b].Item1, boundary[b].Item2);
                if (dist < Agent.Radius)
                {
                    isValid = false;
                    break;
                }
            }


            float agentRadius = Agent.Radius;

            Vector2[] obstaclePoints = ob.GetPoints();
            for (int l = 0; l < obstaclePoints.Length; l++)
            {

                var a = obstaclePoints[l];
                var b = obstaclePoints[(l + 1) % obstaclePoints.Length];

                //find distance of point to line
                float dist = Utils.DistanceToLine(np, a, b);
                if (dist <= agentRadius)
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid)
                break;

        }

        return isValid;
    }


    protected void PlacePathNodes(int totalNodes, int seed)
    {

        //var seed = Random.Range(0, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        //Debug.Log("SEED IS: " + seed);

  
        PathNodeMarkersGroup = Utils.FindOrCreateGameObjectByName(PathNodeMarkersGroupName);


        var origin = DiscretizedSpace.BottomLeftCornerWCS;
        var width = DiscretizedSpace.Boundary.size.x;
        var height = DiscretizedSpace.Boundary.size.z;

        bool isValid;


        Vector2 posv = Vector2.zero;


        // Try to place some corner points

        var pt = Instantiate(WaypointPrefab, PathNodeMarkersGroup.transform);

        var radMult = 2f;

        //Debug.Log("AGENT RADIUS: " + Agent.Radius);


        var cornerOffset = Agent.Radius * radMult * Vector2.one;
        posv = origin + cornerOffset;
        if (NewPathNodeIsValid(posv))
            pt.transform.position = new Vector3(posv.x, 0f, posv.y);
        else
            Destroy(pt);

        pt = Instantiate(WaypointPrefab, PathNodeMarkersGroup.transform);

        posv = origin + new Vector2(width, height) - cornerOffset;
        if (NewPathNodeIsValid(posv))
            pt.transform.position = new Vector3(posv.x, 0f, posv.y);
        else
            Destroy(pt);

        pt = Instantiate(WaypointPrefab, PathNodeMarkersGroup.transform);

        cornerOffset *= new Vector2(1f, -1f);
        posv = origin + new Vector2(width, 0f) - cornerOffset;
        if (NewPathNodeIsValid(posv))
            pt.transform.position = new Vector3(posv.x, 0f, posv.y);
        else
            Destroy(pt);

        pt = Instantiate(WaypointPrefab, PathNodeMarkersGroup.transform);

        posv = origin + new Vector2(0f, height) + cornerOffset;
        if (NewPathNodeIsValid(posv))
            pt.transform.position = new Vector3(posv.x, 0f, posv.y);
        else
            Destroy(pt);


        for (int i = 0; i < totalNodes; ++i)
        {

            pt = Instantiate(WaypointPrefab, PathNodeMarkersGroup.transform);

            int count = 0;
            const int MaxCount = 100;

            do
            {
                ++count;

                if (count > MaxCount)
                {
                    isValid = false;

                    Debug.Log("Couldn't find a valid position!");
                    break;

                }

                var x = origin.x + UnityEngine.Random.Range(0f, width);
                var y = origin.y + UnityEngine.Random.Range(0f, height);

                posv = new Vector2(x, y);

                isValid = NewPathNodeIsValid(posv);

            } while (!isValid);

            if (isValid)
                pt.transform.position = new Vector3(posv.x, 0f, posv.y);
            else
                Destroy(pt);
        }


    }


    public enum ObstacleType
    {
        Cube,
        SoftStar
    }


    public struct ObstacleConfig
    {
        public ObstacleType otype;
        public Vector2 pos;
        public Vector2 scale;
        public float rot;

        public ObstacleConfig(ObstacleType otype, Vector2 pos, Vector2 scale, float rot)
        {
            this.otype = otype;
            this.pos = pos;
            this.scale = scale;
            this.rot = rot;
        }

    }


    public struct SceneConfig
    {
        public Vector2 WorldSize;
        public Vector2 AgentPos;
        public float AgentScale;
        public ObstacleConfig[] ObstacleConfigs;
        public Vector2[] PathNodes;
        public float GridCellSize;
        public int NumExtraPathNodes;
        public int Seed;

        public SceneConfig(Vector2 worldSize, Vector2 agentPos, float agentScale, ObstacleConfig[] obstacleConfigs, Vector2[] pathNodes, float gridCellSize, int numExtraPathNodes, int seed)
        {
            WorldSize = worldSize;
            AgentPos = agentPos;
            AgentScale = agentScale;
            ObstacleConfigs = obstacleConfigs;
            PathNodes = pathNodes;
            GridCellSize = gridCellSize;
            NumExtraPathNodes = numExtraPathNodes;
            Seed = seed;
        }
    }


 
    GameObject SelectPrefab(ObstacleType ot)
    {
        switch (ot)
        {
            case ObstacleType.Cube:
                return CubeObstaclePrefab;
            case ObstacleType.SoftStar:
                return SoftStarObstaclePrefab;
            default:
                return CubeObstaclePrefab;

        }
    }

    public void Configure(Vector2 worldSize, Vector2 agentPos, float agentScale, ObstacleConfig[] obstacleConfig, Vector2[] pathNodes, float gridCellSize, int numPathNodes, int seed)
    {
        //delete old obstacles
        obstacles.DeleteObstacles();

        PurgeOutdatedWaypoints();

        //add new obstacles
        GameObject o;
        float YPOS = 0f;
        float YSCALE = 1f;
        float XROT = 0f;
        float ZROT = 0f;

        foreach (var oc in obstacleConfig)
        {
            var prefab = SelectPrefab(oc.otype);
            o = Instantiate(prefab, new Vector3(oc.pos.x, YPOS, oc.pos.y), Quaternion.Euler(XROT, oc.rot, ZROT), ObstaclesGroup.transform);
            o.transform.localScale = new Vector3(oc.scale.x, YSCALE, oc.scale.y);
        }    

        // set ground plane size
        SetDiscretizedSpaceSize(worldSize.x, worldSize.y);

        // set grid rez
        var g = DiscretizedSpace as GameGrid;
        if (g != null)
            g.CellSize = gridCellSize;

        // set agent position (also resetting state)
        Agent.CurrentPosition = agentPos;

        Agent.transform.localScale = new Vector3(agentScale, agentScale, agentScale);

        obstacles.Init();

        // set path network



        PathNodeMarkersGroup = Utils.FindOrCreateGameObjectByName(PathNodeMarkersGroupName);


        var pn = DiscretizedSpace as PathNetwork;
        if (pn != null) {

            foreach( var pnode in pathNodes)
            {
                var onode = Instantiate(WaypointPrefab, new Vector3(pnode.x, 0f, pnode.y), Quaternion.identity, PathNodeMarkersGroup.transform);
            }

            if(numPathNodes > 0)
                PlacePathNodes(numPathNodes, seed);
        }

        DiscretizedSpace.Bake();

    }


 


    


}
