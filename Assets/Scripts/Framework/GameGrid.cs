using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameAICourse;

public class GameGrid : DiscretizedSpaceMonoBehavior
{

    public float CellSize = 0.2f;
    public bool[,] Grid { get; protected set; }

    public Color LineColor = Color.green;
    public Color BlockedLineColor = Color.blue;

    public Material LineMaterial;        //Material used to draw the lines for the grid



    public override void Awake()
    {
        base.Awake();

        Obstacles = this.GetComponent<Obstacles>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Obstacles.Init();

        Bake();

        Utils.DisplayName("CreateGrid", CreateGrid.StudentAuthorName);

    }


    override public void Bake()
    {

        bool[,] grid;
        List<Vector2> pathNodes;
        List<List<int>> pathEdges;

        CreateGrid.Create(BottomLeftCornerWCS, Boundary.size.x, Boundary.size.z, CellSize,
                            Obstacles.getObstacles(), out grid, out pathNodes, out pathEdges);

        Grid = grid;
        PathNodes = pathNodes;
        PathEdges = pathEdges;

        if (grid != null)
        {
            CreateGridLines(Grid, CellSize, BottomLeftCornerWCS);
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

    //
    // Draws a square for each box of the grid that exists
    // size = This is the number of cells in the grid
    // canvas_pos = This is the where the top left corner of the canvas is
    // lengthX, lengthY = this is the width and height of the plane
    //


    void CreateGridLines(bool[,] grid, float cellSize, Vector2 canvas_pos)
    {
        PurgeOutdatedLineViz();

        var parent = Utils.FindOrCreateGameObjectByName(this.gameObject, Utils.LineGroupName);

        float width = grid.GetLength(0) * cellSize;
        float height = grid.GetLength(1) * cellSize;

        for (int i = 0; i <= grid.GetLength(0); i++)
        {
            Utils.DrawLine(canvas_pos + new Vector2(i * cellSize, 0f), canvas_pos + new Vector2(i * cellSize, height), Utils.ZOffset, parent, LineColor, LineMaterial);
        }

        for (int j = 0; j <= grid.GetLength(1); j++)
        {
            Utils.DrawLine(canvas_pos + new Vector2(0f, j * cellSize), canvas_pos + new Vector2(width, j * cellSize), Utils.ZOffset, parent, LineColor, LineMaterial);
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {

                float lX = i * cellSize;
                float rX = (i + 1) * cellSize;
                float tY = j * cellSize;
                float bY = (j + 1) * cellSize;

                if (!grid[i, j])
                {
                    Utils.DrawLine(canvas_pos + new Vector2(lX, tY), canvas_pos + new Vector2(rX, bY), Utils.ZOffset, parent, BlockedLineColor, LineMaterial);
                    Utils.DrawLine(canvas_pos + new Vector2(rX, tY), canvas_pos + new Vector2(lX, bY), Utils.ZOffset, parent, BlockedLineColor, LineMaterial);
                }

            }
        }


    }


    //
    // Returns cell location corresponding to a particular point
    //
    public Vector2 FindGridCellForPoint(Vector2 point)
    {
        Vector2 diff = point - BottomLeftCornerWCS;
        return new Vector2(diff.x / CellSize, diff.y / CellSize);
    }

    /*
     * Returns the location of the center point of a grid cell
     */
    public Vector2 FindCenterOfGridCell(int i, int j)
    {
        Vector2 local = new Vector2(i * CellSize, j * CellSize);
        local = local + new Vector2(CellSize / 2f, CellSize / 2f);
        return local + BottomLeftCornerWCS;
    }



}
